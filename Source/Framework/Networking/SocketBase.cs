// Copyright (c) CypherCore <http://github.com/CypherCore> All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Framework.Networking
{
    public interface ISocket
    {
        void Start();
        bool Update();
        bool IsOpen();
        void CloseSocket();
    }

    public abstract class SocketBase : ISocket, IDisposable
    {
        Socket _socket;
        IPEndPoint _remoteIPEndPoint;
        SocketAsyncEventArgs receiveSocketAsyncEventArgsWithCallback;
        SocketAsyncEventArgs receiveSocketAsyncEventArgs;
        // send queue and async send state to avoid blocking sends
        SocketAsyncEventArgs sendSocketAsyncEventArgs;
        readonly object _sendLock = new();
        readonly LinkedList<byte[]> _sendQueue = new();
        bool _sending;
        // queued bytes accounting to avoid unbounded memory growth when client is slow
        long _queuedBytes;
        const long MaxQueuedBytes = 8 * 1024 * 1024; // 8 MB
        // simple send rate limiting to avoid flooding client during heavy login
        long _bytesSentWindow;
        DateTime _windowStart = DateTime.UtcNow;
        const int SendWindowMs = 1000;
        // configurable send bytes per window (bytes/sec) default 100KB/s
        long MaxSendBytesPerWindow = 100 * 1024; // 100 KB/s default

        public void SetMaxSendBytesPerWindow(long bytesPerSec)
        {
            lock (_sendLock)
                MaxSendBytesPerWindow = bytesPerSec;
        }
        System.Threading.Timer _sendResumeTimer;

        public delegate void SocketReadCallback(SocketAsyncEventArgs args);

        protected SocketBase(Socket socket)
        {
            _socket = socket;
            _remoteIPEndPoint = (IPEndPoint)_socket.RemoteEndPoint;

            receiveSocketAsyncEventArgsWithCallback = new SocketAsyncEventArgs();
            receiveSocketAsyncEventArgsWithCallback.SetBuffer(new byte[0x4000], 0, 0x4000);

            receiveSocketAsyncEventArgs = new SocketAsyncEventArgs();
            receiveSocketAsyncEventArgs.SetBuffer(new byte[0x4000], 0, 0x4000);
            receiveSocketAsyncEventArgs.Completed += (sender, args) => ProcessReadAsync(args);

            sendSocketAsyncEventArgs = new SocketAsyncEventArgs();
            sendSocketAsyncEventArgs.Completed += (sender, args) => ProcessSendAsync(args);
        }

        public virtual void Dispose()
        {
            _socket.Dispose();
        }

        public virtual void Start() { }

        public virtual bool Update()
        {
            return IsOpen();
        }

        public IPEndPoint GetRemoteIpAddress()
        {
            return _remoteIPEndPoint;
        }

        public void AsyncReadWithCallback(SocketReadCallback callback)
        {
            if (!IsOpen())
                return;

            receiveSocketAsyncEventArgsWithCallback.Completed += (sender, args) => callback(args);
            receiveSocketAsyncEventArgsWithCallback.SetBuffer(0, 0x4000);
            if (!_socket.ReceiveAsync(receiveSocketAsyncEventArgsWithCallback))
                callback(receiveSocketAsyncEventArgsWithCallback);
        }

        public void AsyncRead()
        {
            if (!IsOpen())
                return;

            receiveSocketAsyncEventArgs.SetBuffer(0, 0x4000);
            if (!_socket.ReceiveAsync(receiveSocketAsyncEventArgs))
                ProcessReadAsync(receiveSocketAsyncEventArgs);
        }

        void ProcessReadAsync(SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success)
            {
                CloseSocket();
                return;
            }

            if (args.BytesTransferred == 0)
            {
                CloseSocket();
                return;
            }

            ReadHandler(args);
        }

        public abstract void ReadHandler(SocketAsyncEventArgs args);

        public void AsyncWrite(byte[] data)
        {
            if (!IsOpen() || data == null || data.Length == 0)
                return;

            lock (_sendLock)
            {
                // account queued bytes
                _sendQueue.AddLast(data);
                _queuedBytes += data.Length;

                // safety: if queued bytes exceed threshold, disconnect slow client to protect server
                if (_queuedBytes > MaxQueuedBytes)
                {
                    Log.outError(LogFilter.Network, $"SocketBase.AsyncWrite: closing connection {GetRemoteIpAddress()} due to excessive send queue ({_queuedBytes} bytes)");
                    // clear queue to free memory
                    _sendQueue.Clear();
                    _queuedBytes = 0;
                    // closing socket will drop pending sends
                    CloseSocket();
                    return;
                }

                if (!_sending)
                    StartSendNext();
            }
        }

        void StartSendNext()
        {
            // assumes caller holds _sendLock
            if (_sendQueue.Count == 0)
            {
                _sending = false;
                return;
            }

            byte[] buffer = _sendQueue.First.Value;
            _sendQueue.RemoveFirst();
            _sending = true;
            // adjust queued bytes because we're about to send this buffer
            try { _queuedBytes -= buffer.Length; } catch { _queuedBytes = 0; }

            try
            {
                // SetBuffer pins buffer internally; to avoid GC issues we don't reuse the array in pool
                sendSocketAsyncEventArgs.SetBuffer(buffer, 0, buffer.Length);
                if (!_socket.SendAsync(sendSocketAsyncEventArgs))
                {
                    // completed synchronously
                    ProcessSendAsync(sendSocketAsyncEventArgs);
                }
            }
            catch (ObjectDisposedException)
            {
                // socket closed
                _sending = false;
            }
            catch (Exception ex)
            {
                Log.outDebug(LogFilter.Network, $"SocketBase.AsyncWrite: send error {ex.Message}");
                _sending = false;
            }
        }

        void ProcessSendAsync(SocketAsyncEventArgs args)
        {
            // called on send completion (async or sync)
            if (args.SocketError != SocketError.Success)
            {
                CloseSocket();
                return;
            }

            lock (_sendLock)
            {
                // clear buffer reference to allow GC
                try { sendSocketAsyncEventArgs.SetBuffer(null, 0, 0); } catch { }
                if (_sendQueue.Count > 0)
                    StartSendNext();
                else
                    _sending = false;
            }
        }

        public void CloseSocket()
        {
            if (_socket == null || !_socket.Connected)
                return;

            try
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
            }
            catch (Exception ex)
            {
                Log.outDebug(LogFilter.Network, $"WorldSocket.CloseSocket: {GetRemoteIpAddress()} errored when shutting down socket: {ex.Message}");
            }

            OnClose();
        }

        public virtual void OnClose() { Dispose(); }

        public bool IsOpen() { return _socket.Connected; }

        public void SetNoDelay(bool enable)
        {
            _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, enable);
        }
    }
}
