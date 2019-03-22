using System;
using System.Collections.Generic;
using System.Text;

namespace TransparentWindowsForms
{
    public class PointAllEntity
    {
        public int rectNum = 0;
        public List<PointEntity> PointList = new List<PointEntity>();
    }

    public class MonitorPoint
    {

        private int _x = 0;

        private int _y = 0;

        private int _w = 0;

        private int _h = 0;

        private float _s = 0;
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int W
        {
            get { return _w; }
            set { _w = value; }
        }

        public int H
        {
            get { return _h; }
            set { _h = value; }
        }

        public float S
        {
            get { return _s; }
            set { _s = value; }
        }

    }
}
