using ST10204902_PROG7312_POE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10204902_PROG7312_POE.Models
{
    //------------------------------------------------------------------
    /// <summary>
    /// Represents a service request.
    /// </summary>
    public class ServiceRequest : INotifyPropertyChanged
    {
        //Variables
        private string _status;
        private DateTime? _dateResolved;
        private string _resolutionComment;
        private List<MediaAttachment> _attachments;

        //Events
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public string Description { get; set; }
        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        public DateTime DateSubmitted { get; set; }
        public int Priority { get; set; }
        public string Location { get; set; }
        public string RequesterName { get; set; }
        public string ContactInfo { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? DateResolved
        {
            get => _dateResolved;
            set
            {
                if (_dateResolved != value)
                {
                    _dateResolved = value;
                    OnPropertyChanged(nameof(DateResolved));
                }
            }
        }
        public string ResolutionComment
        {
            get => _resolutionComment;
            set
            {
                if (_resolutionComment != value)
                {
                    _resolutionComment = value;
                    OnPropertyChanged(nameof(ResolutionComment));
                }
            }
        }
        public string Category { get; set; }
        public List<MediaAttachment> Attachments
    {
        get => _attachments ?? (_attachments = new List<MediaAttachment>());
        set
            {
                if (_attachments != value)
                {
                    _attachments = new List<MediaAttachment>(value ?? new List<MediaAttachment>());
                    OnPropertyChanged(nameof(Attachments));
                }
            }
        }
        public List<Tuple<DateTime, string>> StatusHistory { get; set; }
        public List<ServiceRequest> Dependencies { get; set; } = new List<ServiceRequest>();


        //---------------------------------------------------------
        /// <summary>
        /// Property changed event handler
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
// ------------------------------EOF------------------------------------
