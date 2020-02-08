using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SharpLocker_2._0.Interfaces
{
    public static class InterfaceLoader
    {
        public static T Get<T>()
        {
            T @interface = default(T);

            GetDlls().ForEach(d =>
            {
                try
                {
                    foreach (Type t in Assembly.LoadFrom(d).GetTypes())
                    {
                        if (typeof(T) == t) { continue; }
                        if (t.GetInterface(typeof(T).FullName) is null) { continue; }

                        @interface = (T)Activator.CreateInstance(t);
                    }

                }
                catch { }

            });

            return @interface;
        }

        private static List<string> GetDlls()
        {
            return Directory.GetFiles(Application.StartupPath).Where(x => x.EndsWith(".dll")).ToList();
        }
    }
}
