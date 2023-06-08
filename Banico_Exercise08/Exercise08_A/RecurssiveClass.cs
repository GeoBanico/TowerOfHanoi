using System;
using System.Collections.Generic;
using System.Text;

namespace Banico_Exercise08
{
    class RecurssiveClass
    {
        public int GCD(int m, int n) 
        {
            if (m % n == 0) 
            {
                return n;
            }

            return GCD(n, m % n);
        }
                                        
        public double SummingSeries(double i)  
        {
            if (i == 0) return 0;

            double seq = i / (2 * i + 1);

            return  seq + SummingSeries(i-1);
        }

        public int SummingInts(int num) 
        {
            if (num == 0) return 0;

            return num % 10 + SummingInts(num / 10);
            
        }
    }
}
