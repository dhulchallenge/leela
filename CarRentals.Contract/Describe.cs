using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Contract
{
    public static class Describe
    {
        public static string Duh(ISampleMessage m)
        {
            return m.ToString();
        }

        public static readonly IDictionary<Type, MethodInfo> Dict = ToDictionary();

        static Dictionary<Type, MethodInfo> ToDictionary()
        {
            var classes = typeof(Describe).Assembly.GetTypes()
                .Where(t => t.Name.StartsWith("Describ"));


            return classes.SelectMany(c => c.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                .Where(m => m.Name == "When")
                .Where(m => m.GetParameters().Length == 1))
                .ToDictionary(m => m.GetParameters().First().ParameterType, m => m);
        }


        public static string Object(object e)
        {
            MethodInfo info;
            var type = e.GetType();
            if (!Dict.TryGetValue(type, out info))
            {
                return e.ToString();
            }
            try
            {
                return (string)info.Invoke(null, new[] { e });
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }


        public static bool TryDescribe(object e, out string description)
        {
            MethodInfo info;
            description = null;

            var type = e.GetType();
            if (!Dict.TryGetValue(type, out info))
            {
                return false;
            }
            try
            {
                description = (string)info.Invoke(null, new[] { e });
                return true;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
