using System;
using System.Collections;
using System.Collections.Generic;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable
    {
        public BinaryTree<T> Left;
        public BinaryTree<T> Right;

        private bool _isDefault = true;

        public T Value;

        public BinaryTree()
        { }

        public BinaryTree(T item)
        {
            Value = item;
            _isDefault = false;
        }

        public void Add(T item) => Add(this, item);

        private void Add(BinaryTree<T> current, T item)
        {
            if (current._isDefault)
            {
                current._isDefault = false;
                current.Value = item;
                return;
            }
            
            switch (item.CompareTo(current.Value) <= 0)
            {
                case true when current.Left == null:
                    current.Left = new BinaryTree<T>(item);
                    break;
                case true when current.Left != null:
                    Add(current.Left, item);
                    break;
            }
            
            switch (item.CompareTo(current.Value) > 0)
            {
                case true when current.Right == null:
                    current.Right = new BinaryTree<T>(item);
                    break;
                case true when current.Right != null:
                    Add(current.Right, item);
                    break;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Left != null)
                foreach (var node in Left)
                    yield return node;

            if (!_isDefault)
                yield return Value;
            
            if (Right != null)
                foreach (var node in Right)
                    yield return node;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class BinaryTree
    {
        public static BinaryTree<T> Create<T>(params T[] items)
            where T : IComparable
        {
            var bt = new BinaryTree<T>();
            foreach (var item in items)
                bt.Add(item);

            return bt;
        }
    }
}
