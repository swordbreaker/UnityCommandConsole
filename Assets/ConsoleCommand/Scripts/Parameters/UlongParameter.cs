namespace CommandConsole.Parameters
{
    public class UlongParameter : NumericParameter<ulong>
    {
        public UlongParameter(string name, bool optional) : base(name, ulong.Parse, optional)
        {
        }

        protected override bool CanParse(string value)
        {
            ulong tmp;
            return ulong.TryParse(value, out tmp);
        }
    }
}
