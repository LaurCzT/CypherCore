
$filePath = "D:\Classic\World of Warcraft\_classic_era_\WowClassic.exe"
if (-not (Test-Path $filePath)) {
    Write-Host "File not found: $filePath"
    exit
}

$fileBytes = [System.IO.File]::ReadAllBytes($filePath)
Write-Host "File size: $($fileBytes.Length) bytes"

# AuthCheckSeed: C5 C6 98 95 76 3F 1D CD B6 A1 37 28 B3 12 FF 8A
$pattern = [byte[]](0xC5, 0xC6, 0x98, 0x95, 0x76, 0x3F, 0x1D, 0xCD, 0xB6, 0xA1, 0x37, 0x28, 0xB3, 0x12, 0xFF, 0x8A)

function FindPattern($bytes, $p) {
    for ($i = 0; $i -le ($bytes.Length - $p.Length); $i++) {
        $match = $true
        for ($j = 0; $j -lt $p.Length; $j++) {
            if ($bytes[$i + $j] -ne $p[$j]) {
                $match = $false
                break
            }
        }
        if ($match) { return $i }
    }
    return -1
}

$offset = FindPattern $fileBytes $pattern
if ($offset -ne -1) {
    Write-Host "Found AuthCheckSeed at offset: $offset (0x$($offset.ToString('X')))"
    
    # Extract nearby 16-byte blocks (searching for potential win64AuthSeed)
    # Usually they are grouped in a structure or nearby
    Write-Host "Nearby 16-byte blocks (Hex):"
    for ($i = -64; $i -le 64; $i += 16) {
        $currentOffset = $offset + $i
        if ($currentOffset -ge 0 -and ($currentOffset + 16) -le $fileBytes.Length) {
            $hex = ""
            for ($j = 0; $j -lt 16; $j++) {
                $hex += $fileBytes[$currentOffset + $j].ToString("X2")
            }
            $label = if ($i -eq 0) { "Pattern (AuthCheckSeed)" } else { "Offset $i" }
            Write-Host "$label [0x$($currentOffset.ToString('X'))]: $hex"
        }
    }
}
else {
    Write-Host "AuthCheckSeed pattern not found."
}
