
import struct
import os

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

def find_rip_relative(file_data, sections, target_rva):
    # Find .text section
    text_sec = None
    for s in sections:
        if s['name'] == '.text':
            text_sec = s
            break
    if not text_sec: return []

    results = []
    # Search in .text section for a 4-byte offset
    start = text_sec['rp']
    end = text_sec['rp'] + text_sec['rs'] - 4
    
    # We look for: TargetRVA = CurrentRVA + InstSize + Offset
    # Typical LEA is 7 bytes: 48 8D 05 [Offset]
    # So Offset = TargetRVA - CurrentRVA - 7
    
    for i in range(start, end):
        # Let's assume a few possible instruction sizes (7 is common for LEA)
        for inst_size in [7, 6, 5]: # LEA, MOV, etc.
            curr_rva = text_sec['va'] + (i - text_sec['rp'])
            next_inst_rva = curr_rva + inst_size
            expected_offset = target_rva - next_inst_rva
            
            # Check if bytes at i+inst_size-4 match expected_offset
            actual_offset = struct.unpack('<i', file_data[i + inst_size - 4 : i + inst_size])[0]
            if actual_offset == expected_offset:
                results.append({'fo': i, 'inst_size': inst_size, 'actual_offset': actual_offset})
    return results

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    base, sections = get_pe_info(path)
    with open(path, 'rb') as f:
        file_data = f.read()

    targets = [
        {'name': 'CastSpell', 'rva': 0x27ADE08},
        {'name': 'AuthChallenge', 'rva': 0x27EEE72},
    ]

    for t in targets:
        print(f"\n--- Searching RIP-relative references to {t['name']} (RVA: 0x{t['rva']:X}) ---")
        refs = find_rip_relative(file_data, sections, t['rva'])
        if refs:
            for r in refs:
                print(f"Found reference at file offset 0x{r['fo']:X} (guessed inst size: {r['inst_size']})")
                # Dump context
                d = file_data[r['fo']:r['fo']+16]
                print(f"  Code: {' '.join(f'{b:02X}' for b in d)}")
        else:
            print("No RIP-relative references found.")
