using System;

namespace GitVersionControl.Models
{
    public class FileSelectionInfo
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public GitFileStatus Status { get; set; }
        public bool IsSelected { get; set; }
        public string RelativePath { get; set; } = string.Empty;
    }

    public enum GitFileStatus
    {
        Untracked,
        Modified,
        Added,
        Deleted,
        Renamed,
        Conflicted
    }
} 