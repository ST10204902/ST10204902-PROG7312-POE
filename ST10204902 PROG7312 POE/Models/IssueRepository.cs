using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10204902_PROG7312_POE.Models
{
    public class IssueRepository : IIssueRepository
    {
        private readonly List<Issue> _issues = new List<Issue>();

        public IEnumerable<Issue> GetAllIssues()
        {
            return _issues;
        }

        public void AddIssue(Issue issue)
        {
            _issues.Add(issue);
        }

        public void RemoveIssue(Issue issue)
        {
            _issues.Remove(issue);
        }
    }
}
