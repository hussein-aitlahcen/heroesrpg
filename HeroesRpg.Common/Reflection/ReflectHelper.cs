using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Common.Reflection
{
    public static class ReflectHelper
    {
        public static T FetchTypeAttribute<T>(Type t) where T : Attribute =>
            t.GetCustomAttribute<T>();

        public static Type[] FetchTypesInNamespace(string nameSpace) =>
            FetchTypesInNamespace(Assembly.GetEntryAssembly(), nameSpace);

        public static Type[] FetchTypesInNamespace(Assembly assembly, string nameSpace) =>
          assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();

        public static IEnumerable<T> FetchPublicStaticMembersAttribute<O, T>()
            where T : Attribute
            => FetchMembersAttribute<O, T>(null, BindingFlags.Public | BindingFlags.Static);

        public static IEnumerable<T> FetchMembersAttribute<O, T>(object instance = null, BindingFlags flags = BindingFlags.ExactBinding)
            where T : Attribute
        {
            return typeof(O)
            .GetFields(flags)
            .Where(p => p.GetCustomAttributes(typeof(T), true).Length > 0)
            .Select(p => (T)p.GetCustomAttribute(typeof(T), true));
        }

        public static IEnumerable<T> FetchFieldsValue<O, T>(object instance = null, BindingFlags flags = BindingFlags.Default)
        {
            var fields = typeof(O).GetFields(flags);
            return fields.Select(field => (T)field.GetValue(instance));
        }

        public static T FetchFieldValue<T>(Type t, string fieldName, BindingFlags flags = BindingFlags.Default, object instance = null) =>
            (T)t.GetField(fieldName, flags).GetValue(instance);
        

        public static T FetchPropertyValue<T>(Type t, string propertName, BindingFlags flags = BindingFlags.Default, object instance = null) =>
            (T)t.GetProperty(propertName, flags).GetValue(instance);
    }
}
