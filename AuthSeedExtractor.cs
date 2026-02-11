
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class AuthSeedExtractor {
    static void Main(string[] args) {
        string path = @"D:\Classic\World of Warcraft\_classic_era_\WowClassic.exe";
        if (args.Length > 0) path = args[0];

        if (!File.Exists(path)) {
            Console.WriteLine("File not found: {0}", path);
            return;
        }

        Console.WriteLine("Scanning: {0}", path);
        byte[] data = File.ReadAllBytes(path);

        byte[] AuthCheckSeed = new byte[] { 0xC5, 0xC6, 0x98, 0x95, 0x76, 0x3F, 0x1D, 0xCD, 0xB6, 0xA1, 0x37, 0x28, 0xB3, 0x12, 0xFF, 0x8A };
        
        int offset = -1;
        for (int i = 0; i <= data.Length - AuthCheckSeed.Length; i++) {
            bool match = true;
            for (int j = 0; j < AuthCheckSeed.Length; j++) {
                if (data[i + j] != AuthCheckSeed[j]) {
                    match = false;
                    break;
                }
            }
            if (match) {
                offset = i;
                break;
            }
        }

        if (offset == -1) {
            Console.WriteLine("Could not find AuthCheckSeed pattern. This client might use a different protocol version.");
            return;
        }

        Console.WriteLine("Found AuthHandshake structure at 0x{0:X}", offset);
        Console.WriteLine("--------------------------------------------------");
        
        Console.WriteLine("Nearby Constants:");
        PrintBlock(data, offset - 64, "Unknown Block 1 (-64)");
        PrintBlock(data, offset - 48, "Potential AuthSeed (-48)");
        PrintBlock(data, offset - 32, "Unknown Block 2 (-32)");
        PrintBlock(data, offset - 16, "Warden/Meta String (-16)");
        PrintBlock(data, offset,      "AuthCheckSeed (Reference)");
        PrintBlock(data, offset + 16, "ContinuedSessionSeed");
        PrintBlock(data, offset + 32, "SessionKeySeed");
        PrintBlock(data, offset + 48, "EncryptionKeySeed");
        PrintBlock(data, offset + 64, "EnableEncryptionSeed");
        Console.WriteLine("--------------------------------------------------");

        Console.WriteLine("\nSuggested config update:");
        string potentialSeed = BitConverter.ToString(data, offset - 48, 16).Replace("-", "");
        Console.WriteLine("UPDATE build_info SET win64AuthSeed = '{0}', mac64AuthSeed = '{0}' WHERE build = 40618;", potentialSeed);
    }

    static void PrintBlock(byte[] data, int offset, string label) {
        if (offset < 0 || offset + 16 > data.Length) return;
        string hex = BitConverter.ToString(data, offset, 16).Replace("-", "");
        Console.WriteLine("{0} [0x{1:X}]: {2}", label.PadRight(25), offset, hex);
    }
}
