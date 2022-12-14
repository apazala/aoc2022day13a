internal class Program
{
    abstract class Signal
    {
        public static Signal CreateSignal(string s, ref int i)
        {   
            if(s[i] == ',')
                i++;         
            bool createArr = (s[i] == '[');
            if(createArr){
                i++;
                return new SignalArr(s, ref i);
            }
            else
                return new SignalInt(s, ref i);
        }

        abstract public int CompareTo(Signal other);
    }

    class SignalArr:Signal
    {
        private List<Signal> itemsList;

        public SignalArr(SignalInt sig)
        {
            itemsList = new List<Signal>();
            itemsList.Add(sig);
        }
        public SignalArr(string s, ref int i)
        {
            itemsList = new List<Signal>();
            while(s[i] != ']')
            {
                itemsList.Add(Signal.CreateSignal(s, ref i));
            }
            i++;//skip ']'
        }

        public override int CompareTo(Signal other)
        {
            SignalArr sig2;
            if(other is SignalInt){
                sig2 = new SignalArr((SignalInt)other);
            } else {
                sig2 = (SignalArr)other;
            }

            int cmp = 0;
            int count = (itemsList.Count < sig2.itemsList.Count? itemsList.Count:sig2.itemsList.Count);
            for(int i = 0; cmp == 0 && i < count; i++)
            {
                cmp = itemsList[i].CompareTo(sig2.itemsList[i]);
            }

            return (cmp != 0? cmp : itemsList.Count - sig2.itemsList.Count);
        }

        public override string ToString()
        {
            string str = "[";
            if(itemsList.Count > 0)
                str += itemsList[0];
            for(int i = 1; i < itemsList.Count; i++)
                str += ", " + itemsList[i];
            str += "]";
            return str;
        }

    }

    class SignalInt:Signal
    {
        int v;
        public SignalInt(string s, ref int i)
        {
            v = 0;
            while(i < s.Length && s[i] >= '0' && s[i] <= '9')
            {
                v = 10*v + (s[i] - '0');
                i++;
            }
        }

        public override int CompareTo(Signal other)
        {
            SignalInt sig2 = other as SignalInt;
            if(sig2 == null){
                SignalArr thisArr = new SignalArr(this);
                return thisArr.CompareTo(other);
            }
            
            return v - sig2.v;
        }

        public override string ToString()
        {
            return v.ToString();
        }
    }

    private static void Main(string[] args)
    {
        int sum = 0;
        string[] lines = File.ReadAllLines("input.txt");
        for(int i = 0, pairIndex = 1; i < lines.Length; i++, pairIndex++)
        {
            int j = 0;
            Signal sig1 = Signal.CreateSignal(lines[i++], ref j);
            j = 0;
            Signal sig2 = Signal.CreateSignal(lines[i++], ref j);

            if(sig1.CompareTo(sig2) < 0)
                sum += pairIndex;
        }

        Console.WriteLine(sum);
    }
}