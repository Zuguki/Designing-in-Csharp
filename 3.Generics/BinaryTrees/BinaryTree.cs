using System;
using System.Collections;
using System.Collections.Generic;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable
    {
        public BinaryTree<T> Parent;
        public BinaryTree<T> Left;
        public BinaryTree<T> Right;

        public T Value;

        public BinaryTree()
        {
            Parent = this;
        }

        public BinaryTree(BinaryTree<T> parent)
        {
            Parent = parent;
        }

        public BinaryTree(BinaryTree<T> parent, T item) : this(parent)
        {
            Value = item;
        }

        public void Add(T item)
        {
            Add(this, item);
        }

        private void Add(BinaryTree<T> current, T item)
        {
            if (current.Value == null)
            {
                current = new BinaryTree<T>(current);
                return;
            }
            
            switch (item.CompareTo(current.Value) < 0)
            {
                case true when current.Left.Value == null:
                    current.Left = new BinaryTree<T>(current, item);
                    break;
                case true when current.Left.Value != null:
                    Add(current.Left, item);
                    break;
            }
            
            
            switch (item.CompareTo(current.Value) > 0)
            {
                case true when current.Right.Value == null:
                    current.Right = new BinaryTree<T>(current, item);
                    break;
                case true when current.Right.Value != null:
                    Add(current.Right, item);
                    break;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var left = Left;
            while (left != null)
                left = left.Left;
            
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
