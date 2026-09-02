// Copyright (c) CypherCore <http://github.com/CypherCore> All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

class AuthSeedExtractor
{
    const int PROCESS_QUERY_INFORMATION = 0x0400;
    const int PROCESS_VM_READ = 0x0010;

    const uint MEM_COMMIT = 0x1000;
    const uint PAGE_NOACCESS = 0x01;
    const uint PAGE_GUARD = 0x100;

    [DllImport("kernel32.dll")]
    static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, IntPtr dwSize, out IntPtr lpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_BASIC_INFORMATION
    {
        public IntPtr BaseAddress;
        public IntPtr AllocationBase;
        public uint AllocationProtect;
        public IntPtr RegionSize;
        public uint State;
        public uint Protect;
        public uint Type;
    }

    static readonly byte[] AuthCheckSeed = new byte[] { 0xC5, 0xC6, 0x98, 0x95, 0x76, 0x3F, 0x1D, 0xCD, 0xB6, 0xA1, 0x37, 0x28, 0xB3, 0x12, 0xFF, 0x8A };
    static readonly byte[] SessionKeySeed = new byte[] { 0x58, 0xCB, 0xCF, 0x40, 0xFE, 0x2E, 0xCE, 0xA6, 0x5A, 0x90, 0xB8, 0x01, 0x68, 0x6C, 0x28, 0x0B };

    static void Main(string[] args)
    {
        Console.WriteLine("CypherCore AuthSeed Extractor");
        Console.WriteLine("-----------------------------");
        Console.WriteLine("1. Scan a file (Wow.exe / WowClassic.exe)");
        Console.WriteLine("2. Scan a running process (Memory Scan)");
        Console.Write("Select option (1/2): ");
        string? choice = Console.ReadLine()?.Trim();

        if (choice == "2")
        {
            ScanProcess();
        }
        else
        {
            ScanFile(args);
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static void ScanFile(string[] args)
    {
        string path = "";
        if (args.Length > 0)
        {
            path = args[0];
        }
        else
        {
            Console.WriteLine("Please enter the full path to Wow.exe or WowClassic.exe (you can drag and drop the file here):");
            Console.Write("> ");
            path = Console.ReadLine() ?? "";
            
            if (!string.IsNullOrWhiteSpace(path))
            {
                path = path.Trim('"');
            }
        }

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            Console.WriteLine("File not found: {0}", string.IsNullOrWhiteSpace(path) ? "empty path" : path);
            return;
        }

        Console.WriteLine("Scanning file: {0}", path);
        byte[] data = File.ReadAllBytes(path);
        
        if (!SearchSeedBlock(data, 0, "file offset"))
        {
            Console.WriteLine("Could not find AuthCheckSeed or SessionKeySeed. This client might use a completely different protocol version or is encrypted.");
        }
    }

    static void ScanProcess()
    {
        Console.WriteLine("Looking for Wow.exe or WowClassic.exe process...");
        Process[] processes = Process.GetProcessesByName("WowClassic");
        if (processes.Length == 0)
            processes = Process.GetProcessesByName("Wow");

        if (processes.Length == 0)
        {
            Console.WriteLine("Process not found! Please ensure the game is running.");
            return;
        }

        Process wow = processes[0];
        Console.WriteLine("Found process: {0} (PID: {1})", wow.ProcessName, wow.Id);

        IntPtr handle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, wow.Id);
        if (handle == IntPtr.Zero)
        {
            Console.WriteLine("Failed to open process. Try running this tool as Administrator.");
            return;
        }

        Console.WriteLine("Scanning memory. This might take a few seconds...");

        ulong address = 0;
        ulong maxAddress = (ulong)(IntPtr.Size == 8 ? 0x7FFFFFFFFFF : 0x7FFFFFFF);

        while (address < maxAddress)
        {
            int result = VirtualQueryEx(handle, (IntPtr)address, out MEMORY_BASIC_INFORMATION memInfo, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION)));
            if (result == 0) break;

            if (memInfo.State == MEM_COMMIT && 
                (memInfo.Protect & PAGE_NOACCESS) == 0 && 
                (memInfo.Protect & PAGE_GUARD) == 0)
            {
                // Only scan blocks larger than our seed search size to avoid out of bounds
                long regionSize = (long)memInfo.RegionSize;
                if (regionSize > 128)
                {
                    byte[] buffer = new byte[regionSize];
                    IntPtr bytesRead;
                    if (ReadProcessMemory(handle, (IntPtr)address, buffer, (IntPtr)regionSize, out bytesRead))
                    {
                        if (SearchSeedBlock(buffer, address, "memory address"))
                        {
                            Console.WriteLine("Scan complete.");
                            return;
                        }
                    }
                }
            }

            address = (ulong)memInfo.BaseAddress + (ulong)memInfo.RegionSize;
        }

        Console.WriteLine("Scan complete. Pattern not found in memory. Blizzard might have changed the seeds entirely.");
    }

    static bool SearchSeedBlock(byte[] data, ulong baseAddress, string addressType)
    {
        int offset = -1;
        for (int i = 0; i <= data.Length - AuthCheckSeed.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < AuthCheckSeed.Length; j++)
            {
                if (data[i + j] != AuthCheckSeed[j])
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                offset = i;
                break;
            }
        }

        if (offset == -1)
        {
            int sessionOffset = -1;
            for (int i = 0; i <= data.Length - SessionKeySeed.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < SessionKeySeed.Length; j++)
                {
                    if (data[i + j] != SessionKeySeed[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    sessionOffset = i;
                    break;
                }
            }
            
            if (sessionOffset != -1)
            {
                offset = sessionOffset - 32;
            }
        }

        if (offset == -1)
            return false;

        Console.WriteLine("Found AuthHandshake structure at {0} 0x{1:X}", addressType, baseAddress + (ulong)offset);
        Console.WriteLine("--------------------------------------------------");

        Console.WriteLine("Nearby Constants:");
        PrintBlock(data, offset - 64, "Unknown Block 1 (-64)");
        PrintBlock(data, offset - 48, "Potential AuthSeed (-48)");
        PrintBlock(data, offset - 32, "Unknown Block 2 (-32)");
        PrintBlock(data, offset - 16, "Warden/Meta String (-16)");
        PrintBlock(data, offset, "AuthCheckSeed (Reference)");
        PrintBlock(data, offset + 16, "ContinuedSessionSeed");
        PrintBlock(data, offset + 32, "SessionKeySeed");
        PrintBlock(data, offset + 48, "EncryptionKeySeed");
        PrintBlock(data, offset + 64, "EnableEncryptionSeed");
        Console.WriteLine("--------------------------------------------------");

        Console.WriteLine("\nSuggested config update:");
        string potentialSeed = BitConverter.ToString(data, offset - 48, 16).Replace("-", "");
        Console.WriteLine("UPDATE build_info SET win64AuthSeed = '{0}', mac64AuthSeed = '{0}' WHERE build = <INSERT_BUILD>;", potentialSeed);
        return true;
    }

    static void PrintBlock(byte[] data, int offset, string label)
    {
        if (offset < 0 || offset + 16 > data.Length) return;
        string hex = BitConverter.ToString(data, offset, 16).Replace("-", "");
        Console.WriteLine("{0} [0x{1:X}]: {2}", label.PadRight(25), offset, hex);
    }
}