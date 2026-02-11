
$filePath = "D:\Classic\World of Warcraft\_classic_era_\WowClassic.exe"
if (-not (Test-Path $filePath)) {
    Write-Host "File not found: $filePath"
    exit
}

$fileBytes = [System.IO.File]::ReadAllBytes($filePath)

# 40618 decimal = 0x9E92
$pattern = [byte[]](0x92, 0x9E, 0x00, 0x00)

for ($i = 0; $i -le ($fileBytes.Length - $pattern.Length); $i++) {
    $match = $true
    for ($j = 0; $j -lt $pattern.Length; $j++) {
        if ($fileBytes[$i + $j] -ne $pattern[$j]) {
            $match = $false
            break
        }
    }
    if ($match) {
        Write-Host "Found build number 40618 at offset: 0x$($i.ToString('X'))"
        # Look around for 16-byte blocks
        for ($k = -128; $k -le 128; $k += 16) {
            $currentOffset = $i + $k
            if ($currentOffset -ge 0 -and ($currentOffset + 16) -le $fileBytes.Length) {
                $hex = ""
                for ($h = 0; $h -lt 16; $h++) {
                    $hex += $fileBytes[$currentOffset + $h].ToString("X2")
                }
                Write-Host "  Offset $k [0x$($currentOffset.ToString('X'))]: $hex"
            }
        }
    }
}
