using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Paradigmak
{
    public interface IVegrehajthato
    {
        void Vegrehajtas();
    }

    public interface IFuggo
    {
        public bool FuggosegTeljesul { get; }
    }

    public class TaroloMegteltKivetel : Exception
    {

    }

    public class FeladatTaroloBejaro<T> : IEnumerator<T>
    {
        T[] tarolo;
        int n;
        int aktualisEgesz = -1;

        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
        }

        public T Current
        {
            get
            {
                return tarolo[aktualisEgesz];
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (aktualisEgesz < n - 1)
            {
                aktualisEgesz++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            aktualisEgesz = -1;
        }

        public void Dispose()
        {
        }
    }

    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
    {
        protected T[] tarolo;
        protected int n = 0;

        public FeladatTarolo(int meret)
        {
            tarolo = new T[meret];
        }

        public void Felvesz(T elem)
        {
            if (n < tarolo.Length)
            {
                tarolo[n++] = elem;
            }
            else
            {
                throw new TaroloMegteltKivetel();
            }
        }

        public virtual void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                tarolo[i].Vegrehajtas();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            FeladatTaroloBejaro<T> bejaro = new FeladatTaroloBejaro<T>(tarolo, n);
            return bejaro;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {
        public FuggoFeladatTarolo(int meret) : base(meret)
        {
        }

        public override void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                if (tarolo[i].FuggosegTeljesul)
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }
    }
}
