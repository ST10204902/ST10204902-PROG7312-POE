using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10204902_PROG7312_POE.Models
{
    public class Issue
    {
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<MediaAttachment> MediaAttachments { get; private set; }

        public Issue(string location, string category, string description)
        {
            Location = location;
            Category = category;
            Description = description;
            MediaAttachments = new List<MediaAttachment>();
        }

        public Issue()
        {
            MediaAttachments = new List<MediaAttachment>();
        }

        public void AddMediaAttachment(MediaAttachment mediaAttachment)
        {
            MediaAttachments.Add(mediaAttachment);
        }

        public void RemoveMediaAttachment(MediaAttachment mediaAttachment)
        {
            MediaAttachments.Remove(mediaAttachment);
        }

        public IEnumerable<string> GetMediaAttachmentDetails()
        {
            return MediaAttachments.Select(x => $"File: {x.FileName}, Type: {x.FileType.Name}");
        }
    }
}