using System;
using System.Collections.ObjectModel;
using System.IO;

namespace GitVersionControl.Models
{
    public class FileTreeItem
    {
        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public bool IsDirectory { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }
        public string Icon { get; set; } = string.Empty;
        public ObservableCollection<FileTreeItem> Children { get; set; } = new ObservableCollection<FileTreeItem>();
        
        public FileTreeItem()
        {
        }
        
        public FileTreeItem(string name, string fullPath, bool isDirectory)
        {
            Name = name;
            FullPath = fullPath;
            IsDirectory = isDirectory;
            Icon = GetIcon(name, isDirectory);
        }
        
        private string GetIcon(string name, bool isDirectory)
        {
            if (isDirectory)
            {
                // Use Git icon for repository root
                if (name.Equals("Repository", StringComparison.OrdinalIgnoreCase))
                    return "🐙"; // Git octopus icon
                return "📁";
            }
            return GetFileIcon(name);
        }
        
        private string GetFileIcon(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".cs" => "📄",
                ".xaml" => "📄",
                ".xml" => "📄",
                ".json" => "📄",
                ".txt" => "📄",
                ".md" => "📄",
                ".gitignore" => "📄",
                ".exe" => "⚙️",
                ".dll" => "⚙️",
                ".png" => "🖼️",
                ".jpg" => "🖼️",
                ".jpeg" => "🖼️",
                ".gif" => "🖼️",
                ".ico" => "🖼️",
                ".pdf" => "📕",
                ".zip" => "📦",
                ".rar" => "📦",
                ".7z" => "📦",
                _ => "📄"
            };
        }
    }
} 