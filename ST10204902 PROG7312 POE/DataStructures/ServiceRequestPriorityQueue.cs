using ST10204902_PROG7312_POE.Models;
using System;
using System.Collections.Generic;

namespace ST10204902_PROG7312_POE.DataStructures
{
    /// <summary>
    /// Represents a priority queue implemented as a min heap specifically for ServiceRequest objects.
    /// The priority is determined by the Priority property of ServiceRequest, where lower numbers indicate higher priority.
    /// </summary>
    public class ServiceRequestPriorityQueue
    {
        //------------------------------------------------------------------
        // Fields
        private List<ServiceRequest> _heap;

        //------------------------------------------------------------------
        /// <summary>
        /// Gets the number of items in the priority queue.
        /// </summary>
        public int Count => _heap.Count;

        //------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ServiceRequestPriorityQueue.
        /// </summary>
        public ServiceRequestPriorityQueue()
        {
            _heap = new List<ServiceRequest>();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Adds a service request to the priority queue.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="request">The service request to add.</param>
        public void Enqueue(ServiceRequest request)
        {
            _heap.Add(request);
            HeapifyUp(_heap.Count - 1);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Extracts and returns the highest priority service request.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <returns>The highest priority service request.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public ServiceRequest ExtractMin()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Priority queue is empty");

            // Store the minimum element
            ServiceRequest min = _heap[0];

            // If this is the last element
            if (_heap.Count == 1)
            {
                _heap.RemoveAt(0);
                return min;
            }

            // Move last element to root and remove last element
            _heap[0] = _heap[_heap.Count - 1];
            _heap.RemoveAt(_heap.Count - 1);

            // Restore heap property
            HeapifyDown(0);

            return min;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets the top N priority requests without modifying the heap.
        /// Time Complexity: O(n log n)
        /// </summary>
        /// <param name="count">Number of requests to retrieve.</param>
        /// <returns>List of top N priority requests.</returns>
        public List<ServiceRequest> GetTopN(int count)
        {
            if (count <= 0)
                return new List<ServiceRequest>();

            // Create a copy of the heap to avoid modifying the original
            var tempHeap = new List<ServiceRequest>(_heap);
            var result = new List<ServiceRequest>();

            // Extract minimum elements up to count or heap size
            for (int i = 0; i < count && tempHeap.Count > 0; i++)
            {
                // Get the minimum element
                var min = tempHeap[0];
                result.Add(min);

                // Replace root with last element
                tempHeap[0] = tempHeap[tempHeap.Count - 1];
                tempHeap.RemoveAt(tempHeap.Count - 1);

                // Restore heap property if there are elements remaining
                if (tempHeap.Count > 0)
                {
                    // Create temporary queue for heapification
                    var tempQueue = new ServiceRequestPriorityQueue();
                    tempQueue._heap = tempHeap;
                    tempQueue.HeapifyDown(0);
                    tempHeap = tempQueue._heap;
                }
            }

            return result;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Peeks at the highest priority element without removing it.
        /// Time Complexity: O(1)
        /// </summary>
        /// <returns>The highest priority service request.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
        public ServiceRequest Peek()
        {
            if (_heap.Count == 0)
                throw new InvalidOperationException("Priority queue is empty");

            return _heap[0];
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Removes a specific service request from the priority queue.
        /// Time Complexity: O(n log n)
        /// </summary>
        /// <param name="request">The service request to remove.</param>
        public void Remove(ServiceRequest request)
        {
            int index = _heap.FindIndex(r => r.Id == request.Id);
            if (index == -1) return;

            // Move the item to remove to the end
            SwapElements(index, _heap.Count - 1);
            _heap.RemoveAt(_heap.Count - 1);

            // Rebuild heap structure
            Heapify();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Updates the priority of a service request and rebalances the heap.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="request">The service request to update.</param>
        /// <param name="newPriority">The new priority value.</param>
        /// <exception cref="ArgumentNullException">Thrown when request is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when request is not found in queue.</exception>
        public void UpdatePriority(ServiceRequest request, int newPriority)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Find the index of the request
            int index = _heap.FindIndex(r => r.Id == request.Id);
            if (index == -1)
                throw new InvalidOperationException("Request not found in priority queue");

            int oldPriority = _heap[index].Priority;
            _heap[index].Priority = newPriority;

            // If new priority is higher (smaller number), bubble up
            if (newPriority < oldPriority)
            {
                HeapifyUp(index);
            }
            // If new priority is lower (larger number), bubble down
            else if (newPriority > oldPriority)
            {
                HeapifyDown(index);
            }
        }

        //------------------------------------------------------------------
        // Private Methods
        //------------------------------------------------------------------
        
        /// <summary>
        /// Rebuilds the entire heap structure.
        /// Time Complexity: O(n)
        /// </summary>
        private void Heapify()
        {
            for (int i = (_heap.Count / 2) - 1; i >= 0; i--)
            {
                HeapifyDown(i);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Maintains the min-heap property by moving an element up the heap.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="index">The index of the element to move up.</param>
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                
                if (_heap[parentIndex].Priority < _heap[index].Priority ||
                    (_heap[parentIndex].Priority == _heap[index].Priority && 
                     _heap[parentIndex].DateSubmitted <= _heap[index].DateSubmitted))
                {
                    break;
                }

                SwapElements(index, parentIndex);
                index = parentIndex;
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Maintains the min-heap property by moving an element down the heap.
        /// Time Complexity: O(log n)
        /// </summary>
        /// <param name="index">The index of the element to move down.</param>
        private void HeapifyDown(int index)
        {
            while (true)
            {
                int smallest = index;
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;

                // Compare left child
                if (leftChild < _heap.Count)
                {
                    if (_heap[leftChild].Priority < _heap[smallest].Priority ||
                        (_heap[leftChild].Priority == _heap[smallest].Priority && 
                         _heap[leftChild].DateSubmitted < _heap[smallest].DateSubmitted))
                    {
                        smallest = leftChild;
                    }
                }

                // Compare right child
                if (rightChild < _heap.Count)
                {
                    if (_heap[rightChild].Priority < _heap[smallest].Priority ||
                        (_heap[rightChild].Priority == _heap[smallest].Priority && 
                         _heap[rightChild].DateSubmitted < _heap[smallest].DateSubmitted))
                    {
                        smallest = rightChild;
                    }
                }

                if (smallest == index)
                    break;

                SwapElements(index, smallest);
                index = smallest;
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Swaps two elements in the heap.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="i">The index of the first element.</param>
        /// <param name="j">The index of the second element.</param>
        private void SwapElements(int i, int j)
        {
            ServiceRequest temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }
    }
}

//-----------------------EOF------------------------------------------------------------------