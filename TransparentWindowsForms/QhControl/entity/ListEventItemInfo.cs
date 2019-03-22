using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransparentWindowsForms.QhControl.entity
{
    public class ListEventItemInfo
    {

        private int _pos = -1;

        private bool _status = false;

        public ListEventItemInfo(int pos, bool status)
        {
            _pos = pos;
            _status = status;
        }
        public int Pos
        {
            get { return _pos; }
            set
            {
                _pos = value;
            }
        }

        public bool Status
        {
            get { return _status; }
            set
            {
                _status = value;
            }
        }
    }

}
