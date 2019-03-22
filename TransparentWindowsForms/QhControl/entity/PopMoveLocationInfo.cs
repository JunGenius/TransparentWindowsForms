using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransparentWindowsForms.QhControl.entity
{
    public class PopMoveLocationInfo
    {
        public PopMoveLocationInfo(Point point , PopMoveForm pop)
        {
            PopForm = pop;

            Point = point;
        }

        public string Id { get; set; }

        public Point Point { get; set; }

        public PopMoveForm PopForm { get; set; }

        private string GetGuid()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            return guid.ToString();
        }
    }
}
