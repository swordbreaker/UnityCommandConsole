namespace CommandConsole.Parameters
{
    public class UintParameter : NumericParameter<uint>
    {
        public UintParameter(string name, bool optional) : base(name, uint.Parse, optional)
        {
        }

        protected override bool CanParse(string value)
        {
            uint tmp;
            return uint.TryParse(value, out tmp);
        }
    }
}
