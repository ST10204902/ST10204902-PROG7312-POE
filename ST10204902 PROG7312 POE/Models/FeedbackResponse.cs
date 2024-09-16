using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10204902_PROG7312_POE.Models
{
    public class FeedbackResponse
    {
        //---------------------------------------------------------
        //Variables
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }

        //---------------------------------------------------------
        /// <summary>
        /// Default constructor
        /// </summary>
        public FeedbackResponse()
        {
            Date = DateTime.Now;
        }

        //---------------------------------------------------------
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="content"></param>
        /// <param name="rating"></param>
        public FeedbackResponse (string content, int rating)
        {
            Content = content;
            Rating = rating;
            Date = DateTime.Now;
        }

        //---------------------------------------------------------
        /// <summary>
        /// Custom ToString method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Content: {Content}, Rating: {Rating}, Date: {Date}";
        }
    }
}
