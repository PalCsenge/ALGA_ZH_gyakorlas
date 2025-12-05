using OE.ALGA.Paradigmak;
using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class FaElem<T> where T : IComparable
    {
        public T tart;
        public FaElem<T>? bal;
        public FaElem<T>? jobb;
        public int darab = 1;

        public FaElem(T tart, FaElem<T>? bal, FaElem<T>? jobb)
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;
        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable
    {
        FaElem<T> gyoker = null;

        public FaHalmaz()
        {
        }

        public int Elemszam
        {
            get
            {
                int db = 0;
                BejarElemszam(x =>
                {
                    db += x.darab;
                });
                return db;
            }
            private set { Elemszam = value; }
        }

        public void Bejar(Action<T> muvelet)
        {
            ReszfaBejarasPreOrder(gyoker, muvelet);
        }

        public void BejarElemszam(Action<FaElem<T>> muvelet)
        {
            ReszfaBejaras(gyoker, muvelet);
        }

        public void Beszur(T ertek)
        {
            if (Eleme(ertek))
            {
                Kereses(ertek).darab++;
            }
            else
            {
                gyoker = ReszfabaBeszur(gyoker, ertek);
            }
        }

        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }

        public void Torol(T ertek)
        {
            if (Eleme(ertek))
            {
                Kereses(ertek).darab--;
                if (Kereses(ertek).darab == 0)
                {
                    gyoker = ReszfabolTorol(gyoker, ertek);
                }
            }
        }

        public int Darab(T ertek)
        {
            if (Eleme(ertek))
                return Kereses(ertek).darab;
            return 0;
        }

        public FaElem<T> Kereses(T ertek)
        { 
            return PrivateKereses(gyoker, ertek);
        }
        private FaElem<T> PrivateKereses(FaElem<T> csicsk, T ertek) 
        { 
            int cmp = csicsk.tart.CompareTo(ertek); 
            if (cmp > 0) 
                return PrivateKereses(csicsk.bal, ertek);
            else if (cmp < 0) 
                return PrivateKereses(csicsk.jobb, ertek);
            else 
                return csicsk;
        }

        private static protected FaElem<T> ReszfabaBeszur(FaElem<T> p, T ertek)
        {
            if (p == null)
            {
                return new FaElem<T>(ertek, null, null);
            }

            int cmp = p.tart.CompareTo(ertek);

            if (cmp == 1)
            {
                p.bal = ReszfabaBeszur(p.bal, ertek);
            }
            else if (cmp == -1)
            {
                p.jobb = ReszfabaBeszur(p.jobb, ertek);
            }

            return p;
        }

        private static protected bool ReszfaEleme(FaElem<T> p, T ertek)
        {
            if (p == null) return false;

            int cmp = p.tart.CompareTo(ertek);

            if (cmp > 0)
                return ReszfaEleme(p.bal, ertek);
            else if (cmp < 0)
                return ReszfaEleme(p.jobb, ertek);
            else
                return true;
        }

        private void ReszfaBejarasPreOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }
        }

        private void ReszfaBejaras(FaElem<T> p, Action<FaElem<T>> muvelet)
        {
            if (p != null)
            {
                muvelet(p);
                ReszfaBejaras(p.bal, muvelet);
                ReszfaBejaras(p.jobb, muvelet);
            }
        }

        private static protected FaElem<T> ReszfabolTorol(FaElem<T> p, T ertek)
        {
            if (p == null)
                throw new NincsElemKivetel();

            int cmp = p.tart.CompareTo(ertek);

            if (cmp > 0)
            {
                p.bal = ReszfabolTorol(p.bal, ertek);
            }
            else if (cmp < 0)
            {
                p.jobb = ReszfabolTorol(p.jobb, ertek);
            }
            else
            {
                if (p.bal == null)
                {
                    return p.jobb;
                }
                else if (p.jobb == null)
                {
                    return p.bal;
                }
                else
                {
                    p.bal = KetGyerekesTorles(p, p.bal);
                }
            }

            return p;
        }

        private static FaElem<T> KetGyerekesTorles(FaElem<T> e, FaElem<T> r)
        {
            if (r.jobb != null)
            {
                r.jobb = KetGyerekesTorles(e, r.jobb);
                return r;
            }
            else
            {
                e.tart = r.tart;
                return r.bal;
            }
        }
    }
}
