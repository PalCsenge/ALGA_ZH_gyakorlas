using System;

namespace OE.ALGA.Optimalizalas
{
    public class HatizsakProblema
    {
        public int n { get; }
        public int Wmax { get; }
        public int[] w { get; }
        public float[] p { get; }

        public HatizsakProblema(int n, int Wmax, int[] w, float[] p)
        {
            this.n = n;
            this.Wmax = Wmax;
            this.w = w;
            this.p = p;
        }

        public int OsszSuly(bool[] pakolas)
        {
            int s = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i]) s += w[i];
            }
            return s;
        }

        public float OsszErtek(bool[] pakolas)
        {
            float s = 0;
            for (int i = 0; i < n; i++)
            {
                if (pakolas[i]) s += p[i];
            }
            return s;
        }

        public bool Ervenyes(bool[] pakolas)
        {
            return OsszSuly(pakolas) <= Wmax;
        }
    }

    public class NyersEro<T>
    {
        int m;
        Func<int, T> generator;
        Func<T, int> josag;

        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, int> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
        }

        public T OptimalisMegoldas()
        {
            T O = generator(1);

            for (int i = 2; i <= m; i++)
            {
                T K = generator(i);
                LepesSzam++;
                if (josag(K) > josag(O))
                    O = K;
            }
            return O;
        }
    }

    public class NyersEroHatizsakPakolas
    {
        HatizsakProblema problema;

        public int LepesSzam { get; private set; }

        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public bool[] OptimalisMegoldas()
        {
            int lehetsegesMegoldasok = Convert.ToInt32(Math.Pow(2, problema.n));
            NyersEro<bool[]> nyersEro = new NyersEro<bool[]>(lehetsegesMegoldasok, this.Generator, this.Josag);
            bool[] optimalisPakolas = nyersEro.OptimalisMegoldas();
            LepesSzam = nyersEro.LepesSzam;
            return optimalisPakolas;
        }

        public float OptimalisErtek()
        {
            bool[] optimalisPakolas = OptimalisMegoldas();
            return problema.OsszErtek(optimalisPakolas);
        }

        public bool[] Generator(int i)
        {
            int szam = i - 1;
            bool[] K = new bool[problema.n];
            for (int j = 0; j < problema.n; j++)
            {
                K[j] = (szam / (int)Math.Pow(2, j)) % 2 == 1;
            }
            return K;
        }

        public int Josag(bool[] pakolas)
        {
            if (problema.Ervenyes(pakolas))
            {
                return Convert.ToInt32(problema.OsszErtek(pakolas));
            }
            return -1;
        }
    }
}
