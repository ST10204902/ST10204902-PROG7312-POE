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
    public interface IIssueRepository
    {
        //Methods
        IEnumerable<Issue> GetAllIssues();
        void AddIssue(Issue issue);
        void RemoveIssue(Issue issue);
    }
}
// ------------------------------EOF------------------------------------