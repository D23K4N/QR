using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace Core
{
    public class generator
    {

        public static double [] LCG(double seed)
        {
            //double seed = DateTime.Now.Millisecond;
            double m = 2000000;
            double [] seriesDouble = new double[(int)m];
            double  a = 16807;
            for (int i=0; i<m;i++)
            {
                if (i == 0) { seriesDouble[i] = ((a * seed) % (m + 1)); }
                else { seriesDouble[i] = ((a * seriesDouble[i - 1] * m) % (m + 1)); }
            }
            for (int i = 0; i < m; i++)
            {
             seriesDouble[i] = seriesDouble[i]/m;
            }
            return seriesDouble;
        }


    }

}