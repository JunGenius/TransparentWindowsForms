using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TransparentWindowsForms.QhControl.entity
{
    public class EyeBoxClickInfo : IDisposable
    {
        public EyeBoxClickInfo()
        {
        }

        public EyeBoxClickInfo(int x, int y, string name , int type)
        {
            X = x;
            Y = y;
            Name = name;
            Type = type;
        }


        public int X { get; set; }

        public int Y { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public void Dispose()
        {
        }
    }
}
