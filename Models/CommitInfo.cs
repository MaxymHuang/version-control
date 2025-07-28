using System;

namespace GitVersionControl.Models
{
    public class CommitInfo
    {
        public string Id { get; set; } = string.Empty;
        public string ShortId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string FormattedDate => Date.ToString("yyyy-MM-dd HH:mm:ss");
        public string ShortMessage => Message.Length > 50 ? Message.Substring(0, 47) + "..." : Message;
        
        public override string ToString()
        {
            return $"{ShortId} - {ShortMessage} ({Author}, {FormattedDate})";
        }
    }
} 