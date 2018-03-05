using System.Collections.Generic;
using System.Linq;

namespace CommandConsole
{
    public class AutoCompleteManger
    {
        private readonly Dictionary<string, List<string>> _autoCompleteDictronary = new Dictionary<string,List<string>>();

        /// <summary>
        /// Gets the available commands saved in the AutoCompleManager for these Pattern.
        /// </summary>
        /// <param name="pattern">The Pattern.</param>
        /// <returns></returns>
        public IEnumerable<string> GetAvailableCommands(string pattern)
        {
            var splitt = pattern.Split('.');
            if (_autoCompleteDictronary.ContainsKey(splitt[0]))
            {
                return _autoCompleteDictronary[pattern];
            }
            if (splitt.Length > 1)
            {
                var cmd = splitt[0] + ".";
                if (_autoCompleteDictronary.ContainsKey(cmd))
                {
                    return Filter(_autoCompleteDictronary[cmd], splitt[1]).Select(s1 => cmd + s1);
                }
                return new List<string>();
            }
            return Filter(_autoCompleteDictronary.Keys, splitt[0]);
        }

        /// <summary>
        /// Adds the class command to the Manager.
        /// </summary>
        /// <param name="commandName">The class name.</param>
        /// <param name="l">A list of all methods of a class. When the command is global the this should be null or an empty list</param>
        private void Add(string commandName, IEnumerable<string> l)
        {
            if(l == null) l = new List<string>();
            _autoCompleteDictronary[commandName] = l.ToList();
        }

        /// <summary>
        /// Adds the class command to the Manager.
        /// </summary>
        /// <param name="key">The class name with the . at the end. For example TestClass.</param>
        /// <param name="value">The method</param>
        public void Add(string key, string value)
        {
            if (_autoCompleteDictronary.ContainsKey(key))
            {
                _autoCompleteDictronary[key].Add(value);
            }
            else
            {
                _autoCompleteDictronary.Add(key, new List<string>(new[]{value}));
            }
        }

        /// <summary>
        /// Adds the specified command to the manager.
        /// </summary>
        /// <param name="commandName">Name of the command.</param>
        public void Add(string commandName)
        {
            Add(commandName, new List<string>());
        }

        /// <summary>
        /// Removes the specified command when it is a class all Methods will be also removed.
        /// </summary>
        /// <param name="commandName">Name of the command.</param>
        public void Remove(string commandName)
        {
            _autoCompleteDictronary.Remove(commandName);
        }

        private IEnumerable<string> Filter(IEnumerable<string> list, string value)
        {
            return list.Where(s => s.Length >= value.Length && s.Substring(0, value.Length) == value);
        }
    }
}
