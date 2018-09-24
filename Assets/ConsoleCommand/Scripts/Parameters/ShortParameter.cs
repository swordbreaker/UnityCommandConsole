namespace CommandConsole.Parameters
{
    public class ShortParameter : NumericParameter<short>
    {
        public ShortParameter(string name, bool optional) : base(name, short.Parse, optional)
        {
        }

        protected override bool CanParse(string value)
        {
            short tmp;
            return short.TryParse(value, out tmp);
        }
    }
}
