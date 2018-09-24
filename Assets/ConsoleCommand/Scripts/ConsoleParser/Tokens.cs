using System.Collections.Generic;

namespace CommandConsole.ConsoleParser
{
    public class Command
    {
        public string Name;
        public IEnumerable<IValue> Parameters;

        public Command(string name, IEnumerable<IValue> parameters)
        {
            Name = name;
            Parameters = parameters;
        }
    }

    public class Variable : IValue
    {
        public string Value;

        public Variable(string value)
        {
            Value = value;
        }
    }

    public class VList : IValue
    {
        public List<IValue> Variables;

        public VList(List<IValue> variables)
        {
            Variables = variables;
        }
    }

    public class VObject : IValue
    {
        public List<IValue> Variables;

        public VObject(List<IValue> variables)
        {
            Variables = variables;
        }
    }

    public class QuestionMark : IValue { }

    public interface IValue { }
}
