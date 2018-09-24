using System.Linq;
using Sprache;

namespace CommandConsole.ConsoleParser
{
    static partial class CParser
    {
        public static Parser<Variable> NumberParser
        {
            get
            {
                return from regex in Parse.Regex(@"-?\s*[0-9]*\.*[0-9]*\s*")
                    select new Variable(regex);
            }
        }

        public static Parser<Variable> StringParser
        {
            get { return QuoatSeperated.Or(WhitespaceSeperated).Select(s => new Variable(s)); }
        }

        public static Parser<VObject> ObjectParser
        {
            get
            {
                return from first in Parse.Char('(')
                    from inner in ValueParser.Token().Many()
                    from last in Parse.Char(')')
                    select new VObject(inner.ToList());
            }
        }

        public static Parser<string> QuoatSeperated
        {
            get
            {
                return from q in Parse.Char('"')
                    from inner in Parse.AnyChar.Until(Parse.Char('"'))
                    select new string(inner.ToArray());
            }
        }

        public static Parser<string> WhitespaceSeperated
        {
            get { return Parse.LetterOrDigit.Many().Token().Select(c => new string(c.ToArray())); }
        }

        public static Parser<Variable> VariableParser
        {
            get { return NumberParser.Token().Or(StringParser); }
        }

        public static Parser<VList> ListParser
        {
            get
            {
                return from first in Parse.Char('{')
                    from inner in ValueParser.Token().Until(Parse.Char('}'))
                    select new VList(inner.ToList());
            }
        }

        public static Parser<IValue> ValueParser
        {
            get
            {
                return ListParser.Select(list => (IValue)list)
                    .Or(ObjectParser.Select(o => (IValue)o))
                    .Or(VariableParser.Select(variable => (IValue)variable));
            }
        }
    }
}