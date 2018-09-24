namespace CommandConsole.Parameters
{
    public class SbyteParameter : NumericParameter<sbyte>
    {
        public SbyteParameter(string name, bool optional) : base(name, sbyte.Parse, optional) {}

        protected override bool CanParse(string value)
        {
            sbyte tmp;
            return sbyte.TryParse(value, out tmp);
        }
    }
}
