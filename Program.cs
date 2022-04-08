var path = AppDomain.CurrentDomain.BaseDirectory;

FileChangedHandler.OnCreated(sender: default, 
    new FileSystemEventArgs(WatcherChangeTypes.Created, string.Empty, string.Empty)
   );

Console.WriteLine($"Watching Current path: { path}");

using var watcher = new FileSystemWatcher(path);

watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;
watcher.Filter = "IP.txt";
watcher.Changed += FileChangedHandler.OnChanged;
watcher.Created += FileChangedHandler.OnCreated;
watcher.Error += FileChangedHandler.OnError;

watcher.EnableRaisingEvents = true;

Console.WriteLine("Press enter to exit.");
Console.ReadLine();








