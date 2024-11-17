using ST10204902_PROG7312_POE.Models;
using System;
using System.Collections.Generic;
using ST10204902_PROG7312_POE.Constants;

namespace ST10204902_PROG7312_POE.Helpers
{
    //------------------------------------------------------------------
    /// <summary>
    /// Generates sample service requests for testing purposes.
    /// </summary>
    public static class ServiceRequestDataGenerator
    {
        //------------------------------------------------------------------
        // Fields
        //------------------------------------------------------------------
        private static readonly string[] Locations = { 
            "Drakenstein", 
            "Cape Town", 
            "Bellville", 
            "Kuilsriver", 
            "Brakpan", 
            "Durbanville", 
            "Rondebosch", 
            "Sea Point", 
            "Constantia" 
        };

        private static readonly string[] Teams = { 
            "Water & Sanitation", 
            "Roads & Transport", 
            "Electricity", 
            "Waste Management", 
            "Parks & Recreation", 
            "Building & Planning", 
            "Traffic Services", 
            "Public Safety",
            "Community Services" 
        };

        //------------------------------------------------------------------
        // Methods
        //------------------------------------------------------------------

        //------------------------------------------------------------------
        /// <summary>
        /// Generates a list of sample service requests.
        /// </summary>
        /// <param name="count">The number of requests to generate.</param>
        /// <returns>A list of generated service requests.</returns>
        public static List<ServiceRequest> GenerateSampleRequests(int count = 10)
        {
            var requests = new List<ServiceRequest>();
            var random = new Random();

            for (int i = 1; i <= count; i++)
            {
                var daysAgo = random.Next(1, 30);
                var priority = random.Next(1, 4);
                var status = GetRandomStatus(random);

                var request = new ServiceRequest
                {
                    Id = i,
                    Description = GenerateDescription(random),
                    Status = status,
                    DateSubmitted = DateTime.Now.AddDays(-daysAgo),
                    Priority = priority,
                    Location = Locations[random.Next(Locations.Length)],
                    RequesterName = GenerateRequesterName(random),
                    ContactInfo = GenerateEmail(random),
                    AssignedTo = Teams[random.Next(Teams.Length)],
                    Category = MunicipalConstants.Categories[random.Next(MunicipalConstants.Categories.Length)],
                    Attachments = new List<MediaAttachment>(),
                    StatusHistory = GenerateStatusHistory(status, daysAgo),
                    Dependencies = new List<ServiceRequest>()
                };

                if (status == "Completed")
                {
                    request.DateResolved = DateTime.Now.AddDays(-random.Next(1, daysAgo));
                    request.ResolutionComment = MunicipalConstants.DefaultResolutionMessage;
                }

                requests.Add(request);
            }

            // Add some random dependencies
            for (int i = 0; i < count / 2; i++)
            {
                var dependent = requests[random.Next(requests.Count)];
                var dependency = requests[random.Next(requests.Count)];

                if (dependent != dependency && !dependent.Dependencies.Contains(dependency))
                {
                    dependent.Dependencies.Add(dependency);
                }
            }

            return requests;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Generates a description for a service request.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <returns>A generated description.</returns>
        private static string GenerateDescription(Random random)
        {
            var issueIndex = random.Next(MunicipalConstants.CommonIssues.Length);
            var locationIndex = random.Next(Locations.Length);
            return string.Format(MunicipalConstants.CommonIssues[issueIndex], Locations[locationIndex]);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Generates a requester name.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <returns>A generated requester name.</returns>
        private static string GenerateRequesterName(Random random)
        {
            var firstNames = new[] { "John", "Jane", "Michael", "Sarah", "David", "Emma", "James", "Lisa", "Robert", "Maria" };
            var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez" };

            return $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Generates an email address for a requester.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <returns>A generated email address.</returns>
        private static string GenerateEmail(Random random)
        {
            var name = GenerateRequesterName(random).ToLower().Replace(" ", ".");
            var domain = MunicipalConstants.EmailDomains[random.Next(MunicipalConstants.EmailDomains.Length)];
            return $"{name}@{domain}";
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets a random status for a service request.
        /// </summary>
        /// <param name="random">The random number generator.</param>
        /// <returns>A random status.</returns>
        private static string GetRandomStatus(Random random)
        {
            return MunicipalConstants.Statuses[random.Next(MunicipalConstants.Statuses.Length)];
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Generates a status history for a service request.
        /// </summary>
        /// <param name="currentStatus">The current status.</param>
        /// <param name="daysAgo">The number of days ago.</param>
        /// <returns>A list of status history tuples.</returns>
        private static List<Tuple<DateTime, string>> GenerateStatusHistory(string currentStatus, int daysAgo)
        {
            var history = new List<Tuple<DateTime, string>>
            {
                Tuple.Create(DateTime.Now.AddDays(-daysAgo), "Submitted")
            };

            if (currentStatus == "In Progress" || currentStatus == "Resolved")
            {
                history.Add(Tuple.Create(DateTime.Now.AddDays(-daysAgo + 1), "In Progress"));
            }

            if (currentStatus == "Resolved")
            {
                history.Add(Tuple.Create(DateTime.Now.AddDays(-1), "Resolved"));
            }

            return history;
        }
    }
}
// ------------------------------EOF------------------------------------