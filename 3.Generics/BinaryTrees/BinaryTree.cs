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

        public T Value;

        private bool _isDefault = true;

        public BinaryTree()
        {
        }

        private BinaryTree(T item)
        {
            Value = item;
            _isDefault = false;
        }

        public void Add(T item) => Add(this, item);

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

        private static void Add(BinaryTree<T> current, T item)
        {
            if (current._isDefault)
            {
                current._isDefault = false;
                current.Value = item;
                return;
            }

            if (item.CompareTo(current.Value) <= 0 && current.Left == null)
                current.Left = new BinaryTree<T>(item);
            else if (item.CompareTo(current.Value) <= 0 && current.Left != null)
                Add(current.Left, item);

            if (item.CompareTo(current.Value) > 0 && current.Right == null)
                current.Right = new BinaryTree<T>(item);
            else if (item.CompareTo(current.Value) > 0 && current.Right != null)
                Add(current.Right, item);
        }
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