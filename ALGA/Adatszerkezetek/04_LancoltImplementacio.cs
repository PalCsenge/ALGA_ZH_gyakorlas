using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace OE.ALGA.Adatszerkezetek
{
    public class LancElem<T>
    {
        public T tart;
        public LancElem<T>? kov;

        public LancElem(T tart, LancElem<T>? kov)
        {
            this.tart = tart;
            this.kov = kov;
        }
    }

    public class LancoltVerem<T> : Verem<T>
    {
        LancElem<T>? fej;

        public LancoltVerem()
        {
            fej = null;
        }

        public bool Ures { get { return fej == null; } }

        public T Felso()
        {
            if (fej == null)
            {
                throw new NincsElemKivetel();
            }

            return fej.tart;
        }

        public void Verembe(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, fej);
            fej = uj;
        }

        public T Verembol()
        {
            if (fej == null) throw new NincsElemKivetel();

            LancElem<T>? q = fej;
            fej = fej.kov;
            return q.tart;
        }
    }

    public class LancoltSor<T> : Sor<T>
    {
        LancElem<T>? fej;
        LancElem<T>? vege;

        public LancoltSor()
        {
            fej = null;
            vege = null;
        }

        public bool Ures {get { return fej == null; } }

        public T Elso()
        {
            if (fej == null) throw new NincsElemKivetel();
            return fej.tart;
        }

        public void Sorba(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);
            
            if (vege != null) vege.kov = uj;
            else fej = uj;
            vege = uj;
        }

        public T Sorbol()
        {
            if (fej == null) throw new NincsElemKivetel();

            LancElem<T>? q = fej;
            fej = fej.kov;

            if (fej == null) vege = null;

            return q.tart;
        }
    }

    public class LancoltLista<T> : Lista<T>, IEnumerable<T>
    {
        LancElem<T>? fej;
        int n = 0;

        public LancoltLista()
        {
            fej = null;
        }

        public int Elemszam { get { Bejar(); return n; } }

        public void Bejar()
        {
            LancElem<T>? p = fej;
            n = 0;

            while (p != null)
            {
                p = p.kov;
                n++;
            }
        }

        public void Bejar(Action<T> muvelet)
        {
            LancElem<T>? p = fej;

            while (p != null)
            {
                muvelet(p.tart);
                p = p.kov;
            }
        }

        public virtual void Beszur(int index, T ertek)
        {
            if (fej == null || index == 0)
            {
                LancElem<T> uj = new LancElem<T>(ertek, fej);
                fej = uj;
            }
            else
            {
                LancElem<T>? p = fej;
                int i = 0;

                while (p.kov != null && i < index - 1)
                {
                    p = p.kov;
                    i++;
                }

                if (i <= index - 1)
                {
                    LancElem<T> uj = new LancElem<T>(ertek, p.kov);
                    p.kov = uj;                   
                }
                else
                {
                    throw new HibasIndexKivetel();
                }               
            }                
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LancoltListaBejaro<T>(fej);
        }

        public virtual void Hozzafuz(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);

            if (fej == null)
            {
                fej = uj;
                return;
            }

            LancElem<T>? p = fej;
            while (p.kov != null)
            {
                p = p.kov;
            }
            p.kov = uj;
        }

        public T Kiolvas(int index)
        {
            LancElem<T>? p = fej;
            int i = 0;

            while(p !=  null && i < index)
            {
                p = p.kov;
                i++;
            }

            if (p == null)
            {
                throw new HibasIndexKivetel();
            }
            return p.tart;
        }

        public virtual void Modosit(int index, T ertek)
        {
            LancElem<T>? p = fej;
            int i = 0;

            while (p != null && i < index)
            {
                p = p.kov;
                i++;
            }

            if (p == null)
            {
                throw new HibasIndexKivetel();
            }
            p.tart = ertek;
        }

        public void Torol(T ertek)
        {
            LancElem<T>? p = fej;
            LancElem<T>? e = null;

            while (p != null)
            {
                if (p.tart.Equals(ertek))
                {
                    if (e == null)
                    {
                        fej = p.kov;
                    }
                    else
                    {
                        e.kov = p.kov;
                    }
                    p = p.kov;
                }
                else
                {
                    e = p;
                    p = p.kov;
                }
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class LancoltListaBejaro<T> : IEnumerator<T>
    {
        LancElem<T>? fej;
        LancElem<T>? aktualisElem;

        public LancoltListaBejaro(LancElem<T>? fej)
        {
            this.fej = fej;
            aktualisElem = null;
        }

        public T Current { get { return aktualisElem.tart; } }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (aktualisElem != null)
            {
                aktualisElem = aktualisElem.kov;
                return aktualisElem != null;
            }
            aktualisElem = fej;
            return aktualisElem != null;
        }

        public void Reset()
        {
            aktualisElem = null;
        }
    }

    //public class FeltetelesLancoltLista<T> : LancoltLista<T>
    //{
    //    Predicate<T> feltetel;
    //    public FeltetelesLancoltLista(Predicate<T> pred)
    //    {
    //        feltetel = pred;
    //    }

    //    public override void Beszur(int index, T ertek)
    //    {
    //        if (feltetel(ertek)) base.Beszur(index, ertek);
    //        else throw new FeltetelNemTeljesulException();
    //    }

    //    public override void Hozzafuz(T ertek)
    //    {
    //        if (feltetel(ertek)) base.Hozzafuz(ertek);
    //        else throw new FeltetelNemTeljesulException();
    //    }

    //    public override void Modosit(int index, T ertek)
    //    {
    //        if (feltetel(ertek)) base.Modosit(index, ertek);
    //        else throw new FeltetelNemTeljesulException();
    //    }

    //    public void FelteteltModosit(Predicate<T> pred)
    //    {
    //        bool nem = true;
    //        for (int i = 0; i < Elemszam; i++)
    //        {
    //            if (!pred(Kiolvas(i))) nem = false;
    //        }

    //        if (nem) feltetel = pred;
    //    }
    //}

    //public class FeltetelNemTeljesulException : Exception
    //{
    //    public FeltetelNemTeljesulException()
    //    {
    //    }
    //}
}