using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Iesi.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListWrapper<T> : EnumerableWrapper<T>,  IList<T>
    {

        private IList innerList;
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toWrapp"></param>
        public ListWrapper(IList toWrapp):base(toWrapp)
        {
            this.innerList = toWrapp;
        }
       
        #region IList<T> Members
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return innerList.IndexOf(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            innerList.Insert(index, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            innerList.Remove(index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                return (T)innerList[index];
            }
            set
            {
                innerList[index] = value;
            }
        }

        #endregion

        #region ICollection<T> Members
        /// <summary>
        /// Add a new item to the collection
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            innerList.Add(item);
        }
        /// <summary>
        /// Clear the items in this list
        /// </summary>
        public void Clear()
        {
            innerList.Clear();
        }
        /// <summary>
        /// Determine if an item is contained in the list
        /// </summary>
        /// <param name="item">The item to find</param>
        /// <returns>Boolean, true if the item is already a member of the list</returns>
        public bool Contains(T item)
        {
            return innerList.Contains(item);
        }

        /// <summary>
        /// Copies an array of items to this list
        /// </summary>
        /// <param name="array">An array of items to add</param>
        /// <param name="arrayIndex">an array Index</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Number of members in the list
        /// </summary>
        public int Count
        {
            get { return innerList.Count; }
        }

        /// <summary>
        /// Boolean, true if the inner list is read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return innerList.IsReadOnly; }
        }

        /// <summary>
        /// Removes an item from the list
        /// </summary>
        /// <param name="item">the item to remove</param>
        /// <returns>Boolean, true if the item was removed</returns>
        public bool Remove(T item)
        {
            if (!innerList.Contains(item))
                return false;
            innerList.Remove(item);
            return true;
        }

        #endregion

       
       
    }
}
