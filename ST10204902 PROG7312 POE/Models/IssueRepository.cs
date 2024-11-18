using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10204902_PROG7312_POE.Models
{
    /// <summary>
    /// Interface for the Issue Repository
    /// </summary>
    public class IssueRepository : IIssueRepository
    {
        //Variables
        private readonly List<Issue> _issues = new List<Issue>();

        //---------------------------------------------------------
        /// <summary>
        /// Get all issues
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Issue> GetAllIssues()
        {
            return _issues;
        }

        //---------------------------------------------------------
        /// <summary>
        /// Add an issue to the list of issues
        /// </summary>
        /// <param name="issue"></param>
        public void AddIssue(Issue issue)
        {
            _issues.Add(issue);
        }

        //---------------------------------------------------------
        /// <summary>
        /// Remove an issue from the list of issues
        /// </summary>
        /// <param name="issue"></param>
        public void RemoveIssue(Issue issue)
        {
            _issues.Remove(issue);
        }
    }
}
// ------------------------------EOF------------------------------------