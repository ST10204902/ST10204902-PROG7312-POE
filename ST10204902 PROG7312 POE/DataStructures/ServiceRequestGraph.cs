using ST10204902_PROG7312_POE.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ST10204902_PROG7312_POE.DataStructures
{
    /// <summary>
    /// Represents a graph specifically for ServiceRequest objects.
    /// Inherits from the generic Graph class.
    /// </summary>
    public class ServiceRequestGraph : Graph<ServiceRequest>
    {
        //------------------------------------------------------------------
        // Constructor
        //------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ServiceRequestGraph class.
        /// </summary>
        public ServiceRequestGraph() : base()
        {
        }

        //------------------------------------------------------------------
        // Public Methods - Dependency Management
        //------------------------------------------------------------------
        /// <summary>
        /// Adds a dependency between two service requests.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="dependent">The dependent service request.</param>
        /// <param name="dependency">The service request it depends on.</param>
        public void AddDependency(ServiceRequest dependent, ServiceRequest dependency)
        {
            AddVertex(dependent);
            AddVertex(dependency);
            AddEdge(dependent, dependency);

            // Update the Dependencies list in the ServiceRequest object
            if (dependent.Dependencies == null)
            {
                dependent.Dependencies = new List<ServiceRequest>();
            }

            if (!dependent.Dependencies.Contains(dependency))
            {
                dependent.Dependencies.Add(dependency);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Removes a dependency between two service requests.
        /// Time Complexity: O(E) where E is the number of edges
        /// </summary>
        /// <param name="dependent">The dependent service request.</param>
        /// <param name="dependency">The dependency to remove.</param>
        public void RemoveDependency(ServiceRequest dependent, ServiceRequest dependency)
        {
            RemoveEdge(dependent, dependency);
            if (dependent.Dependencies != null)
            {
                dependent.Dependencies.Remove(dependency);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Clears all dependencies for a service request.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="request">The service request to clear dependencies for.</param>
        public void ClearDependencies(ServiceRequest request)
        {
            if (AdjList.ContainsKey(request))
            {
                AdjList[request].Clear();
                request.Dependencies?.Clear();
            }
        }

        //------------------------------------------------------------------
        // Public Methods - Graph Operations
        //------------------------------------------------------------------
        /// <summary>
        /// Checks if the graph contains the specified service request.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="request">The service request to check.</param>
        /// <returns>True if the service request is in the graph; otherwise, false.</returns>
        public bool ContainsServiceRequest(ServiceRequest request)
        {
            return ContainsVertex(request);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets the dependencies of the specified service request.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="request">The service request to get dependencies for.</param>
        /// <returns>A list of dependencies.</returns>
        public List<ServiceRequest> GetDependencies(ServiceRequest request)
        {
            return GetNeighbours(request);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets all service requests in the graph.
        /// Time Complexity: O(1)
        /// </summary>
        /// <returns>An enumerable collection of all service requests.</returns>
        public IEnumerable<ServiceRequest> GetAllServiceRequests()
        {
            return GetAllVertices();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Adds a new service request to the graph.
        /// Time Complexity: O(1)
        /// </summary>
        /// <param name="request">The service request to add.</param>
        public void AddServiceRequest(ServiceRequest request)
        {
            AddVertex(request);
        }

        //------------------------------------------------------------------
        // Public Methods - Traversal and Analysis
        //------------------------------------------------------------------
        /// <summary>
        /// Gets the dependency order using depth-first search.
        /// Time Complexity: O(V + E) where V is vertices and E is edges
        /// </summary>
        /// <param name="startRequest">The starting service request.</param>
        /// <returns>An enumerable of service requests in dependency order.</returns>
        public IEnumerable<ServiceRequest> GetDependencyOrder(ServiceRequest startRequest)
        {
            return DepthFirstSearch(startRequest);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets the immediate dependencies using breadth-first search.
        /// Time Complexity: O(V + E) where V is vertices and E is edges
        /// </summary>
        /// <param name="startRequest">The starting service request.</param>
        /// <returns>An enumerable of immediate dependencies.</returns>
        public IEnumerable<ServiceRequest> GetImmediateDependencies(ServiceRequest startRequest)
        {
            return BreadthFirstSearch(startRequest).Skip(1).Take(1);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Checks if the graph has any circular dependencies.
        /// Time Complexity: O(V + E) where V is vertices and E is edges
        /// </summary>
        /// <returns>True if circular dependencies exist; otherwise, false.</returns>
        public bool HasCircularDependency()
        {
            return HasCycle();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Finds the dependency path between two service requests.
        /// Time Complexity: O(V + E) where V is vertices and E is edges
        /// </summary>
        /// <param name="source">The source service request.</param>
        /// <param name="target">The target service request.</param>
        /// <returns>A list representing the dependency path.</returns>
        public List<ServiceRequest> FindDependencyPath(ServiceRequest source, ServiceRequest target)
        {
            return FindPath(source, target);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets all related service requests using breadth-first search.
        /// Time Complexity: O(V + E) where V is vertices and E is edges
        /// </summary>
        /// <param name="request">The service request to find relations for.</param>
        /// <returns>An enumerable of related service requests.</returns>
        public IEnumerable<ServiceRequest> GetRelatedRequests(ServiceRequest request)
        {
            return BreadthFirstSearch(request);
        }
    }
}
// ------------------------------EOF------------------------------------