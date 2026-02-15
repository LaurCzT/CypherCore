
import sys

def dump_hex(file_path, offset, size=64):
    try:
        with open(file_path, 'rb') as f:
            f.seek(max(0, offset - size))
            data = f.read(size * 2 + 1) # Read before and after
            start_addr = max(0, offset - size)
            
            print(f"Dump around 0x{offset:X} (context: +-{size} bytes):")
            for i in range(0, len(data), 16):
                hex_vals = ' '.join(f"{b:02X}" for b in data[i:i+16])
                ascii_vals = ''.join(chr(b) if 32 <= b <= 126 else '.' for b in data[i:i+16])
                print(f"0x{start_addr + i:08X}: {hex_vals:<48} | {ascii_vals}")
    except Exception as e:
        print(f"Error: {e}")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    # Offset of 'AuthChallenge' found previously: 0x27ED472
    dump_hex(path, 0x27ED472, 64)
