using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransparentWindowsForms.QhControl
{
    public class EyeBoxDetailInfo
    {
        public EyeBoxDetailInfo(string title, string sub_title, string info)
        {
            Title = title;
            SubTitle = sub_title;
            Info = info;
        }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Info { get; set; }
    }
}
