using System;
using System.Collections.Generic;
using System.Text;

namespace Banico_Exercise08.TowerOfHanoi
{
    public interface ToH_Interface
    {
        int Name { get; set; }
        string CurrentPosition { get; set; }
        int Level { get; set; }
        string CurrentPeg { get; set; }
    }

    public class Peg : ToH_Interface
    {
        public string CurrentPosition { get; set; }
        public int Name { get; set; }
        public int Level { get; set; }
        public string CurrentPeg { get; set; }

        public Peg(int name, string curpos, int lev, string curpeg) 
        {
            CurrentPosition = curpos;
            Name = name;
            Level = lev;
            CurrentPeg = curpeg;
        }
    }
}
