
using System;
using System.Security.Cryptography;
using System.Linq;
using System.Text;

class AuthTester {
    static void Main() {
        string sessionKeyHex = "A1DFF1885A466A28C62E656E1FDBFFB0BCC06ACEAC7F6783A68F428182C9072FF4F7189F3A31E45544054869E6EF624738C7143B209F209F5A75EC677E0DFC79"; 
        string localChallengeHex = "97490199A994AE1A5B97CE41A2A1235B"; 
        string serverChallengeHex = "B24FB0E5A637A2777A3904896C35BC6F"; 
        string clientDigestHex = "204CE779CE0948D91083E60F625C7563F566603FDAB55D93"; 

        byte[] sessionKey = StringToByteArray(sessionKeyHex);
        byte[] localChallenge = StringToByteArray(localChallengeHex);
        byte[] serverChallenge = StringToByteArray(serverChallengeHex);
        byte[] clientDigest = StringToByteArray(clientDigestHex);

        Console.WriteLine("Session Key len: " + sessionKey.Length);
        Console.WriteLine("Client Digest len: " + clientDigest.Length);

        string[] seeds = {
            "C5FA0568360A861A178AB9B49BB24B7F",
            "C5C69895763F1DCDB6A13728B312FF8A",
            "58CBCF40FE2ECEA65A90B801686C280B",
            "E9753C50909361DA3B07EEFAFF9D41B8",
            "1278EB34F243ED7898D614C0E278EAC0",
            "7528AB80D693E149907757BC9540A6A6"
        };

        foreach (var s1 in seeds) {
            foreach (var s2 in seeds) {
                byte[] seed1 = StringToByteArray(s1);
                byte[] seed2 = StringToByteArray(s2);

                // Combination 1: SHA256(Key, Seed1) -> HMAC(Local, Server, Seed2)
                if (Test(sessionKey, localChallenge, serverChallenge, seed1, seed2, clientDigest, "C1")) return;
                
                // Combination 2: SHA256(Key, Seed1) -> HMAC(Server, Local, Seed2)
                if (Test(sessionKey, serverChallenge, localChallenge, seed1, seed2, clientDigest, "C2 (Swapped)")) return;

                // Combination 3: SHA256(Key) -> HMAC(Local, Server, Seed1, Seed2)
                // (Try adding both seeds to HMAC)
            }
        }
        Console.WriteLine("No match found.");
    }

    static bool Test(byte[] sessionKey, byte[] c1, byte[] c2, byte[] s1, byte[] s2, byte[] target, string label) {
        byte[] key;
        using (var sha = SHA256.Create()) {
            sha.TransformBlock(sessionKey, 0, sessionKey.Length, sessionKey, 0);
            sha.TransformFinalBlock(s1, 0, s1.Length);
            key = sha.Hash;
        }

        using (var hmac = new HMACSHA256(key)) {
            hmac.TransformBlock(c1, 0, c1.Length, c1, 0);
            hmac.TransformBlock(c2, 0, c2.Length, c2, 0);
            hmac.TransformFinalBlock(s2, 0, s2.Length);
            byte[] digest = hmac.Hash;

            if (Compare(digest, target, 24)) {
                Console.WriteLine("MATCH FOUND! " + label);
                Console.WriteLine("Seed1 (for key): " + BitConverter.ToString(s1).Replace("-", ""));
                Console.WriteLine("Seed2 (for hmac): " + BitConverter.ToString(s2).Replace("-", ""));
                return true;
            }
        }
        return false;
    }

    static byte[] StringToByteArray(string hex) {
        if (hex.Length % 2 != 0) hex = "0" + hex;
        byte[] res = new byte[hex.Length / 2];
        for (int i = 0; i < res.Length; i++) {
            res[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return res;
    }

    static bool Compare(byte[] a, byte[] b, int length) {
        if (a.Length < length || b.Length < length) return false;
        for (int i = 0; i < length; i++)
            if (a[i] != b[i]) return false;
        return true;
    }
}
