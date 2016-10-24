using System;
using System;
using System.IO;
using System.Linq;
using Common.Ioc;

namespace Common.Helper
{
    public static class ResourceHelper
    {
        public static string GetTextFromFile(this string fileName)
        {
            string returnValue;

            using (var stream = AssemblyHelper.AssembliesOrdered("Common.", "XDDEasy.")
                .Select(x => x.GetManifestResourceStream(fileName))
                .FirstOrDefault(y => y != null))
            {
                if (stream == null)
                {
                    throw new ApplicationException(string.Format("Can not read resource named {0}.", fileName));
                }
                using (var reader = new StreamReader(stream))
                {
                    returnValue = reader.ReadToEnd();
                }
            }
            return returnValue;
        }
    }
}
