using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TransparentWindowsForms.QhControl.entity
{
    public class ListBoxItem : IDisposable
    {
        public ListBoxItem()
        {
        }

        public ListBoxItem(Guid id, string name, string ip, string mac, bool isCheck)
        {
            Id = id;
            Name = name;
            IP = ip;
            Mac = mac;
            IsFocus = false;
            IsCheck = isCheck;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IP { get; set; }

        public string Mac { get; set; }


        public bool IsFocus { get; set; }

        public bool IsCheck { get; set; }

        public void Dispose()
        {
        }
    }
}
