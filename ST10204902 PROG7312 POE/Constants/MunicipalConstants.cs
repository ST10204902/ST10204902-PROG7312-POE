using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10204902_PROG7312_POE.Constants
{
    //------------------------------------------------------------------
    /// <summary>
    /// Constants for the municipal system.
    /// </summary>  
    public static class MunicipalConstants
    {
        public static readonly string[] Categories = {
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

        public static readonly string[] Statuses = {
        "Pending",
        "In Progress",
        "Resolved"
    };

        public static readonly string[] PriorityLevels = {
        "High",
        "Medium",
        "Low"
    };

        public static readonly string[] CommonIssues = {
        "Water leak reported in {0}",
        "Pothole repair needed on main road in {0}",
        "Street light malfunction in {0}",
        "Waste collection delay in {0}",
        "Park maintenance required in {0}",
        "Building inspection request for {0}",
        "Traffic light failure at {0}",
        "Public safety concern reported in {0}",
        "Blocked storm drain in {0}",
        "Illegal dumping reported in {0}",
        "Fallen tree removal needed in {0}",
        "Grass cutting request for {0}",
        "Noise complaint investigation in {0}",
        "Building permit application for {0}",
        "Street sign replacement in {0}"
    };

        public static readonly string[] ResolutionMessages = {
        "Issue resolved successfully",
        "Maintenance completed as requested",
        "Service restored to normal operation",
        "Repairs completed and tested",
        "Area cleaned and restored",
        "Inspection completed and approved",
        "Safety measures implemented"
    };

        public static readonly string DefaultResolutionMessage = "Issue resolved successfully";

        public static readonly string[] EmailDomains = {
            "drakensteinmunicipality.co.za",
            "cityofcapetown.gov.za",
            "gmail.com",
            "outlook.com"
        };

        
    }
}
// ------------------------------EOF------------------------------------
