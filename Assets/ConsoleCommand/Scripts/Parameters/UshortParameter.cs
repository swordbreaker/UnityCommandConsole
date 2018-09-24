namespace CommandConsole.Parameters
{
    public class UshortParameter : NumericParameter<ushort>
    {
        public UshortParameter(string name, bool optional) : base(name, ushort.Parse, optional)
        {
        }

        protected override bool CanParse(string value)
        {
            ushort tmp;
            return ushort.TryParse(value, out tmp);
        }
    }
}