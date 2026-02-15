
import re

def find_jam_strings(file_path):
    with open(file_path, 'rb') as f:
        data = f.read()
    
    # Search for Jam:: with anything after, null terminated
    pattern = re.compile(b'Jam::[A-Za-z0-9_:]+\x00')
    
    matches = []
    for m in pattern.finditer(data):
        s = m.group().decode('ascii', errors='ignore').strip('\x00')
        matches.append({'name': s, 'fo': m.start()})
    
    print(f"Found {len(matches)} Jam:: strings.")
    for m in matches[:50]:
        print(f"0x{m['fo']:08X}: {m['name']}")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    find_jam_strings(path)
