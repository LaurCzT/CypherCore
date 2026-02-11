
using System;
using System.IO;
using System.Linq;

class Scanner {
    static void Main() {
        string path = @"D:\Classic\World of Warcraft\_classic_era_\WowClassic.exe";
        byte[] data = File.ReadAllBytes(path);
        byte[] pattern = { 0x92, 0x9E, 0x00, 0x00 }; // 40618

        for (int i = 0; i <= data.Length - 16; i++) {
            if (data[i] == pattern[0] && data[i+1] == pattern[1] && data[i+2] == pattern[2] && data[i+3] == pattern[3]) {
                Console.WriteLine($"Found build 40618 at 0x{i:X}");
                for (int k = -128; k <= 128; k += 16) {
                    int offset = i + k;
                    if (offset >= 0 && offset + 16 <= data.Length) {
                        string hex = BitConverter.ToString(data, offset, 16).Replace("-", "");
                        Console.WriteLine($"  Offset {k}: {hex} at 0x{offset:X}");
                    }
                }
            }
        }
    }
}
