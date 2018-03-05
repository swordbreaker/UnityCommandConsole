namespace CommandConsole.Parameters
{
    public class ByteParamter : NumericParameter<byte>
    {
        public ByteParamter(string name, bool optional) : base(name, byte.Parse, optional) {}

        protected override bool CanParse(string value)
        {
            byte tmp;
            return byte.TryParse(value, out tmp);
        }
    }
}
