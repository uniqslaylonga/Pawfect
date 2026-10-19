# Run this ONCE on the employee PC (the one that will hold the shared database). Use 2-Setup-Server.bat.
# It installs SQL Server Express if needed, opens it to the LAN, creates the "pawfect" login and restores Pawfect.bak.
param(
    [string]$Instance      = 'SQLEXPRESS',
    [string]$LoginName     = 'pawfect',
    [string]$LoginPassword = 'Pawfect@123',   # must match ServerPassword in Database.cs; do not use a single quote (')
    [switch]$ForceRestore                      # overwrite an existing Pawfect database with Pawfect.bak
)
$ErrorActionPreference = 'Stop'

$SqlHost  = 'tcp:127.0.0.1,1433'   # the script sets the instance to port 1433 below, so no SQL Browser needed
$Service = if ($Instance -eq 'MSSQLSERVER') { 'MSSQLSERVER' } else { "MSSQL`$$Instance" }

function Step([string]$text) { Write-Host ''; Write-Host "== $text" -ForegroundColor Cyan }

function Sql([string]$query) {
    $c = New-Object System.Data.SqlClient.SqlConnection "Server=$SqlHost;Database=master;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=10;"
    try {
        $c.Open()
        $cmd = $c.CreateCommand(); $cmd.CommandTimeout = 0; $cmd.CommandText = $query
        return $cmd.ExecuteScalar()
    } finally { $c.Dispose() }
}

function SqlRows([string]$query) {
    $c = New-Object System.Data.SqlClient.SqlConnection "Server=$SqlHost;Database=master;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=10;"
    $rows = @()
    try {
        $c.Open()
        $cmd = $c.CreateCommand(); $cmd.CommandTimeout = 0; $cmd.CommandText = $query
        $r = $cmd.ExecuteReader()
        while ($r.Read()) { $rows += ,@($r['LogicalName'], $r['Type']) }
        $r.Close()
    } finally { $c.Dispose() }
    return $rows
}

function Get-InstanceId {
    $key = 'HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL'
    if (Test-Path $key) { return (Get-ItemProperty $key).$Instance }
    return $null
}

# 1 ---------------------------------------------------------------- SQL Server Express
Step 'Checking for SQL Server Express'
$id = Get-InstanceId
if (-not $id) {
    if (Get-Command winget -ErrorAction SilentlyContinue) {
        Write-Host 'Installing SQL Server Express with winget (this takes a few minutes)...'
        winget install --id Microsoft.SQLServer.2022.Express -e --accept-package-agreements --accept-source-agreements
        $id = Get-InstanceId
    }
    if (-not $id) {
        throw "SQL Server Express ($Instance) was not found. Install it from https://www.microsoft.com/sql-server/sql-server-downloads (Express > Basic), then run this setup again."
    }
}
Write-Host "Found instance $id"

# 2 ---------------------------------------------------------------- allow SQL logins + TCP port 1433
Step 'Turning on SQL logins and TCP/IP (port 1433)'
$base = "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\$id\MSSQLServer"
Set-ItemProperty $base -Name LoginMode -Value 2
$tcp = "$base\SuperSocketNetLib\Tcp"
Set-ItemProperty $tcp -Name Enabled -Value 1
Set-ItemProperty "$tcp\IPAll" -Name TcpPort -Value '1433'
Set-ItemProperty "$tcp\IPAll" -Name TcpDynamicPorts -Value ''
Restart-Service $Service -Force

# 3 ---------------------------------------------------------------- firewall
Step 'Opening the firewall for SQL Server'
Get-NetFirewallRule -DisplayName 'Pawfect SQL 1433' -ErrorAction SilentlyContinue | Remove-NetFirewallRule
New-NetFirewallRule -DisplayName 'Pawfect SQL 1433' -Direction Inbound -Protocol TCP -LocalPort 1433 -Action Allow -Profile Any | Out-Null

# 4 ---------------------------------------------------------------- wait until SQL answers
Step 'Waiting for SQL Server to start'
$ready = $false
for ($i = 0; $i -lt 30 -and -not $ready; $i++) {
    try { [void](Sql 'SELECT 1'); $ready = $true } catch { Start-Sleep -Seconds 2 }
}
if (-not $ready) { throw 'SQL Server did not start. Open Services and check "SQL Server (SQLEXPRESS)".' }

# 5 ---------------------------------------------------------------- database
Step 'Setting up the Pawfect database'
$bak = Join-Path $PSScriptRoot 'Pawfect.bak'
$exists = (Sql "SELECT COUNT(*) FROM sys.databases WHERE name = N'Pawfect'") -gt 0

if ($exists -and -not $ForceRestore) {
    Write-Host 'A Pawfect database already exists on this PC, so it was kept. (Run with -ForceRestore to replace it with Pawfect.bak.)'
}
elseif (Test-Path $bak) {
    $dataDir = [string](Sql "SELECT CAST(SERVERPROPERTY('InstanceDefaultDataPath') AS nvarchar(512))")
    $bakDir  = [string](Sql "SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS nvarchar(512))")
    Copy-Item $bak $bakDir -Force      # put it where the SQL service is allowed to read it
    $bakInSql = Join-Path $bakDir 'Pawfect.bak'

    $moves = @()
    foreach ($f in (SqlRows "RESTORE FILELISTONLY FROM DISK = N'$bakInSql'")) {
        $target = if ($f[1] -eq 'L') { Join-Path $dataDir 'Pawfect_log.ldf' } else { Join-Path $dataDir 'Pawfect.mdf' }
        $moves += "MOVE N'$($f[0])' TO N'$target'"
    }
    [void](Sql "RESTORE DATABASE [Pawfect] FROM DISK = N'$bakInSql' WITH REPLACE, $($moves -join ', ')")
    Write-Host 'Database restored from Pawfect.bak'
}
else {
    [void](Sql 'CREATE DATABASE [Pawfect]')
    Write-Warning 'Pawfect.bak was not found next to this script, so an EMPTY database was created. Run 1-Backup-Database.bat on the PC that has your data, copy Pawfect.bak here, and run this setup again with -ForceRestore.'
}

# 6 ---------------------------------------------------------------- login for the app
Step "Creating the '$LoginName' login"
[void](Sql @"
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = N'$LoginName')
    CREATE LOGIN [$LoginName] WITH PASSWORD = N'$LoginPassword', CHECK_POLICY = OFF;
ELSE
    ALTER LOGIN [$LoginName] WITH PASSWORD = N'$LoginPassword', CHECK_POLICY = OFF;
"@)
[void](Sql @"
USE [Pawfect];
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'$LoginName')
    CREATE USER [$LoginName] FOR LOGIN [$LoginName];
ELSE
    ALTER USER [$LoginName] WITH LOGIN = [$LoginName];
ALTER ROLE db_owner ADD MEMBER [$LoginName];
"@)

# 7 ---------------------------------------------------------------- test like the other PC will
Step 'Testing the connection the way the kiosk PC will'
$test = New-Object System.Data.SqlClient.SqlConnection "Server=127.0.0.1,1433;Database=Pawfect;User Id=$LoginName;Password=$LoginPassword;TrustServerCertificate=True;Connect Timeout=10;"
try { $test.Open(); Write-Host 'Connection over TCP port 1433 works.' -ForegroundColor Green } finally { $test.Dispose() }

# 8 ---------------------------------------------------------------- tell the user the address
Step 'This PC''s address on the network'
$ips = @(Get-NetIPAddress -AddressFamily IPv4 |
    Where-Object { $_.IPAddress -notlike '169.254.*' -and $_.IPAddress -ne '127.0.0.1' -and $_.PrefixOrigin -ne 'WellKnown' } |
    Select-Object -ExpandProperty IPAddress)
$ips | ForEach-Object { Write-Host "  $_" }
if ($ips.Count -gt 0) {
    Set-Content -Path (Join-Path $PSScriptRoot 'server.txt') -Value $ips[0] -Encoding ASCII
    Write-Host ''
    Write-Host "Wrote server.txt containing $($ips[0]). If this PC has several addresses, use the one on the same network as the kiosk PC." -ForegroundColor Yellow
}
Write-Host ''
Write-Host 'NEXT: copy server.txt next to Pawfect.exe on BOTH PCs (employee and kiosk), then start the app.' -ForegroundColor Green

