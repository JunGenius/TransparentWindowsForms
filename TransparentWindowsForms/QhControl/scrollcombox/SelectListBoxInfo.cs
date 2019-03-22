using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransparentWindowsForms.QhControl.scrollcombox
{
    public class SelectListBoxInfo
    {
        public SelectListBoxInfo(string info, bool isCheck)
        {
            Info = info;
            IsCheck = isCheck;
        }

        public string Info { get; set; }

        public bool IsCheck { get; set; }
    }
}
