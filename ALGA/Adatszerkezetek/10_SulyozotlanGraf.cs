using System;
using System.Collections.Generic;
using System.Linq;

namespace OE.ALGA.Adatszerkezetek
{
    public class EgeszGrafEl : GrafEl<int>, IComparable
    {
        public EgeszGrafEl(int honnan, int hova)
        {
            Honnan = honnan;
            Hova = hova;
        }

        public int Honnan {  get; }
        public int Hova { get; }

        public int CompareTo(object? obj)
        {
            if (obj != null && obj is EgeszGrafEl b)
            {
                if (Honnan != b.Honnan)
                    return Honnan.CompareTo(b.Honnan);
                else
                    return Hova.CompareTo(b.Hova);
            }
            else
                throw new InvalidOperationException();
        }
    }

    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        private int n;
        private bool[,] M;

        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            M = new bool[n, n];
        }

        public int CsucsokSzama { get { return n; } }

        public int ElekSzama { get { return M.Cast<bool>().Count(x => x); } }

        public Halmaz<int> Csucsok { 
            get 
            { 
                Halmaz<int> fa = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                    fa.Beszur(i);               
                return fa;
            } 
        }

        public Halmaz<EgeszGrafEl> Elek 
        { 
            get
            {
                FaHalmaz<EgeszGrafEl> fa = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i, j])
                        {
                            fa.Beszur(new EgeszGrafEl(i, j));
                        }
                    }
                }
                return fa;
            }
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> fa = new FaHalmaz<int>();
            for (int j = 0; j < n; j++)
            {
                if (M[csucs, j])
                {
                    fa.Beszur(j);
                }
            }
            return fa;
        }

        public void UjEl(int honnan, int hova)
        {
            M[honnan, hova] = true;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return M[honnan, hova];
        }
    }

    public class GrafBejarasok
    {
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable
        {
            Sor<V> S = new LancoltSor<V>();
            Halmaz<V> F = new FaHalmaz<V>();
            S.Sorba(start);
            F.Beszur(start);

            while (!S.Ures)
            {
                V k = S.Sorbol();
                muvelet(k);

                g.Szomszedai(k).Bejar((x) =>
                {
                    if (!F.Eleme(x))
                    {
                        S.Sorba(x);
                        F.Beszur(x);
                    }
                });
            }

            return F;
        }

        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable
        {
            Halmaz<V> F = new FaHalmaz<V>();

            MelysegiBejarasRekurzio(g, start, F, muvelet);

            return F;
        }

        public static void MelysegiBejarasRekurzio<V, E>(Graf<V, E> g, V k, Halmaz<V> F, Action<V> muvelet)
        {
            F.Beszur(k);
            muvelet(k);

            g.Szomszedai(k).Bejar((x) =>
            {
                if (!F.Eleme(x))
                    MelysegiBejarasRekurzio(g, x, F, muvelet);
            });
        }
    }
}