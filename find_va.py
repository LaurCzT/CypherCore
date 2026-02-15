
import struct

def find_va(file_path, va):
    with open(file_path, 'rb') as f:
        data = f.read()
    va_bytes = struct.pack('<Q', va)
    idx = data.find(va_bytes)
    if idx != -1:
        print(f"Found VA 0x{va:X} at offset 0x{idx:X}")
        return idx
    else:
        print(f"VA 0x{va:X} not found")
        return None

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    # VA for C_AuthChallenge (RVA: 0x27EEE70)
    find_va(path, 0x1427EEE70)
