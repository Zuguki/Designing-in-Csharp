using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
    public static class Traversal
    {
        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            if (root == null)
                yield break;

            foreach (var product in root.Categories.SelectMany(GetProducts))
                yield return product;

            foreach (var product in root.Products)
                yield return product;
        }

        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            if (root == null)
                yield break;

            if (root.Subjobs.Count == 0)
                yield return root;
            else
                foreach (var endJob in root.Subjobs.SelectMany(GetEndJobs))
                    yield return endJob;
        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            if (root == null)
                yield break;

            if (root.Left == null || root.Right == null)
                yield return root.Value;

            foreach (var left in GetBinaryTreeValues(root.Left))
                yield return left;
            foreach (var right in GetBinaryTreeValues(root.Right))
                yield return right;
        }
    }
}
