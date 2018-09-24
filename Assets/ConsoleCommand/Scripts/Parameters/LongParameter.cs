namespace CommandConsole.Parameters
{
    public class LongParameter : NumericParameter<long>
    {
        public LongParameter(string name, bool optional) : base(name, long.Parse, optional)
        {
        }

        protected override bool CanParse(string value)
        {
            long tmp;
            return long.TryParse(value, out tmp);
        }
    }
}
