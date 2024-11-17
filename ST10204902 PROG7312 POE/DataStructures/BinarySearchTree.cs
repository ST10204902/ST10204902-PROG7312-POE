using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10204902_PROG7312_POE.DataStructures
{
    /// <summary>
    /// Represents a binary search tree (BST) data structure.
    /// </summary>
    /// <typeparam name="TKey">The type of the key, which must implement IComparable</typeparam>
    /// <typeparam name="TData">The type of the data stored in the tree</typeparam>
    public class BinarySearchTree<TKey, TData> where TKey : IComparable<TKey>
    {
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Represents a node in the binary search tree.
        /// </summary>
        public class Node
        {
            //--------------------------------------------------------------------------------
            //Variables
            /// <summary>
            /// Gets or sets the key of the node.
            /// </summary>
            public TKey Key { get; set; }

            /// <summary>
            /// Gets or sets the data of the node.
            /// </summary>
            public TData Data { get; set; }

            /// <summary>
            /// Gets or sets the left child of the node.
            /// </summary>
            public Node Left { get; set; }

            /// <summary>
            /// Gets or sets the right child of the node.
            /// </summary>
            public Node Right { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class with the specified key and data.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="data"></param>
            public Node(TKey key, TData data)
            {
                Key = key;
                Data = data;
                Left = null;
                Right = null;
            }
        }

        private Node root;

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Inserts a new node with the specified key and data into the tree.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void Insert(TKey key, TData data)
        {
            root = InsertRecord(root, key, data);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Recursively inserts a new node with the specified key and data into the tree.
        /// </summary>
        /// <param name="root">The root node of the subtree</param>
        /// <param name="key">The key of the new node</param>
        /// <param name="data">The data of the new node</param>
        /// <returns>The root node of the subtree</returns>
        private Node InsertRecord(Node root, TKey key, TData data)
        {
            if (root == null)
            {
                root = new Node(key, data);
                return root;
            }

            if (key.CompareTo(root.Key) < 0)
            {
                root.Left = InsertRecord(root.Left, key, data);
            }
            else if (key.CompareTo(root.Key) > 0)
            {
                root.Right = InsertRecord(root.Right, key, data);
            }
            return root;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Searches for a node with the specified key in the tree.
        /// </summary>
        /// <param name="root">The root node of the subtree</param>
        /// <param name="key">The key of the node to search for</param>
        /// <returns>The node with the specified key, or null if the key is not found.</returns>
        public Node SearchRecords(Node root, TKey key)
        {
            if (root == null || root.Key.CompareTo(key) == 0)
            {
                return root;
            }

            if (root.Key.CompareTo(key) < 0)
            {
                return SearchRecords(root.Right, key);
            }

            return SearchRecords(root.Left, key);
        }
    }
}
//--------------------------EOF-----------------------------------