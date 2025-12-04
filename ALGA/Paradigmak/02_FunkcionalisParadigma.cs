using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace OE.ALGA.Paradigmak
{
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T>, IEnumerable<T> where T : IVegrehajthato
    {

        public Func<T, bool> BejaroFeltetel { get; set; }

        public FeltetelesFeladatTarolo(int meret) : base(meret)
        {
        }

        public void FeltetelesVegrehajtas(Func<T, bool> feltetel)
        {
            for (int i = 0; i < n; i++)
            {
                if (feltetel(tarolo[i]))
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }

        public new IEnumerator<T> GetEnumerator()
        {
            if (BejaroFeltetel == null)
            {
                return new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, (T) => { return true; });
            }
            return new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, BejaroFeltetel);
        }
    }

    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T>
    {
        T[] tarolo;
        int egesz;
        int aktualisIndex = -1;
        Func<T, bool> bejaroFeltetel;

        public FeltetelesFeladatTaroloBejaro(T[] tarolo, int egesz, Func<T, bool> bejaroFeltetel)
        {
            this.tarolo = tarolo;
            this.egesz = egesz;
            this.bejaroFeltetel = bejaroFeltetel;
        }

        public object Current => Current;

        T IEnumerator<T>.Current => tarolo[aktualisIndex];

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            while (++aktualisIndex < egesz)
            {
                if (bejaroFeltetel(tarolo[aktualisIndex]))
                {
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            aktualisIndex = -1;
        }
    }
}
