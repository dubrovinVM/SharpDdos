using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDdos
{
    internal record class TargetService
    {
        private string DbPath { get; set; }
        private string TargetTaskPath { get; set; }

        public TargetService(string dbPath, string targetTaskPath)
        {
            DbPath = dbPath;
            TargetTaskPath = targetTaskPath;
        }

        internal List<Target> GetUniqueTargets()
        {
            var linesFromHistory = GetLinesFromFile(DbPath);
            var targetsFromHistory = GetTargetsFromLines(linesFromHistory);

            var linesFromNewFile = GetLinesFromFile(TargetTaskPath);
            var targetsFromNewFile = GetTargetsFromLines(linesFromNewFile);

            if (targetsFromHistory.Count == 0)
            {
                return targetsFromNewFile;
            }

            var newTargets = targetsFromNewFile.RemoveDuplicates(targetsFromHistory);

            return newTargets;
        }

        internal List<Target> GetTargetsFromLines(List<string> lines)
        {
            List<Target> targets = new();

            foreach (var line in lines)
            {
                var targetsFromLine = GetTargetsFromLine(line);

                if (targetsFromLine.Count == 0)
                {
                    continue;
                }

                targets.AddRange(targetsFromLine);
            }

            return targets;            
        }

        internal void WriteTargetToDB(List<Target> newTargets)
        {
            using var dbWriter = new StreamWriter(DbPath, true);

            foreach (var target in newTargets)
            {
                dbWriter.WriteLine($"{target.IpAddress}:{target.Method}/{target.Port}");
            }
        }

        internal List<string> GetLinesFromFile(string filePath)
        {
            using var fileReader = new StreamReader(File.Open(filePath, FileMode.OpenOrCreate));
            var lines = new List<string>();
            while (!fileReader.EndOfStream)
            {
                string line = fileReader.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (!lines.Contains(line))
                {
                    lines.Add(line);
                }
            }
            return lines;
        }    
        
        internal List<Target> GetTargetsFromLine(string targetLine)
        {
            var targets = new List<Target>();

            if (AddTargetIfOnlyIpAddressMentioned(targetLine, targets))
            {
                return targets;
            }          

            string[] words = targetLine.Split(':');

            string ipAddress = words[0].Trim();
            string portsAmdMethodsLine = words[1];

            if (AddTargetIfMethodAndPortFormatIncorrect(targetLine, ipAddress, targets))
            {
                return targets;
            }

            AddTargetWithPortAndMethod(ipAddress, portsAmdMethodsLine, targets);           

            return targets;

        }
        private void AddTargetWithPortAndMethod(string ipAddress, string portsAmdMethodsLine, List<Target> targets)
        {
            string[] portMethodArray = portsAmdMethodsLine.Trim().Split(' ');

            foreach (var portMethod in portMethodArray)
            {
                string[] @params = portMethod.Split('/');

                var parseResult = Enum.TryParse(@params[0].ToLower(), out Method method);
                method = parseResult ? method : Method.udp;
               
                if (@params.Length != 2 || !int.TryParse(@params[1], out _))
                {
                    Console.WriteLine($"Incorrect part of the line {portMethod} for {ipAddress}");
                    continue;
                }

                targets.Add(new Target()
                {
                    IpAddress = ipAddress,
                    Method = method,
                    Port = @params[1]
                });
            }           
        }

        private bool AddTargetIfOnlyIpAddressMentioned(string targetLine, List<Target> targets)
        {
            if (!targetLine.Contains(':'))
            {
                targets.Add(new Target()
                {
                    IpAddress = targetLine.Trim(),
                    Method = Method.udp,
                    Port =  "53"
                });
                return true;
            }
            return false;
        }

        private bool AddTargetIfMethodAndPortFormatIncorrect(string portMethodLine, string ipAddress, List<Target> targets)
        {
            if (!portMethodLine.Contains(' ') && !portMethodLine.Contains('/'))
            {
                targets.Add(new Target()
                {
                    IpAddress = ipAddress,
                    Method = Method.udp,
                    Port = "53"
                });
                return true;
            }
            return false;           
        }
    }
}
