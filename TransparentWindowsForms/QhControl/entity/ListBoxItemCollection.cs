using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransparentWindowsForms.QhControl.scrollcombox;

namespace TransparentWindowsForms.QhControl.entity
{
    class ListBoxItemCollection : IList, ICollection, IEnumerable
    {
        private readonly UserListBox m_owner;

        public ListBoxItemCollection(UserListBox owner)
        {
            m_owner = owner;
        }

        internal UserListBox Owner
        {
            get { return m_owner; }
        }

        #region  override

        public ListBoxItem this[int index]
        {
            get { return Owner.OldItemSource[index] as ListBoxItem; }
            set { Owner.OldItemSource[index] = value; }
        }

        public int Count
        {
            get { return Owner.OldItemSource.Count; }
        }

        public bool IsReadOnly
        {
            get { return Owner.OldItemSource.IsReadOnly; }
        }

        int IList.Add(object value)
        {
            if (!(value is ListBoxItem))
            {
                throw new ArgumentException();
            }
            return Add(value as ListBoxItem);
        }

        void IList.Clear()
        {
            Clear();
        }

        bool IList.Contains(object value)
        {
            return Contains(value as ListBoxItem);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf(value as ListBoxItem);
        }

        void IList.Insert(int index, object value)
        {
            if (!(value is ListBoxItem))
            {
                throw new ArgumentException();
            }
            Insert(index, value as ListBoxItem);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            Remove(value as ListBoxItem);
        }

        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set
            {
                if (!(value is ListBoxItem))
                {
                    throw new ArgumentException();
                }
                this[index] = value as ListBoxItem;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((ListBoxItem[])array, index);
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return false; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Add(ListBoxItem item)
        {
            if (item == null)
            {
                throw new ArgumentException("item is null");
            }
            return Owner.OldItemSource.Add(item);
        }

        public void AddRange(ListBoxItem[] items)
        {
            Owner.OldItemSource.AddRange(items);
        }

        public void Clear()
        {
            if (Owner.OldItemSource.Count > 0)
            {
                Owner.OldItemSource.Clear();
            }
        }

        public bool Contains(ListBoxItem item)
        {
            bool rst = false;
            foreach (ListBoxItem oldItem in Owner.OldItemSource)
            {
                if (oldItem.Id == item.Id)
                {
                    rst = true;
                    break;
                }
            }
            return rst;
        }

        public void CopyTo(ListBoxItem[] destination, int arrayIndex)
        {
            Owner.OldItemSource.CopyTo(destination, arrayIndex);
        }

        public int IndexOf(ListBoxItem item)
        {
            return Owner.OldItemSource.IndexOf(item);
        }

        public void Insert(int index, ListBoxItem item)
        {
            if (item == null)
            {
                throw new ArgumentException("item is null");
            }
            Owner.OldItemSource.Insert(index, item);
        }

        public void Remove(ListBoxItem item)
        {
            Owner.OldItemSource.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Owner.OldItemSource.RemoveAt(index);
        }

        public IEnumerator GetEnumerator()
        {
            return Owner.OldItemSource.GetEnumerator();
        }

        #endregion

        #region  extention

        public ListBoxItem FindByMac(string mac)
        {
            foreach (ListBoxItem item in Owner.OldItemSource)
            {
                if (item.Mac == mac)
                {
                    return item;
                }
            }
            return null;
        }

        #endregion
    }
}
