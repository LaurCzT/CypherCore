
import struct
import os

def print_pe_sections(file_path):
    with open(file_path, 'rb') as f:
        f.seek(0x3C)
        pe_offset = struct.unpack('<I', f.read(4))[0]
        f.seek(pe_offset)
        sig = f.read(4)
        if sig != b'PE\0\0':
             print("Not a PE file")
             return
             
        # File Header (20 bytes)
        f.seek(pe_offset + 4)
        machine, num_sections, timedate, sym_ptr, num_sym, size_of_opt, characteristics = struct.unpack('<HHIIIHH', f.read(20))
        
        # Optional Header
        optional_header_offset = f.tell()
        f.seek(optional_header_offset)
        magic = struct.unpack('<H', f.read(2))[0]
        
        if magic == 0x20b: # PE32+
            f.seek(optional_header_offset + 24)
            image_base = struct.unpack('<Q', f.read(8))[0]
        else:
            f.seek(optional_header_offset + 28)
            image_base = struct.unpack('<I', f.read(4))[0]
            
        # Section Headers follow Optional Header
        f.seek(optional_header_offset + size_of_opt)
        
        print(f"ImageBase: 0x{image_base:X}")
        print(f"{'Name':<8} {'VSize':<8} {'VAddr':<8} {'RSize':<8} {'RPtr':<8} {'Char':<8}")
        for _ in range(num_sections):
            name_bytes = f.read(8)
            name = name_bytes.split(b'\0')[0].decode('ascii', errors='ignore')
            # Section Header (remaining 32 bytes)
            v_size, v_addr, r_size, r_ptr, r_reloc, r_line, n_reloc, n_line, characteristics = struct.unpack('<IIIIIIHHI', f.read(32))
            print(f"{name:<8} {v_size:08X} {v_addr:08X} {r_size:08X} {r_ptr:08X} {characteristics:08X}")

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    print_pe_sections(path)
