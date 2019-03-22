using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TransparentWindowsForms
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RESULT_CPLUS
    {

        public int x;
        public int y;
        public int w;
        public int h;
        public int objID;
        public float score;

    }
}
