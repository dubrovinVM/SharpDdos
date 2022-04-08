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
                    Method = Constants.DEFAULT_METHOD,
                    Port =  Constants.DEFAULT_PORT
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
                    Method = Constants.DEFAULT_METHOD,
                    Port = Constants.DEFAULT_PORT
                });
                return true;
            }
            return false;
        }

        internal void AddTargetWithPortAndMethod(string ipAddress, string portsAmdMethodsLine, List<Target> targets)
        {
            string[] portMethodArray = portsAmdMethodsLine.Trim().Replace(",","").Split(' ');

            Method methodGlobal = Constants.DEFAULT_METHOD;
            bool methodParseResultGlobal = default;
            List<int> ports = new();
            bool methodParseResult;

            foreach (var portOrMethod in portMethodArray)
            {
                if (!portOrMethod.Contains('/'))
                {
                    Method currentMethod;

                    _ = Enum.TryParse(portOrMethod.ToLower(), out currentMethod);
                    var isDefined = Enum.IsDefined(typeof(Method), currentMethod);

                    var portParseResult = int.TryParse(portOrMethod.ToLower(), out int port);

                    if (!portParseResult && isDefined)
                    {
                        if (ports.Count == 0)
                        {
                            methodGlobal = currentMethod;
                            methodParseResultGlobal = true;
                            continue;
                        }

                        foreach (var prt in ports)
                        {
                            targets.Add(new Target()
                            {
                                IpAddress = ipAddress,
                                Method = methodParseResultGlobal ? methodGlobal : currentMethod,
                                Port = prt
                            });
                        }
                        methodGlobal = currentMethod;
                        methodParseResultGlobal = true;
                        ports.Clear();
                    }
                    else if(portParseResult && !isDefined)
                    {
                        ports.Add(port);
                        continue;
                    }                   
                }
                else
                {
                    if (methodParseResultGlobal && ports.Count > 0)
                    {
                        foreach (var prt in ports)
                        {
                            targets.Add(new Target()
                            {
                                IpAddress = ipAddress,
                                Method = methodGlobal,
                                Port = prt
                            });
                        }
                        methodParseResultGlobal = false;
                        ports.Clear();
                    }

                    int port;
                    Method currentMethod;

                    string[] @params = portOrMethod.Split('/');

                    var param1 = @params[0].ToLower();
                    var param2 = @params[1].ToLower();

                    _ = Enum.TryParse(param1.ToLower(), out currentMethod);
                    var isDefined = Enum.IsDefined(typeof(Method), currentMethod);

                    if(!isDefined)
                    {
                        Enum.TryParse(param2.ToLower(), out currentMethod);
                        isDefined = Enum.IsDefined(typeof(Method), currentMethod);
                    }

                    methodGlobal = isDefined ? currentMethod : methodGlobal;

                    if(!int.TryParse(param1, out port))
                    {
                        _ = int.TryParse(param2, out port);
                    }

                    methodGlobal = !isDefined && !methodParseResultGlobal ? Constants.DEFAULT_METHOD : methodGlobal;
                    port = port == 0 ? Constants.DEFAULT_PORT : port;
                    ports.Add(port);
                    methodParseResultGlobal = true;

                    foreach (var prt in ports)
                    {
                        targets.Add(new Target()
                        {
                            IpAddress = ipAddress,
                            Method = methodGlobal,
                            Port = prt
                        });
                    }
                    ports.Clear();
                }                
            }

            foreach (var prt in ports)
            {
                targets.Add(new Target()
                {
                    IpAddress = ipAddress,
                    Method = methodGlobal,
                    Port = prt
                });
            }

            ports.Clear();
        }
    }
}
