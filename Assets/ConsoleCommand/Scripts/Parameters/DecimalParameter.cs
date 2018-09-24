namespace CommandConsole.Parameters
{
    public class DecimalParameter : NumericParameter<decimal>
    {
        public DecimalParameter(string name, bool optional) : base(name, decimal.Parse, optional)
        {
        }

        protected override bool CanParse(string value)
        {
            decimal tmp;
            return decimal.TryParse(value, out tmp);
        }
    }
}
