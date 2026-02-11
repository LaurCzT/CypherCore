
using System;
using System.IO;
using System.Linq;

class PatternFinder {
    static void Main(string[] args) {
        string path = @"D:\Classic\World of Warcraft\_classic_era_\WowClassic.exe";
        byte[] pattern = { 0x12, 0x78, 0xEB, 0x34, 0xF2, 0x43, 0xED, 0x78, 0x98, 0xD6, 0x14, 0xC0, 0xE2, 0x78, 0xEA, 0xC0 };
        
        if (!File.Exists(path)) {
            Console.WriteLine("File not found");
            return;
        }

        byte[] data = File.ReadAllBytes(path);
        for (int i = 0; i <= data.Length - 16; i++) {
            bool match = true;
            for (int j = 0; j < 16; j++) {
                if (data[i + j] != pattern[j]) {
                    match = false;
                    break;
                }
            }
            if (match) {
                Console.WriteLine("Found at 0x{0:X}", i);
                return;
            }
        }
        Console.WriteLine("Not found");
    }
}
