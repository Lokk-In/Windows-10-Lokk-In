using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Windows10LokkIn.Interfaces
{
    internal static class InterfaceLoader
    {
        public static T Get<T>(bool excludeExe = false)
        {
            return GetAll<T>(excludeExe).FirstOrDefault();
        }

        public static List<T> GetAll<T>(bool excludeExe = false)
        {
            List<T> interfaces = new List<T>();

            GetDlls(excludeExe).ForEach(d =>
            {
                try
                {
                    foreach (Type t in Assembly.LoadFrom(d).GetTypes())
                    {
                        if (typeof(T) == t) { continue; }
                        if (t.GetInterface(typeof(T).FullName) is null) { continue; }

                        interfaces.Add((T)Activator.CreateInstance(t));
                    }

                }
                catch { }

            });

            return interfaces;
        }

        private static List<string> GetDlls(bool excludeExe = false)
        {
            return Directory.GetFiles(Application.StartupPath).Where(x => x.EndsWith(".dll") || (!excludeExe && x.EndsWith(".exe"))).ToList();
        }
    }
}
