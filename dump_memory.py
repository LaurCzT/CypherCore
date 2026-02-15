
import struct

def dump_around(file_path, offset, size=128):
    try:
        with open(file_path, 'rb') as f:
            f.seek(max(0, offset - size))
            data = f.read(size * 2)
            start_addr = max(0, offset - size)
            
            print(f"Dump around 0x{offset:X}:")
            for i in range(0, len(data), 16):
                hex_vals = ' '.join(f"{b:02X}" for b in data[i:i+16])
                ascii_vals = ''.join(chr(b) if 32 <= b <= 126 else '.' for b in data[i:i+16])
                print(f"0x{start_addr + i:08X}: {hex_vals:<48} | {ascii_vals}")
    except Exception as e:
        print(f"Error: {e}")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    dump_around(path, 0x27ED470, 128)
