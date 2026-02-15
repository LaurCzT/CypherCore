
import struct
import re

def get_pe_info(file_path):
    with open(file_path, 'rb') as f:
        f.seek(0x3C)
        pe_offset = struct.unpack('<I', f.read(4))[0]
        f.seek(pe_offset + 4)
        machine, num_sections, timedate, sym_ptr, num_sym, size_of_opt, characteristics = struct.unpack('<HHIIIHH', f.read(20))
        optional_header_offset = f.tell()
        f.seek(optional_header_offset)
        magic = struct.unpack('<H', f.read(2))[0]
        if magic == 0x20b: # PE32+
            f.seek(optional_header_offset + 24)
            image_base = struct.unpack('<Q', f.read(8))[0]
        else:
            f.seek(optional_header_offset + 28)
            image_base = struct.unpack('<I', f.read(4))[0]
        f.seek(optional_header_offset + size_of_opt)
        sections = []
        for _ in range(num_sections):
            name_bytes = f.read(8)
            name = name_bytes.split(b'\0')[0].decode('ascii', errors='ignore')
            v_size, v_addr, r_size, r_ptr, r_reloc, r_line, n_reloc, n_line, characteristics = struct.unpack('<IIIIIIHHI', f.read(32))
            sections.append({'name': name, 'va': v_addr, 'vs': v_size, 'rp': r_ptr, 'rs': r_size})
    return image_base, sections

def file_offset_to_rva(sections, file_offset):
    for s in sections:
        if s['rp'] <= file_offset < s['rp'] + s['rs']:
            return s['va'] + (file_offset - s['rp'])
    return None

def find_all_va_refs_unaligned(file_path):
    base, sections = get_pe_info(file_path)
    with open(file_path, 'rb') as f:
        data = f.read()

    # Targets: C_AuthChallenge and CastSpell (the ones we know for sure)
    targets = [
        {'name': 'CastSpell', 'fo': 0x27AC408},
        {'name': 'C_AuthChallenge', 'fo': 0x27ED470},
    ]
    
    for t in targets:
        rva = file_offset_to_rva(sections, t['fo'])
        if not rva: continue
        va = base + rva
        va_bytes = struct.pack('<Q', va)
        
        offsets = []
        start = 0
        while True:
            idx = data.find(va_bytes, start)
            if idx == -1: break
            offsets.append(idx)
            start = idx + 1
        
        if offsets:
            print(f"Found {len(offsets)} VA references for {t['name']} (VA: 0x{va:X}) at: " + ", ".join(hex(o) for o in offsets))
        else:
            print(f"No VA references for {t['name']} (VA: 0x{va:X})")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    find_all_va_refs_unaligned(path)
