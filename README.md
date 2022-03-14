The program listens to the "IP.txt" file and processes it while initial start.
In case of any changes it starts processing the file:

read the file line by line
get targets from each line
check if there are duplicates in history-targets.txt
in case of new targets
1. parse it (get ip-address, port, method)
2. start new powershell window
3. in powershell windows start the DRypper (DDos tool, based on phyton) with previousely parsed parameters
4. insert targets to history-targets.txt
continue listening for changes in Ip.txt.
Format of targets in "IP.txt": [ip-address]:method/port

Example: 188.128.56.24:http/80 udp/100 tcp/64

Also there is an opportunity to skip method and port in IP.txt, so the program will automatically take udp/53. For example:
For such a line 188.0.0.0 the program will start a DRypper for 188.0.0.0 via 53 port using UDP.

**How to run**
1. Download from https://github.com/dubrovinVM/SharpDdos/blob/develop/ReleaseForDownload/SharpDdosRelease.zip
2. Unzip an archive.
3. Start SharpDdos.exe
4. Fill a file `IP.txt` with targets in any format:
    217.10.36.226 :80 443/HTTP 16 100/Tcp 
    217.10.36.227 :HTTP/80 /443  
    46.4.106.112:80/HTTP , 443/HTTP
    217.10.36.229 :HTTP 16 100/Tcp 
 5. Wait for a while, a progrram will process changes and will run DRypper for each target.


