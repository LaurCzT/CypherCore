
import struct

def scan_mov_patterns(file_path):
    with open(file_path, 'rb') as f:
        data = f.read()
    
    known_opcodes = {
        'AuthSession': 0x3765,
        'AuthContinuedSession': 0x3766,
        'CastSpell': 0x3294,
    }
    
    # Registers: EAX (B8), ECX (B9), EDX (BA), EBX (BB), ESP (BC), EBP (BD), ESI (BE), EDI (BF)
    regs = {'EAX': b'\xB8', 'ECX': b'\xB9', 'EDX': b'\xBA', 'EBX': b'\xBB', 'ESI': b'\xBE', 'EDI': b'\xBF'}
    
    for name, op in known_opcodes.items():
        op_bytes = struct.pack('<I', op)
        for r_name, prefix in regs.items():
            pattern = prefix + op_bytes
            idx = data.find(pattern)
            if idx != -1:
                print(f"Found {name} (0x{op:X}) in {r_name} at file offset 0x{idx:X}")
                # Dump context
                d = data[max(0, idx-16):idx+32]
                print(f"  Dump: {' '.join(f'{b:02X}' for b in d)}")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    scan_mov_patterns(path)
