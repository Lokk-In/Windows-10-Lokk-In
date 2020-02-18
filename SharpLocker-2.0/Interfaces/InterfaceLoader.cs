using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Windows10LokkIn.Interfaces
{
    /// <summary>
    /// Loads interfaces from an assembly
    /// </summary>
    internal static class InterfaceLoader
    {
        /// <summary>
        /// Returns the first object which implements the specified interface.
        /// The interface needs to be contained in an dll-file or exe-file in the startup directory.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excludeExe"></param>
        /// <returns></returns>
        public static T Get<T>(bool excludeExe = false)
        {
            return GetAll<T>(excludeExe).FirstOrDefault();
        }

        /// <summary>
        /// Returns all object which implement the specified interface.
        /// The interface needs to be contained in an dll-file or exe-file in the startup directory. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excludeExe"></param>
        /// <returns></returns>
        public static List<T> GetAll<T>(bool excludeExe = false)
        {
            List<T> interfaces = new List<T>();

            GetDlls(excludeExe).ForEach(d => // loop through every file
            {
                try
                {
                    foreach (Type t in Assembly.LoadFrom(d).GetTypes()) // get types that are contained in the assembly of the file
                    {
                        if (typeof(T) == t) { continue; } // check if type is the specified interface
                        if (t.GetInterface(typeof(T).FullName) is null) { continue; } // check if type implements an interface

                        interfaces.Add((T)Activator.CreateInstance(t)); // create an instance of the interface implementing class
                    }

                }
                catch { /* Do nothing so we don't crash and can carry on*/ }

            });

            return interfaces;
        }

        /// <summary>
        /// Gets all files from the startup path that are dll (and exe files)
        /// </summary>
        /// <param name="excludeExe"></param>
        /// <returns></returns>
        private static List<string> GetDlls(bool excludeExe = false)
        {
            return Directory.GetFiles(Application.StartupPath).Where(x => x.EndsWith(".dll") || (!excludeExe && x.EndsWith(".exe"))).ToList();
        }
    }
}
