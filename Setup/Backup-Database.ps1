# Run this on the PC that has your Pawfect data right now (the one you develop on).
# It saves the database to Pawfect.bak next to this script.
$ErrorActionPreference = 'Stop'
$out = Join-Path $PSScriptRoot 'Pawfect.bak'
if (Test-Path $out) { Remove-Item $out -Force }

$conn = New-Object System.Data.SqlClient.SqlConnection 'Server=(localdb)\MSSQLLocalDB;Database=master;Integrated Security=True;TrustServerCertificate=True;'
try {
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandTimeout = 0
    $cmd.CommandText = "BACKUP DATABASE [Pawfect] TO DISK = N'$out' WITH INIT, FORMAT, COPY_ONLY"
    [void]$cmd.ExecuteNonQuery()
} finally { $conn.Dispose() }

Write-Host ''
Write-Host "Done. Backup saved to: $out" -ForegroundColor Green
Write-Host 'Copy this whole Setup folder (with Pawfect.bak inside) to the employee PC, then run 2-Setup-Server.bat there.'
