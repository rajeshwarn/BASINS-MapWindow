using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Iesi.Collections.Generic
{
    /// <summary>
    /// A wrapper that can wrap a ISet as a generic ISet&lt;T&gt; 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// In most operations, there is no copying of collections. The wrapper just delegate the function to the wrapped.
    /// The following functions' implementation may involve collection copying:
    /// Union, Intersect, Minus, ExclusiveOr, ContainsAll, AddAll, RemoveAll, RetainAll
    /// </remarks>
    /// <exception cref="InvalidCastException">
    /// If the wrapped has any item that is not of Type T, InvalidCastException could be thrown at any time
    /// </exception>
    public sealed class SetWrapper<T> : ISet<T>
    {
        private ISet innerSet;
        
        
        private SetWrapper(){}
        
        /// <summary>
        /// SetWrapper based on an ISet
        /// </summary>
        /// <param name="toWrap"></param>
        public SetWrapper(ISet toWrap)
        {
            if (toWrap == null)
                throw new ArgumentNullException();
            this.innerSet = toWrap;
            
        }
       
        #region ISet<T> Members

        #region Operators

        /// <summary>
        /// Joines two sets
        /// </summary>
        /// <param name="a">an ISet to merge with</param>
        /// <returns>the union of this set and the ISet specified</returns>
        public ISet<T> Union(ISet<T> a)
        {
            return getSetCopy().Union(a);
        }
        /// <summary>
        /// THe intersection of this set and the specified ISet
        /// </summary>
        /// <param name="a">An Iset</param>
        /// <returns>The collection of items that are in both sets</returns>
        public ISet<T> Intersect(ISet<T> a)
        {
            return getSetCopy().Intersect(a);  
        }

        /// <summary>
        /// removes the items from the specified ISet from this set
        /// </summary>
        /// <param name="a">An ISet to subtract</param>
        /// <returns>the items that are in this set but not in the specified ISet</returns>
        public ISet<T> Minus(ISet<T> a)
        {
            return getSetCopy().Minus(a);
        }

        /// <summary>
        /// Returns the items that exist in one set or the other, but not both
        /// </summary>
        /// <param name="a">The set to compare against</param>
        /// <returns>An ISet with the items in this set or the specified set but not both</returns>
        public ISet<T> ExclusiveOr(ISet<T> a)
        {
            return getSetCopy().ExclusiveOr(a);
        } 
        
        #endregion

        /// <summary>
        /// Tests if the specified item is in this set
        /// </summary>
        /// <param name="o">An item to test</param>
        /// <returns>Boolean, true if the item is in the set</returns>
        public bool Contains(T o)
        {
            return innerSet.Contains(o);
        }

        /// <summary>
        /// Tests if every member of an ICollection, and returns true if they are all in this set
        /// </summary>
        /// <param name="c">The collection of items to test</param>
        /// <returns>Boolean, true if every member of the specified collection is in this set</returns>
        public bool ContainsAll(ICollection<T> c)
        {
            return innerSet.ContainsAll(getSetCopy(c));
        }

        /// <summary>
        /// Boolean, true if this set has no items
        /// </summary>
        public bool IsEmpty
        {
            get { return innerSet.IsEmpty; }
        }
        
        /// <summary>
        /// Adds a member to the set
        /// </summary>
        /// <param name="o">an item to add</param>
        /// <returns>true if the add was successful</returns>
        public bool Add(T o)
        {
            return innerSet.Add(o);
        }

        /// <summary>
        /// Adds every member of a collection to the set
        /// </summary>
        /// <param name="c">The collection with members to add</param>
        /// <returns>True if the add was successful</returns>
        public bool AddAll(ICollection<T> c)
        {
            return innerSet.AddAll(getSetCopy(c));
        }

        /// <summary>
        /// Removes a specified item from the set
        /// </summary>
        /// <param name="o">the item to remove</param>
        /// <returns>Boolean, true if the removal was successful</returns>
        public bool Remove(T o)
        {
            return innerSet.Remove(o);
        }

        /// <summary>
        /// Removes a collection from the set
        /// </summary>
        /// <param name="c">The collection to remove</param>
        /// <returns>true if the removal was successful</returns>
        public bool RemoveAll(ICollection<T> c)
        {
            return innerSet.RemoveAll(getSetCopy(c));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool RetainAll(ICollection<T> c)
        {
            return innerSet.RemoveAll(getSetCopy(c));
        }

        /// <summary>
        /// Clears the members from this collection
        /// </summary>
        public void Clear()
        {
            innerSet.Clear();
        }

        /// <summary>
        /// Creates a duplicate of this set
        /// </summary>
        /// <returns>An ISet duplicate of this set</returns>
        public ISet<T> Clone()
        {
            return new SetWrapper<T>((ISet)innerSet.Clone());
        }
        /// <summary>
        /// The number of elements in this set
        /// </summary>
        public int Count
        {
            get {
                return innerSet.Count;
            }
        }

    
        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            innerSet.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Tests if this set is read only... this is always false
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that can be used to iterate through the members of this set
        /// </summary>
        /// <returns> Returns an IEnumerator that can be used to iterate through the members of this set</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new EnumeratorWrapper<T>(innerSet.GetEnumerator());
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return innerSet.GetEnumerator();
        }

        #endregion

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region private methods
        private Set<T> getSetCopy(ICollection<T> c)
        {
            return new HashedSet<T>(c);
        }
        private Set<T> getSetCopy(ICollection c)
        {
            Set<T> retVal = new HashedSet<T>();
            ((ISet)retVal).AddAll(c);
            return retVal;
        }
        private Set<T> getSetCopy()
        {
            return getSetCopy(innerSet);
        } 
        #endregion
    }
}
