using System;
using System.Collections.Generic;

namespace OE.ALGA.Optimalizalas
{
    public class DinamikusHatizsakPakolas
    {
        HatizsakProblema problema;

        public int LepesSzam { get; private set; }

        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            LepesSzam = 0;
        }

        public int[,] TablazatFeltoltes()
        {
            int[,] tablazat = new int[problema.n + 1, problema.Wmax + 1];

            for (int i = 0; i <= problema.n; i++)
            {
                for (int j = 0; j <= problema.Wmax; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        tablazat[i, j] = 0;
                    }
                    else if (problema.w[i - 1] <= j)
                    {
                        tablazat[i, j] = Math.Max(
                            tablazat[i - 1, j],
                            tablazat[i - 1, j - problema.w[i - 1]] + (int)problema.p[i - 1]
                        );
                        LepesSzam++;
                    }
                    else
                    {
                        tablazat[i, j] = tablazat[i - 1, j];
                        LepesSzam++;
                    }    
                }          
            }

            return tablazat;
        }

        public int OptimalisErtek()
        {
            int[,] tablazat = TablazatFeltoltes();
            return tablazat[problema.n, problema.Wmax];
        }

        public bool[] OptimalisMegoldas()
        {
            int[,] tablazat = TablazatFeltoltes();
            bool[] optimalisMegoldas = new bool[problema.n];

            int t = problema.n;
            int h = problema.Wmax;
            while(t > 0 && h > 0)
            {
                if (tablazat[t, h] != tablazat[t - 1, h])
                {
                    optimalisMegoldas[t - 1] = true;
                    h -= problema.w[t - 1];
                }
                t--;
            }

            return optimalisMegoldas;
        }
    }
}
