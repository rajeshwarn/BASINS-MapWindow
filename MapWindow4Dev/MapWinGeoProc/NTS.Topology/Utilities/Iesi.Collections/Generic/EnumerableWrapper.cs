using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Iesi.Collections.Generic
{
    /// <summary>
    /// A Simple Wrapper for wrapping an regular Enumerable as a generic Enumberable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <exception cref="InvalidCastException">
    /// If the wrapped has any item that is not of Type T, InvalidCastException could be thrown at any time
    /// </exception>
    public  class EnumerableWrapper <T> : IEnumerable<T>
    {
        private IEnumerable innerEnumerable;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerEnumerable"></param>
        public EnumerableWrapper(IEnumerable innerEnumerable)
        {
            this.innerEnumerable = innerEnumerable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!obj.GetType().Equals( this.GetType() )) return false;
            if (obj == this) return true;
            return this.innerEnumerable.Equals(((EnumerableWrapper<T>)obj).innerEnumerable);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #region IEnumerable<T> Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new EnumeratorWrapper<T>(this.innerEnumerable.GetEnumerator());
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.innerEnumerable.GetEnumerator();    
        }

        #endregion
       
    }
}
