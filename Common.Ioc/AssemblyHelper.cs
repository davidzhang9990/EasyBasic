using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;

namespace Common.Ioc
{
    public static class AssemblyHelper
    {
        private static List<Assembly> _assemblies;

        public static IEnumerable<Assembly> AssembliesOrdered(string firstStartsWith, string last)
        {
            var first = new List<Assembly>();
            var second = new List<Assembly>();
            var list3 = new List<Assembly>();
            foreach (Assembly assembly in Assemblies)
            {
                if (assembly.FullName.Equals(last))
                {
                    second.Add(assembly);
                }
                else if (assembly.FullName.StartsWith(firstStartsWith))
                {
                    first.Add(assembly);
                }
                else
                {
                    list3.Add(assembly);
                }
            }
            return first.Concat<Assembly>(list3).Concat<Assembly>(second);
        }

        public static string GetAssembleFilePath()
        {
            var path = HostingEnvironment.IsHosted ? HttpRuntime.BinDirectory : Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            return path;
        }

        private static IEnumerable<string> GetAssemblyFileNames()
        {
            string path = GetAssembleFilePath();
            Contract.Assert(path != null, "directory != null");
            return Directory.GetFiles(path, "*.dll").Concat<string>(Directory.GetFiles(path, "*.exe"));
        }

        [DebuggerNonUserCode]
        private static void LoadAssemblyIgnoringErrors(string assemblyFileName)
        {
            try
            {
                _assemblies.Add(Assembly.LoadFrom(assemblyFileName));
                Trace.WriteLine("Loaded assembly " + assemblyFileName);
            }
            catch (Win32Exception)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (FileNotFoundException)
            {
            }
            catch (PathTooLongException)
            {
            }
            catch (BadImageFormatException)
            {
            }
            catch (SecurityException)
            {
            }
        }

        private static IEnumerable<Assembly> AppCodeAssemblies
        {
            get
            {
                if (HostingEnvironment.IsHosted && (BuildManager.CodeAssemblies != null))
                {
                    return BuildManager.CodeAssemblies.OfType<Assembly>();
                }
                return Enumerable.Empty<Assembly>();
            }
        }

        public static IEnumerable<Assembly> Assemblies
        {
            get
            {
                if (_assemblies == null)
                {
                    _assemblies = new List<Assembly>();
                    foreach (string str in GetAssemblyFileNames())
                    {
                        LoadAssemblyIgnoringErrors(str);
                    }
                }
                return _assemblies.Concat<Assembly>(AppCodeAssemblies);
            }
        }
    }
}
