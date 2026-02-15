
import re

def extract_opcode_strings_near(file_path, target_offset, window=1024):
    with open(file_path, 'rb') as f:
        f.seek(max(0, target_offset - window))
        data = f.read(window * 2)
    
    # Broaden regex to find any string that looks like an opcode name
    # They don't always have C_ or S_ prefix, but usually they are CamelCase.
    # Let's try to find potential names within the window.
    pattern = re.compile(b'[A-Z][a-zA-Z0-9_]{5,64}\x00')
    
    matches = []
    base_offset = max(0, target_offset - window)
    for m in pattern.finditer(data):
        s = m.group().decode('ascii', errors='ignore').strip('\x00')
        matches.append({'name': s, 'fo': base_offset + m.start()})
    
    print(f"Strings near 0x{target_offset:X}:")
    for m in matches:
        print(f"0x{m['fo']:08X}: {m['name']}")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    # CastSpell near 0x27AC408
    extract_opcode_strings_near(path, 0x27AC408)
