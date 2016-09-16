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
    public class CollectionWrapper<T> : EnumerableWrapper<T>, ICollection<T>
    {
        private ICollection innerCollection;
        /// <summary>
        /// Wraps an ICollection
        /// </summary>
        /// <param name="toWrap"></param>
        public CollectionWrapper(ICollection toWrap) :base(toWrap)
        {
            this.innerCollection = toWrap;
        }
        

        #region ICollection<T> Members

        /// <summary>
        /// Throws a NotSupportedException because the collection is readonly
        /// </summary>
        /// <param name="item">the item to add</param>
        public void Add(T item)
        {
            ThrowReadOnlyException();
        }
        
        /// <summary>
        /// Throws a NotSupportedException because the collection is readonly
        /// </summary>
        public void Clear()
        {
            ThrowReadOnlyException();
        }

        /// <summary>
        /// Tests if an item is contained within the set
        /// </summary>
        /// <param name="item">the item to test</param>
        /// <returns>Boolean if the item exists in the set</returns>
        public bool Contains(T item)
        {
            foreach (object o in innerCollection)
                if ( (object)item == o) return true;
            return false;
        }

        /// <summary>
        /// Copies an array of items to the collection
        /// </summary>
        /// <param name="array">an array to copy to the set</param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            innerCollection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// The number of items in the collection
        /// </summary>
        public int Count
        {
            get { return innerCollection.Count; }
        }

        /// <summary>
        /// Tests if this is read only, and it is always true.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; //always return true since the old ICollection does not support mutation 
            }
        }

        /// <summary>
        ///Throws a NotSupportedException because the collection is readonly
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            return ThrowReadOnlyException();
        }

        #endregion
        
        /// <summary>
        /// Throws a NotSupportedException because the collection is readonly
        /// </summary>
        /// <returns></returns>
        private bool ThrowReadOnlyException()
        {
            throw new NotSupportedException("The ICollection is read-only.");
        
        }
    }
}
