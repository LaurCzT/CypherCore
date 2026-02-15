
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

def find_all_va_refs(file_path):
    base, sections = get_pe_info(file_path)
    with open(file_path, 'rb') as f:
        data = f.read()

    # Find all strings starting with C_ or S_
    pattern = re.compile(b'[CS]_[A-Za-z0-9_]{3,64}\x00')
    opcode_strings = []
    for m in pattern.finditer(data):
        name = m.group().decode('ascii', errors='ignore').strip('\x00')
        fo = m.start()
        rva = file_offset_to_rva(sections, fo)
        if rva:
            opcode_strings.append({'name': name, 'fo': fo, 'rva': rva, 'va': base + rva})

    print(f"Collected {len(opcode_strings)} opcode strings.")

    # Dictionary for fast lookup
    va_to_name = {}
    for o in opcode_strings:
        va_bytes = struct.pack('<Q', o['va'])
        va_to_name[va_bytes] = o['name']

    all_refs = []
    # Search the binary
    for i in range(0, len(data) - 8, 8): # Align to 8 bytes for data table search speed
        chunk = data[i:i+8]
        if chunk in va_to_name:
            all_refs.append({'fo': i, 'name': va_to_name[chunk], 'va': struct.unpack('<Q', chunk)[0]})

    print(f"Found {len(all_refs)} VA references.")
    
    # Sort and cluster
    all_refs.sort(key=lambda x: x['fo'])
    
    if all_refs:
        print("\nVA Reference Clusters:")
        current_cluster = [all_refs[0]]
        for r in all_refs[1:]:
            if r['fo'] - current_cluster[-1]['fo'] < 128:
                current_cluster.append(r)
            else:
                if len(current_cluster) >= 1: # Print any reference found
                    print(f"Cluster near 0x{current_cluster[0]['fo']:X} (Size {len(current_cluster)}):")
                    for cr in current_cluster[:10]:
                        print(f"  0x{cr['fo']:08X} -> {cr['name']}")
                    if len(current_cluster) > 10: print("  ...")
                current_cluster = [r]
        if current_cluster:
            print(f"Cluster near 0x{current_cluster[0]['fo']:X} (Size {len(current_cluster)}):")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    find_all_va_refs(path)
