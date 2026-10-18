PAWFECT - TWO PCs, ONE DATABASE (kiosk + employee over LAN)

WHAT THIS DOES
The employee PC holds the database. The kiosk PC and the employee app both connect to it over the
network, so a booking made on the kiosk shows up on the employee screen within about 3 seconds.

STEPS
1. Replace the two files in Code\ in your project (Database.cs and MainForm.Data.cs), rebuild.
2. On the PC that has your data now:   run Setup\1-Backup-Database.bat   -> creates Setup\Pawfect.bak
3. Copy this whole Setup folder to the employee PC.
4. On the employee PC:                 run Setup\2-Setup-Server.bat (click Yes on the admin prompt)
   It installs SQL Server Express if needed, restores the data, opens the firewall, and writes server.txt.
5. Copy server.txt next to Pawfect.exe on BOTH PCs
   (the folder with the .exe, e.g. bin\Debug\net10.0-windows10.0.19041.0\).
6. Put both PCs on the same router / WiFi. Start the employee app on one, the kiosk on the other.

IF IT DOES NOT CONNECT
- The IP in server.txt must be the employee PC's address on the shared network (run ipconfig there).
  If the IP changes (new WiFi, restart), edit server.txt on both PCs - no rebuild needed.
- Both PCs must be on the same network. Guest / "client isolation" WiFi blocks PC-to-PC traffic; use a phone hotspot or a router.
- On the kiosk PC, test:  Test-NetConnection 192.168.1.10 -Port 1433   (should say TcpTestSucceeded : True)

GOING BACK TO A SINGLE PC
Delete server.txt. The app goes back to the local database automatically.

NOTE
The login is pawfect / Pawfect@123 (set in Database.cs and Setup-Server.ps1). Fine for a demo on a private network;
choose a stronger password if this is ever used for real.
