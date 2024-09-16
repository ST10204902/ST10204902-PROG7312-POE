using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10204902_PROG7312_POE.Models
{
    /// <summary>
    /// Issue class. Represents an issue that can be reported by the user.
    /// </summary>
    public class Issue
    {
        //---------------------------------------------------------
        //Variables
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<MediaAttachment> MediaAttachments { get; private set; }

        //---------------------------------------------------------
        /// <summary>
        /// Parameterized constructor. Initializes the issue with the location, category and description.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="category"></param>
        /// <param name="description"></param>
        public Issue(string location, string category, string description)
        {
            Location = location;
            Category = category;
            Description = description;
            MediaAttachments = new List<MediaAttachment>();
        }

        //---------------------------------------------------------
        /// <summary>
        /// Default constructor. Initializes the media attachments list.
        /// </summary>
        public Issue()
        {
            MediaAttachments = new List<MediaAttachment>();
        }

        //---------------------------------------------------------
        /// <summary>
        /// Add a media attachment to the list of media attachments.
        /// </summary>
        /// <param name="mediaAttachment"></param>
        public void AddMediaAttachment(MediaAttachment mediaAttachment)
        {
            MediaAttachments.Add(mediaAttachment);
        }

        //---------------------------------------------------------
        /// <summary>
        /// Remove a media attachment from the list of media attachments.
        /// </summary>
        /// <param name="mediaAttachment"></param>
        public void RemoveMediaAttachment(MediaAttachment mediaAttachment)
        {
            MediaAttachments.Remove(mediaAttachment);
        }

        //---------------------------------------------------------
        /// <summary>
        /// Get the details of the media attachments.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetMediaAttachmentDetails()
        {
            return MediaAttachments.Select(x => $"File: {x.FileName}, Type: {x.FileType.Name}");
        }
    }
}//---------------------------------EOF------------------------------------//