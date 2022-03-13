using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpDdos
{
    internal class TargetProcessor
    {
        internal bool AddTargetIfOnlyIpAddressMentioned(string targetLine, List<Target> targets)
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

        internal bool AddTargetIfMethodAndPortFormatIncorrect(string portMethodLine, string ipAddress, List<Target> targets)
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

        internal void AddTargetWithPortAndMethod(string ipAddress, string portsAmdMethodsLine, List<Target> targets)
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
    }
}
