using System;

namespace OE.ALGA.Adatszerkezetek
{
    public class SzotarElem<K, T>
    {
        public K kulcs;
        public T tart;

        public SzotarElem(K kulcs, T tart)
        {
            this.kulcs = kulcs;
            this.tart = tart;
        }
    }

    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        SzotarElem<K, T>[] E;
        Func<K, int> h;
        LancoltLista<SzotarElem<K, T>> U;

        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this(meret, x => x.GetHashCode())
        {
        }

        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K, int> hasitoFuggveny)
        {
            E = new SzotarElem<K, T>[meret];
            h = x => Math.Abs(hasitoFuggveny(x)) % E.Length;
            U = new LancoltLista<SzotarElem<K, T>>();
        }

        public void Beir(K kulcs, T ertek)
        {
            SzotarElem<K, T> meglevo = KulcsKeres(kulcs);
            if (meglevo != null)
            {
                meglevo.tart = ertek;
            }
            else
            {
                var uj = new SzotarElem<K, T>(kulcs, ertek);
                int index = h(kulcs);
                if (E[index] == null)
                {
                    E[index] = uj;
                }
                else
                {
                    U.Hozzafuz(uj);
                }
            }
        }

        public T Kiolvas(K kulcs)
        {
            SzotarElem<K, T> meglevo = KulcsKeres(kulcs);

            if (meglevo == null)
            {
                throw new HibasKulcsKivetel();
            }
            return meglevo.tart;
        }

        public void Torol(K kulcs)
        {
            int index = h(kulcs);
            if (E[index] != null && E[index].kulcs.Equals(kulcs))
            {
                E[index] = null;
            }
            else
            {
                SzotarElem<K, T> e = null;

                foreach (var item in U)
                {
                    if (item.kulcs.Equals(kulcs))
                    {
                        e = item;
                        break;
                    }
                }

                if (e == null)
                {
                    throw new HibasKulcsKivetel();
                }
                U.Torol(e);
            }
        }

        private SzotarElem<K, T> KulcsKeres(K kulcs)
        {
            int index = h(kulcs);
            SzotarElem<K, T> elem = E[index];
            if (elem != null && elem.kulcs.Equals(kulcs))
            {
                return elem;
            }

            foreach (var item in U)
            {
                if (item.kulcs.Equals(kulcs))
                {
                    return item;
                }
            }
            return null;
        }
    }
}
