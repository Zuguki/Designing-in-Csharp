using System;
using System.Collections;
using System.Collections.Generic;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T>
    {
        private List<T> _items = new List<T>();
        
        public BinaryTree()
        {
            
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public T Value()
        {
            
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>) _items).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
