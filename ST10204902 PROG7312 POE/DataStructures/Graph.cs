using System;
using System.Collections.Generic;
using System.Linq;

namespace ST10204902_PROG7312_POE.DataStructures
{
    /// <summary>
    /// Represents a generic directed graph data structure.
    /// </summary>
    /// <typeparam name="T">The type of vertices in the graph.</typeparam>
    public class Graph<T>
    {
        //------------------------------------------------------------------
        // Fields
        //------------------------------------------------------------------
        /// <summary>
        /// The adjacency list representing the graph structure.
        /// Key: vertex, Value: list of adjacent vertices
        /// </summary>
        protected Dictionary<T, List<T>> AdjList { get; private set; }

        //------------------------------------------------------------------
        // Constructor
        //------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the Graph class.
        /// Time Complexity: O(1)
        /// </summary>
        public Graph()
        {
            AdjList = new Dictionary<T, List<T>>();
        }

        //------------------------------------------------------------------
        // Public Methods
        //------------------------------------------------------------------
        /// <summary>
        /// Adds a vertex to the graph if it doesn't already exist.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="vertex">The vertex to add.</param>
        public void AddVertex(T vertex)
        {
            if (!AdjList.ContainsKey(vertex))
            {
                AdjList[vertex] = new List<T>();
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Adds a directed edge from source to destination vertex.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="destination">The destination vertex.</param>
        public void AddEdge(T source, T destination)
        {
            AddVertex(source);
            AddVertex(destination);
            
            if (!AdjList[source].Contains(destination))
            {
                AdjList[source].Add(destination);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Removes a directed edge from source to destination vertex.
        /// Time Complexity: O(E) where E is the number of edges from source
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="destination">The destination vertex.</param>
        public void RemoveEdge(T source, T destination)
        {
            if (AdjList.ContainsKey(source))
            {
                AdjList[source].Remove(destination);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Checks if the graph contains the specified vertex.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="vertex">The vertex to check.</param>
        /// <returns>True if the vertex exists in the graph; otherwise, false.</returns>
        public bool ContainsVertex(T vertex)
        {
            return AdjList.ContainsKey(vertex);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets all vertices adjacent to the specified vertex.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="vertex">The vertex whose neighbors to get.</param>
        /// <returns>A list of neighboring vertices.</returns>
        public List<T> GetNeighbours(T vertex)
        {
            return AdjList.ContainsKey(vertex) 
                ? new List<T>(AdjList[vertex]) 
                : new List<T>();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets all vertices in the graph.
        /// Time Complexity: O(1)
        /// </summary>
        /// <returns>An enumerable collection of all vertices.</returns>
        public IEnumerable<T> GetAllVertices()
        {
            return AdjList.Keys;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Performs a depth-first search traversal starting from the specified vertex.
        /// Time Complexity: O(V + E) where V is vertices and E is edges
        /// </summary>
        /// <param name="startVertex">The vertex to start the traversal from.</param>
        /// <returns>An enumerable sequence of vertices in DFS order.</returns>
        public IEnumerable<T> DepthFirstSearch(T startVertex)
        {
            if (!ContainsVertex(startVertex))
                yield break;

            var visited = new HashSet<T>();
            var stack = new Stack<T>();
            
            stack.Push(startVertex);
            
            while (stack.Count > 0)
            {
                var vertex = stack.Pop();
                
                if (!visited.Contains(vertex))
                {
                    visited.Add(vertex);
                    yield return vertex;
                    
                    var neighbors = GetNeighbours(vertex).ToList();
                    neighbors.Reverse();
                    
                    foreach (var neighbor in neighbors)
                    {
                        if (!visited.Contains(neighbor))
                        {
                            stack.Push(neighbor);
                        }
                    }
                }
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Performs a breadth-first search traversal starting from the specified vertex.
        /// Time Complexity: O(V + E) where V is vertices and E is edges
        /// </summary>
        /// <param name="startVertex">The vertex to start the traversal from.</param>
        /// <returns>An enumerable sequence of vertices in BFS order.</returns>
        public IEnumerable<T> BreadthFirstSearch(T startVertex)
        {
            if (!ContainsVertex(startVertex))
                yield break;

            var visited = new HashSet<T>();
            var queue = new Queue<T>();
            
            visited.Add(startVertex);
            queue.Enqueue(startVertex);
            
            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                yield return vertex;
                
                foreach (var neighbor in GetNeighbours(vertex))
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Checks if the graph contains any cycles.
        /// Time Complexity: O(V + E) where V is vertices and E is edges
        /// </summary>
        /// <returns>True if the graph contains a cycle; otherwise, false.</returns>
        public bool HasCycle()
        {
            var visited = new HashSet<T>();
            var recursionStack = new HashSet<T>();
            
            foreach (var vertex in GetAllVertices())
            {
                if (HasCycleUtil(vertex, visited, recursionStack))
                    return true;
            }
            
            return false;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Finds a path between source and destination vertices.
        /// Time Complexity: O(V + E) where V is vertices and E is edges
        /// </summary>
        /// <param name="source">The source vertex.</param>
        /// <param name="destination">The destination vertex.</param>
        /// <returns>A list representing the path if found; otherwise, null.</returns>
        public List<T> FindPath(T source, T destination)
        {
            if (!ContainsVertex(source) || !ContainsVertex(destination))
                return null;

            var visited = new HashSet<T>();
            var queue = new Queue<T>();
            var path = new Dictionary<T, T>();

            visited.Add(source);
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.Equals(destination))
                {
                    return ReconstructPath(path, source, destination);
                }

                foreach (var neighbor in GetNeighbours(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                        path[neighbor] = current;
                    }
                }
            }

            return null;
        }

        //------------------------------------------------------------------
        // Private Methods
        //------------------------------------------------------------------
        /// <summary>
        /// Helper method for cycle detection.
        /// Time Complexity: O(V + E) where V is vertices and E is edges
        /// </summary>
        private bool HasCycleUtil(T vertex, HashSet<T> visited, HashSet<T> recursionStack)
        {
            if (!visited.Contains(vertex))
            {
                visited.Add(vertex);
                recursionStack.Add(vertex);
                
                foreach (var neighbor in GetNeighbours(vertex))
                {
                    if (!visited.Contains(neighbor))
                    {
                        if (HasCycleUtil(neighbor, visited, recursionStack))
                            return true;
                    }
                    else if (recursionStack.Contains(neighbor))
                    {
                        return true;
                    }
                }
            }
            
            recursionStack.Remove(vertex);
            return false;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Reconstructs the path from source to destination using the path dictionary.
        /// Time Complexity: O(P) where P is the path length
        /// </summary>
        private List<T> ReconstructPath(Dictionary<T, T> path, T source, T destination)
        {
            var pathList = new List<T>();
            var current = destination;
            pathList.Add(current);

            while (!current.Equals(source))
            {
                current = path[current];
                pathList.Add(current);
            }

            pathList.Reverse();
            return pathList;
        }
    }
}
// ------------------------------EOF------------------------------------