using System;

namespace CommandConsole.Parameters
{
    public class StringParameter : VariableParameter
    {
        public StringParameter(string name, bool optional = false) : base(name, optional)
        {
            
        }

        protected override object ParseValue(string s)
        {
            return s;
        }

        protected override bool CanParse(string value)
        {
            return true;
        }

        public override Type GetParamType()
        {
            return typeof(string);
        }
    }
}
