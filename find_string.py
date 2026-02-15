
import sys

def find_string(file_path, search_str):
    try:
        with open(file_path, 'rb') as f:
            data = f.read()
        
        offset = data.find(search_str.encode('ascii') + b'\0')
        if offset == -1:
            offset = data.find(search_str.encode('ascii'))
        
        if offset != -1:
            print(f"Found '{search_str}' at 0x{offset:X}")
            return offset
        else:
            print(f"'{search_str}' not found")
            return -1
    except Exception as e:
        print(f"Error: {e}")
        return -1

if __name__ == "__main__":
    path = r"D:\Classic\World of Warcraft Classic\_classic_era_\WoWClassic.exe"
    test_names = [
        "CMSG_",
        "SMSG_",
        "ClientServices",
        "WorldServices",
        "AuthServices",
        "AuthSession",
        "AuthChallenge"
    ]
    for name in test_names:
        find_string(path, name)
