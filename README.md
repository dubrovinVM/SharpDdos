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