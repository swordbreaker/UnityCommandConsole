using System.Linq;
using Sprache;

namespace CommandConsole.ConsoleParser
{
    public static partial class CParser
    {
        public static Parser<string> SimpleCommandParser
        {
            get
            {
                return from leading in Parse.WhiteSpace.Many()
                    from first in Parse.Letter.Once()
                    from rest in Parse.LetterOrDigit.Many()
                    select new string(first.Concat(rest).ToArray());
            }
        }

        public static Parser<string> ClassParser
        {
            get
            {
                return from leading in Parse.WhiteSpace.Many()
                       from first in Parse.Letter.Once()
                       from c in Parse.LetterOrDigit.Many()
                       from point in Parse.Char('.')
                       from m in Parse.LetterOrDigit.Many()
                       select new string(first.Concat(c).ToArray()) + point + new string(m.ToArray());
            }
        }

        public static Parser<string> CommandNameParser
        {
            get { return ClassParser.Or(SimpleCommandParser); }
        }

        public static Parser<Variable> BracketsParser
        {
            get
            {
                return from leading in Parse.WhiteSpace.Many()
                    from first in Parse.Chars('(', '[', '{').Once()
                    from inner in Parse.AnyChar.Except(Parse.Chars(')', ']', '}').Once()).Many()
                    from last in Parse.Chars(')', ']', '}').Once()
                    select new Variable(new string(first.Concat(inner).Concat(last).ToArray()));
            }
        }

        public static Parser<Variable> CurlyBracketsParser
        {
            get
            {
                return from leading in Parse.WhiteSpace.Many()
                       from first in Parse.Chars('{').Once()
                       from inner in Parse.AnyChar.Except(Parse.Chars(')').Once()).Many()
                       from last in Parse.Chars('}').Once()
                       select new Variable(new string(first.Concat(inner).Concat(last).ToArray()));
            }
        }

        public static Parser<IValue> ArgumentParser
        {
            get { return QuestionMarkParser.Select(mark => (IValue)mark).Or(ValueParser); }
        }

        public static Parser<QuestionMark> QuestionMarkParser
        {
            get { return Parse.Char('?').Once().Select(c => new QuestionMark()); }
        }

        public static Parser<Command> CommandParser
        {
            get
            {
                return from cmd in CommandNameParser
                    from w in Parse.WhiteSpace.Many()
                    from par in ArgumentParser.Token().Many()
                    select new Command(cmd, par);
            }
        }
    }
}
