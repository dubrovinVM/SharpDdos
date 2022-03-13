namespace SharpDdos
{
    internal record struct Constants
    {
        internal const string POWER_SHELL_PATH = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
        internal const string DB_PATH = @"target-history.txt";
        internal const string TARGET_TASK_PATH= @"IP.txt";
        internal const int DEFAULT_PORT= 53;
        internal const Method DEFAULT_METHOD = Method.udp;
    }
}
