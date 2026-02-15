
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

def find_all_opcode_refs(file_path):
    base, sections = get_pe_info(file_path)
    with open(file_path, 'rb') as f:
        data = f.read()

    pattern = re.compile(b'[CS]_[A-Za-z0-9_]{3,64}\x00')
    opcode_strings = []
    for m in pattern.finditer(data):
        name = m.group().decode('ascii', errors='ignore').strip('\x00')
        fo = m.start()
        rva = file_offset_to_rva(sections, fo)
        if rva:
            opcode_strings.append({'name': name, 'fo': fo, 'rva': rva})

    rva_to_name = {}
    for o in opcode_strings:
        rva_bytes = struct.pack('<I', o['rva'])
        rva_to_name[rva_bytes] = o['name']

    all_refs = []
    for i in range(0, len(data) - 4):
        chunk = data[i:i+4]
        if chunk in rva_to_name:
            all_refs.append({'fo': i, 'name': rva_to_name[chunk], 'rva': struct.unpack('<I', chunk)[0]})

    print(f"Found {len(all_refs)} RVA references:")
    for r in all_refs:
        print(f"0x{r['fo']:08X} -> {r['name']} (RVA 0x{r['rva']:X})")
        # Dump around the reference
        d = data[max(0, r['fo']-16):r['fo']+32]
        print(f"  Dump: {' '.join(f'{b:02X}' for b in d)}")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    find_all_opcode_refs(path)
