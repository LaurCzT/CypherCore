$password = "Fondat,.12"
$mysqlIdx = "C:\Program Files\MySQL\MySQL Server 8.4\bin\mysql.exe"
$argsBase = "-u", "root", "-p$password"

function Execute-Sql {
    param(
        [string]$DbName,
        [string]$SqlContent,
        [string]$File
    )
    
    if ($File) {
        # Using cmd /c type to handle encoding issues if any, or just powershell pipe
        # Get-Content might have encoding issues with some SQL dumps if they are UTF8 w/ BOM or similar.
        # But usually standard ASCII/UTF8 works.
        Write-Host "Importing $File into $DbName..."
        Get-Content $File | & $mysqlIdx $argsBase $DbName
    } elseif ($SqlContent) {
        Write-Host "Executing SQL on $DbName..."
        $SqlContent | & $mysqlIdx $argsBase $DbName
    }
}

# Create databases
$createDbSql = @"
CREATE DATABASE IF NOT EXISTS cypher114_logon DEFAULT CHARACTER SET utf8mb4;
CREATE DATABASE IF NOT EXISTS cypher114_chars DEFAULT CHARACTER SET utf8mb4;
CREATE DATABASE IF NOT EXISTS cypher114_world DEFAULT CHARACTER SET utf8mb4;
CREATE DATABASE IF NOT EXISTS cypher114_hotfix DEFAULT CHARACTER SET utf8mb4;
"@

$createDbSql | & $mysqlIdx $argsBase

# Import structure
Execute-Sql -DbName "cypher114_logon" -File "d:\Classic\CypherCoreClassic\sql\base\auth_database.sql"
Execute-Sql -DbName "cypher114_chars" -File "d:\Classic\CypherCoreClassic\sql\base\characters_database.sql"
Execute-Sql -DbName "cypher114_hotfix" -File "d:\Classic\CypherCoreClassic\sql\base\dev\hotfixes_database.sql"
Execute-Sql -DbName "cypher114_world" -File "d:\Classic\CypherCoreClassic\sql\base\dev\world_database.sql"

# Configure Realmlist and Build Info
# Using '50808' for 1.14.4
$configSql = @"
USE cypher114_logon;
DELETE FROM realmlist;
INSERT INTO realmlist (id, name, address, localAddress, localSubnetMask, port, icon, flag, timezone, allowedSecurityLevel, population, gamebuild, Region, Battlegroup) 
VALUES (1, 'CypherCore Classic', '127.0.0.1', '127.0.0.1', '255.255.255.0', 8085, 0, 0, 1, 0, 0, 50808, 1, 1);

INSERT IGNORE INTO build_info (build, majorVersion, minorVersion, bugfixVersion, hotfixVersion) VALUES (50808, 1, 14, 4, '0');
"@

$configSql | & $mysqlIdx $argsBase

Write-Host "Database setup complete."
