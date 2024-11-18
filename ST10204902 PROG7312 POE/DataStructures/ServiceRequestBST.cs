using ST10204902_PROG7312_POE.Models;
using System;
using System.Collections.Generic;

namespace ST10204902_PROG7312_POE.DataStructures
{
    /// <summary>
    /// Represents a self-balancing AVL tree specifically for ServiceRequest objects.
    /// Uses the ServiceRequest ID as the key for ordering.
    /// </summary>
    public class ServiceRequestBST
    {
        //------------------------------------------------------------------
        // Inner Class
        //------------------------------------------------------------------
        /// <summary>
        /// Represents a node in the AVL tree.
        /// </summary>
        private class Node
        {
            public ServiceRequest Data { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public int Height { get; set; }

            public Node(ServiceRequest data)
            {
                Data = data;
                Height = 1;
                Left = Right = null;
            }
        }

        //------------------------------------------------------------------
        // Fields
        //------------------------------------------------------------------
        private Node _root;

        //------------------------------------------------------------------
        // Public Methods
        //------------------------------------------------------------------
        /// <summary>
        /// Inserts a new service request into the BST while maintaining AVL balance.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="request">The service request to insert.</param>
        public void Insert(ServiceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            _root = InsertRec(_root, request);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Performs an in-order traversal of the BST.
        /// Time Complexity: O(n)
        /// </summary>
        /// <returns>A list of service requests in ascending order by ID.</returns>
        public List<ServiceRequest> InOrderTraversal()
        {
            var result = new List<ServiceRequest>();
            InOrderTraversalRec(_root, result);
            return result;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Performs a pre-order traversal of the BST.
        /// Time Complexity: O(n)
        /// </summary>
        /// <returns>A list of service requests in pre-order.</returns>
        public List<ServiceRequest> PreOrderTraversal()
        {
            var result = new List<ServiceRequest>();
            PreOrderTraversalRec(_root, result);
            return result;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Performs a post-order traversal of the BST.
        /// Time Complexity: O(n)
        /// </summary>
        /// <returns>A list of service requests in post-order.</returns>
        public List<ServiceRequest> PostOrderTraversal()
        {
            var result = new List<ServiceRequest>();
            PostOrderTraversalRec(_root, result);
            return result;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Searches for a service request by ID.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="id">The ID to search for.</param>
        /// <returns>The service request if found; otherwise, null.</returns>
        public ServiceRequest Search(int id)
        {
            return SearchRec(_root, id)?.Data;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Searches for service requests by date range.
        /// Time Complexity: O(n)
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A list of service requests within the date range.</returns>
        public List<ServiceRequest> SearchByDateRange(DateTime startDate, DateTime endDate)
        {
            var result = new List<ServiceRequest>();
            SearchByDateRangeRec(_root, startDate, endDate, result);
            return result;
        }

        //------------------------------------------------------------------
        // Private Helper Methods
        //------------------------------------------------------------------

        /// <summary>
        /// Recursive helper method to insert a new service request into the BST.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private Node InsertRec(Node node, ServiceRequest request)
        {
            if (node == null)
                return new Node(request);

            if (request.Id < node.Data.Id)
                node.Left = InsertRec(node.Left, request);
            else if (request.Id > node.Data.Id)
                node.Right = InsertRec(node.Right, request);
            else
                return node; // Duplicate IDs not allowed

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            // Left Left Case
            if (balance > 1 && request.Id < node.Left.Data.Id)
                return RightRotate(node);

            // Right Right Case
            if (balance < -1 && request.Id > node.Right.Data.Id)
                return LeftRotate(node);

            // Left Right Case
            if (balance > 1 && request.Id > node.Left.Data.Id)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            // Right Left Case
            if (balance < -1 && request.Id < node.Right.Data.Id)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Recursive helper method to perform an in-order traversal of the BST.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="result"></param>
        private void InOrderTraversalRec(Node node, List<ServiceRequest> result)
        {
            if (node != null)
            {
                InOrderTraversalRec(node.Left, result);
                result.Add(node.Data);
                InOrderTraversalRec(node.Right, result);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Recursive helper method to perform a pre-order traversal of the BST.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="result"></param>
        private void PreOrderTraversalRec(Node node, List<ServiceRequest> result)
        {
            if (node != null)
            {
                result.Add(node.Data);
                PreOrderTraversalRec(node.Left, result);
                PreOrderTraversalRec(node.Right, result);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Recursive helper method to perform a post-order traversal of the BST.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="result"></param>
        private void PostOrderTraversalRec(Node node, List<ServiceRequest> result)
        {
            if (node != null)
            {
                PostOrderTraversalRec(node.Left, result);
                PostOrderTraversalRec(node.Right, result);
                result.Add(node.Data);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Searches for a service request by ID.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private Node SearchRec(Node node, int id)
        {
            if (node == null || node.Data.Id == id)
                return node;

            if (id < node.Data.Id)
                return SearchRec(node.Left, id);

            return SearchRec(node.Right, id);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Recursive helper method to search for service requests by date range.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="result"></param>
        private void SearchByDateRangeRec(Node node, DateTime startDate, DateTime endDate, List<ServiceRequest> result)
        {
            if (node != null)
            {
                SearchByDateRangeRec(node.Left, startDate, endDate, result);
                
                if (node.Data.DateSubmitted >= startDate && node.Data.DateSubmitted <= endDate)
                    result.Add(node.Data);
                
                SearchByDateRangeRec(node.Right, startDate, endDate, result);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Performs a right rotation on the given node.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private Node RightRotate(Node y)
        {
            Node x = y.Left;
            Node T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Performs a left rotation on the given node.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private Node LeftRotate(Node x)
        {
            Node y = x.Right;
            Node T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Helper method to get the height of a node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int GetHeight(Node node)
        {
            return node?.Height ?? 0;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Helper method to get the balance factor of a node.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private int GetBalance(Node node)
        {
            if (node == null)
                return 0;
            return GetHeight(node.Left) - GetHeight(node.Right);
        }
    }
}
// ------------------------------EOF------------------------------------