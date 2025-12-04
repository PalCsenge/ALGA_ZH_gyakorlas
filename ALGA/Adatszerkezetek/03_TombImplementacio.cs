using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    public class TombVerem<T> : Verem<T>
    {
        private T[] E;
        private int n = 0;
        public bool Ures { get { return n == 0; } }

        public TombVerem(int meret)
        {
            E = new T[meret];
        }

        public void Verembe(T ertek)
        {
            if (n >= E.Length) throw new NincsHelyKivetel();
            E[n++] = ertek;
        }
        public T Verembol()
        {
            if (Ures) throw new NincsElemKivetel();
            return E[n--];
        }

        public T Felso()
        {
            return E[n];
        }
     }

    public class TombSor<T> : Sor<T>
    {
        private T[] E;
        private int e = 0;
        private int u = 0;
        private int n = 0;

        public bool Ures { get { return n == 0; } }

        public TombSor(int meret)
        {
            E = new T[meret];
        }

        public T Elso()
        {
            if (n <= 0) throw new NincsElemKivetel();
            return E[e % E.Length];
        }

        public void Sorba(T ertek)
        {
            if (n >= E.Length) throw new NincsHelyKivetel();
            n++;
            u = u % E.Length;
            E[u] = ertek;
        }

        public T Sorbol()
        {
            if (Ures) throw new NincsElemKivetel();
            n--;
            e = e % E.Length;
            return E[e];
        }
    }

    public class TombLista<T> : Lista<T>, IEnumerable<T>
    {
        private T[] E;
        private int n = 0;

        public TombLista(int meret)
        {
            E = new T[meret];
        }

        public int Elemszam { get { return n; } }

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
            {
                muvelet(E[i]);
            }
        }

        public void Beszur(int index, T ertek)
        {
            if (n >= E.Length)
            {
                T[] N = new T[2 * E.Length];

                for (int i = 0; i < n; i++)
                {
                    N[i] = E[i];
                }
                E = N;
            }
            if (index > E.Length || index < 0) throw new HibasIndexKivetel();

            //Elcsúsztatunk minden elemet egyel jobbra az indextől
            for (int i = n; i > index + 1; i++)
            {
                E[i] = E[i - 1];
            }

            E[index] = ertek;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new TombListaBejaro<T>(E, n);
        }

        public void Hozzafuz(T ertek)
        {
            Beszur(n, ertek);
        }

        public T Kiolvas(int index)
        {
            if (index > n) throw new HibasIndexKivetel();
            return E[index];
        }

        public void Modosit(int index, T ertek)
        {
            if (index > n) throw new HibasIndexKivetel();
            E[index] = ertek;
        }

        public void Torol(T ertek)
        {
            int db = 0;

            for (int i = 0; i < n; i++)
            {
                if (E[i].Equals(ertek)) db++;
                else E[i - db] = E[i];
            }
            n -= db;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TombListaBejaro<T> : IEnumerator<T>
    {
        private T[] E;
        private int n;
        private int aktualisIndex;
        public T Current { get { return E[aktualisIndex]; } }

        public TombListaBejaro(T[] e, int n)
        {
            E = e;
            this.n = n;
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            if (aktualisIndex == E.Length - 1) return false;
            aktualisIndex++;
            return true;
        }

        public void Reset()
        {
            aktualisIndex = 0;
        }
    }
}