namespace CommandConsole
{
    public class Tuple<T1, T2>
    {
        public readonly T1 V1;
        public readonly T2 V2;

        public Tuple(T1 v1, T2 v2)
        {
            V1 = v1;
            V2 = v2;
        }
    }
}
