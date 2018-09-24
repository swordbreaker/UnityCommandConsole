using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using CommandConsole.Attributes;
using CommandConsole.Exceptions;
using CommandConsole.Parameters;

namespace CommandConsole
{
    public static class ReflectionHelper
    {
        public enum ImportType
        {
            Public, Marked
        }

        public static List<IConsoleCommand> GetCommands(Type t, object obj, ImportType importType, string className = null)
        {
            if (className == null) className = t.Name;
            //var className = t.Name;
            var commands = new List<IConsoleCommand>();

            foreach (var methodInfo in t.GetMethods())
            {
                var prefix = className + ".";
                var returnMessage = ConsoleCommand.DefaultReturnMessage;

                //Filter out the object Methods
                if (methodInfo.Name == "Equals" || methodInfo.Name == "GetHashCode" || methodInfo.Name == "GetType" || methodInfo.Name == "ToString") continue;

                var attributes = methodInfo.GetCustomAttributes(typeof(ConsoleCommandAttribute), true);
                //Check for ConsoleCommandAttribite
                if (importType == ImportType.Marked && attributes.Length == 0) continue;

                var commandName = "";
                if (attributes.Length > 0)
                {
                    var cca = (ConsoleCommandAttribute) attributes[0];
                    if (cca.Global)
                    {
                        prefix = "";
                    }

                    returnMessage = cca.ReturnMessage;

                    if (!string.IsNullOrEmpty(cca.Name))
                    {
                        commandName = prefix + cca.Name;
                    }
                    else
                    {
                        commandName = prefix + methodInfo.Name;
                    }
                }
                else
                {
                    commandName = prefix + methodInfo.Name;
                }

                var parameters = methodInfo.GetParameters().Select(ConstructParameter).ToList();

                var mInfo = methodInfo;
                commands.Add(
                    new ConsoleCommand(commandName, arguments => mInfo.Invoke(obj, arguments), parameters.ToArray(), returnMessage));
            }

            return commands;
        }

        private static IParameter ConstructParameter(ParameterInfo pInfo)
        {
            var attribute = pInfo.GetCustomAttributes(typeof(ConsoleParameterAttribute), true).FirstOrDefault();

            Type t = null;
            string name = null;
            ConsoleNumericParameterAttribute cnpa = null;
            if (attribute != null)
            {
                var cpa = (ConsoleParameterAttribute)attribute;
                t = cpa.ParameterType;
                name = cpa.ParameterName;
                if (cpa is ConsoleNumericParameterAttribute)
                {
                    cnpa = (ConsoleNumericParameterAttribute)cpa;
                }
            }

            if (t == null)
            {
                if (!Console.DefaultParameters.ContainsKey(pInfo.ParameterType))
                {
                    throw new ConsoleException(
                        string.Format("Cannot find a parameter for the type {0}", pInfo.ParameterType));
                }
                t = Console.DefaultParameters[pInfo.ParameterType];
            }

            var o = ConstructParameter(t, name ?? pInfo.Name, pInfo.IsOptional);

            if (cnpa != null)
            {
                var m = t.GetMethod("SetRange", new[] { pInfo.ParameterType, pInfo.ParameterType });
                var from = Convert.ChangeType(cnpa.From, pInfo.ParameterType);
                var till = Convert.ChangeType(cnpa.Till, pInfo.ParameterType);

                m.Invoke(o, new[] { from, till });
            }

            return (IParameter)o;
        }

        public static IParameter ConstructParameter(Type t, string name, bool optional)
        {
            object o = t.Assembly.CreateInstance
            (
                typeName: t.FullName,
                ignoreCase: true,
                bindingAttr: BindingFlags.ExactBinding,
                binder: null,
                args: new object[]
                {
                    name,
                    optional
                },
                culture: CultureInfo.CurrentCulture,
                activationAttributes: null
            );

            return (IParameter)o;
        }
    }
}
