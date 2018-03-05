using System;

namespace CommandConsole.Parameters
{
    public class BoolParameter : VariableParameter
    {
 
        public BoolParameter(string name, bool optional = false) : base(name, optional)
        {
        }

        protected override object ParseValue(string value)
        {
            return bool.Parse(value);
        }

        protected override bool CanParse(string value)
        {
            bool tmp;
            return bool.TryParse(value, out tmp);
        }

        public override Type GetParamType()
        {
            return typeof(bool);
        }
    }
}
