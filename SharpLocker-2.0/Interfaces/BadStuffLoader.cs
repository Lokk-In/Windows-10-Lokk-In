using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SharpLocker_2._0.Interfaces
{
    public class BadStuffLoader
    {
        public IDoBadStuff Get()
        {
            IDoBadStuff test = null;

            GetDlls().ForEach(d =>
            {
                try
                {
                    foreach (Type t in Assembly.LoadFrom(d).GetTypes())
                    {
                        if (typeof(IDoBadStuff) == t) { continue; }
                        if (t.GetInterface(typeof(IDoBadStuff).FullName) is null) { continue; }

                        test = (IDoBadStuff)Activator.CreateInstance(t);
                    }

                }
                catch { }

            });

            return test;
        }

        private List<string> GetDlls()
        {
            return Directory.GetFiles(Application.StartupPath).Where(x => x.EndsWith(".dll")).ToList();
        }
    }
}
