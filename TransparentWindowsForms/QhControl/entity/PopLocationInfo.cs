using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransparentWindowsForms.QhControl.entity
{
    public class PopLocationInfo
    {
        public PopLocationInfo(Point point , PopForm pop)
        {
            PopForm = pop;

            Point = point;
        }

        public string Id { get; set; }

        public Point Point { get; set; }

        public PopForm PopForm { get; set; }

        private string GetGuid()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}
