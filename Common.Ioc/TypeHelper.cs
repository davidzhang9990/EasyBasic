using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Common.Ioc
{
    public static class TypeHelper
    {
        public static IEnumerable<Type> FindTypesThatExtend<TBaseType>()
        {
            return FindTypesThatExtend<TBaseType>(AssemblyHelper.Assemblies);
        }

        public static IEnumerable<Type> FindTypesThatExtend<TBaseType>(IEnumerable<Assembly> assemblies)
        {
            return (from assembly in assemblies
                    from type in GetTypesIgnoringError(assembly)
                    where typeof(TBaseType).IsAssignableFrom(type)
                    where type != typeof(TBaseType)
                    where type.IsClass
                    where !type.IsAbstract
                    select type);
        }

        [DebuggerNonUserCode]
        private static IEnumerable<Type> GetTypesIgnoringError(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                return Enumerable.Empty<Type>();
            }
        }
    }
}
