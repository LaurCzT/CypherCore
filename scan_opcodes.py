
import struct

def scan_patterns(file_path):
    with open(file_path, 'rb') as f:
        data = f.read()
    
    known_opcodes = {
        'AuthSession': 0x3765,
        'AuthContinuedSession': 0x3766,
        'CastSpell': 0x3294,
        'AcceptGuildInvite': 0x35Fd,
        'Ping': 0x3768,
        'PlayerLogin': 0x35Eb,
    }
    
    for name, op in known_opcodes.items():
        # Instruction: mov edx, imm32 (BA + 4 bytes)
        pattern = b'\xBA' + struct.pack('<I', op)
        
        offsets = []
        start = 0
        while True:
            idx = data.find(pattern, start)
            if idx == -1: break
            offsets.append(idx)
            start = idx + 1
            
        if offsets:
            print(f"Pattern for {name} (0x{op:X}) found at: " + ", ".join(hex(o) for o in offsets))
            for off in offsets:
                # Dump context: 16 bytes before, 32 bytes after
                d = data[max(0, off-16):off+32]
                print(f"  Dump at 0x{off:X}: {' '.join(f'{b:02X}' for b in d)}")
        else:
            print(f"Pattern for {name} (0x{op:X}) not found.")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    scan_patterns(path)
