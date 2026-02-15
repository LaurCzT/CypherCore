
import struct

def find_sequences(file_path):
    with open(file_path, 'rb') as f:
        data = f.read()
    
    # Try common packet IDs in 1.14.x
    # AuthSession: 0x3765
    # AuthContinuedSession: 0x3766
    
    op1 = struct.pack('<I', 0x3765)
    op2 = struct.pack('<I', 0x3766)
    
    offsets1 = []
    start = 0
    while True:
        idx = data.find(op1, start)
        if idx == -1: break
        offsets1.append(idx)
        start = idx + 1
        
    for o1 in offsets1:
        # Look for op2 within 64 bytes
        idx2 = data.find(op2, o1 - 64, o1 + 64)
        if idx2 != -1:
            print(f"FOUND SEQUENCE: 0x3765 at 0x{o1:X}, 0x3766 at 0x{idx2:X}")
            d = data[min(o1, idx2)-32:max(o1, idx2)+64]
            print(f"  Dump: {' '.join(f'{b:02X}' for b in d)}")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    find_sequences(path)
    # Also dump around 'Jam'
    from dump_memory import dump_around
    print("\n--- Dump around 'Jam' (0x2744838) ---")
    dump_around(path, 0x2744838, 128)
