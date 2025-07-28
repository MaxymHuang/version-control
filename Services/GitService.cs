using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using GitVersionControl.Models;

namespace GitVersionControl.Services
{
    public class GitService
    {
        public bool IsValidRepository(string path)
        {
            try
            {
                return Repository.IsValid(path);
            }
            catch
            {
                return false;
            }
        }
        
        public bool InitializeRepository(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                Repository.Init(path);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to initialize repository: {ex.Message}", ex);
            }
        }
        
        public List<CommitInfo> GetCommitHistory(string repositoryPath, int maxCount = 100)
        {
            var commits = new List<CommitInfo>();
            
            try
            {
                using var repo = new Repository(repositoryPath);
                
                var commitFilter = new CommitFilter
                {
                    SortBy = CommitSortStrategies.Time,
                    FirstParentOnly = false
                };
                
                foreach (var commit in repo.Commits.QueryBy(commitFilter).Take(maxCount))
                {
                    commits.Add(new CommitInfo
                    {
                        Id = commit.Id.Sha,
                        ShortId = commit.Id.Sha.Substring(0, 7),
                        Message = commit.MessageShort,
                        Author = commit.Author.Name,
                        Email = commit.Author.Email,
                        Date = commit.Author.When.DateTime
                    });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get commit history: {ex.Message}", ex);
            }
            
            return commits;
        }
        
        public CommitInfo? GetCommitDetails(string repositoryPath, string commitId)
        {
            try
            {
                using var repo = new Repository(repositoryPath);
                var commit = repo.Lookup<Commit>(commitId);
                
                if (commit == null)
                    return null;
                
                return new CommitInfo
                {
                    Id = commit.Id.Sha,
                    ShortId = commit.Id.Sha.Substring(0, 7),
                    Message = commit.Message,
                    Author = commit.Author.Name,
                    Email = commit.Author.Email,
                    Date = commit.Author.When.DateTime
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get commit details: {ex.Message}", ex);
            }
        }
        
        public string GetRepositoryStatus(string repositoryPath)
        {
            try
            {
                using var repo = new Repository(repositoryPath);
                var status = repo.RetrieveStatus();
                
                var statusInfo = $"Branch: {repo.Head.FriendlyName}\n";
                statusInfo += $"Modified files: {status.Modified.Count()}\n";
                statusInfo += $"Added files: {status.Added.Count()}\n";
                statusInfo += $"Untracked files: {status.Untracked.Count()}";
                
                return statusInfo;
            }
            catch (Exception ex)
            {
                return $"Error getting status: {ex.Message}";
            }
        }

        public List<FileSelectionInfo> GetWorkingDirectoryFiles(string repositoryPath)
        {
            var files = new List<FileSelectionInfo>();
            
            try
            {
                using var repo = new Repository(repositoryPath);
                var status = repo.RetrieveStatus();
                
                // Add modified files
                foreach (var entry in status.Modified)
                {
                    files.Add(new FileSelectionInfo
                    {
                        FilePath = entry.FilePath,
                        FileName = Path.GetFileName(entry.FilePath),
                        Status = GitFileStatus.Modified,
                        IsSelected = false,
                        RelativePath = entry.FilePath
                    });
                }
                
                // Add added files
                foreach (var entry in status.Added)
                {
                    files.Add(new FileSelectionInfo
                    {
                        FilePath = entry.FilePath,
                        FileName = Path.GetFileName(entry.FilePath),
                        Status = GitFileStatus.Added,
                        IsSelected = false,
                        RelativePath = entry.FilePath
                    });
                }
                
                // Add untracked files
                foreach (var entry in status.Untracked)
                {
                    files.Add(new FileSelectionInfo
                    {
                        FilePath = entry.FilePath,
                        FileName = Path.GetFileName(entry.FilePath),
                        Status = GitFileStatus.Untracked,
                        IsSelected = false,
                        RelativePath = entry.FilePath
                    });
                }
                
                // Add deleted files
                foreach (var entry in status.Removed)
                {
                    files.Add(new FileSelectionInfo
                    {
                        FilePath = entry.FilePath,
                        FileName = Path.GetFileName(entry.FilePath),
                        Status = GitFileStatus.Deleted,
                        IsSelected = false,
                        RelativePath = entry.FilePath
                    });
                }
                
                // Add renamed files
                foreach (var entry in status.RenamedInIndex)
                {
                    files.Add(new FileSelectionInfo
                    {
                        FilePath = entry.FilePath,
                        FileName = Path.GetFileName(entry.FilePath),
                        Status = GitFileStatus.Renamed,
                        IsSelected = false,
                        RelativePath = entry.FilePath
                    });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get working directory files: {ex.Message}", ex);
            }
            
            return files;
        }

        public bool StageFile(string repositoryPath, string filePath)
        {
            try
            {
                using var repo = new Repository(repositoryPath);
                Commands.Stage(repo, filePath);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to stage file {filePath}: {ex.Message}", ex);
            }
        }

        public bool UnstageFile(string repositoryPath, string filePath)
        {
            try
            {
                using var repo = new Repository(repositoryPath);
                Commands.Unstage(repo, filePath);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to unstage file {filePath}: {ex.Message}", ex);
            }
        }

        public bool CommitChanges(string repositoryPath, string message, string authorName, string authorEmail)
        {
            try
            {
                using var repo = new Repository(repositoryPath);
                
                var signature = new Signature(authorName, authorEmail, DateTimeOffset.Now);
                var commit = repo.Commit(message, signature, signature);
                
                return commit != null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to commit changes: {ex.Message}", ex);
            }
        }

        public bool ResetToCommit(string repositoryPath, string commitId, ResetMode resetMode = ResetMode.Hard)
        {
            try
            {
                using var repo = new Repository(repositoryPath);
                var commit = repo.Lookup<Commit>(commitId);
                
                if (commit == null)
                    throw new InvalidOperationException($"Commit {commitId} not found");
                
                // Perform the reset
                repo.Reset(resetMode, commit);
                
                // If it's a hard reset, also clean up untracked files
                if (resetMode == ResetMode.Hard)
                {
                    // Remove untracked files and directories
                    var status = repo.RetrieveStatus();
                    foreach (var entry in status.Untracked)
                    {
                        var fullPath = Path.Combine(repositoryPath, entry.FilePath);
                        if (File.Exists(fullPath))
                            File.Delete(fullPath);
                        else if (Directory.Exists(fullPath))
                            Directory.Delete(fullPath, true);
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to reset to commit {commitId}: {ex.Message}", ex);
            }
        }

        public bool SoftResetToCommit(string repositoryPath, string commitId)
        {
            return ResetToCommit(repositoryPath, commitId, ResetMode.Soft);
        }

        public bool HardResetToCommit(string repositoryPath, string commitId)
        {
            return ResetToCommit(repositoryPath, commitId, ResetMode.Hard);
        }

        public bool DiscardAllChanges(string repositoryPath)
        {
            try
            {
                using var repo = new Repository(repositoryPath);
                
                // Reset working directory to match HEAD
                repo.Reset(ResetMode.Hard, repo.Head.Tip);
                
                // Remove untracked files
                var status = repo.RetrieveStatus();
                foreach (var entry in status.Untracked)
                {
                    var fullPath = Path.Combine(repositoryPath, entry.FilePath);
                    if (File.Exists(fullPath))
                        File.Delete(fullPath);
                    else if (Directory.Exists(fullPath))
                        Directory.Delete(fullPath, true);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to discard changes: {ex.Message}", ex);
            }
        }

        public List<string> GetRepositoryFiles(string repositoryPath)
        {
            var files = new List<string>();
            
            try
            {
                using var repo = new Repository(repositoryPath);
                var workDir = repo.Info.WorkingDirectory;
                
                // Get all files in the working directory
                var allFiles = Directory.GetFiles(workDir, "*", SearchOption.AllDirectories);
                
                foreach (var file in allFiles)
                {
                    // Skip .git directory
                    if (file.Contains("\\.git\\"))
                        continue;
                    
                    // Get relative path from repository root
                    var relativePath = Path.GetRelativePath(workDir, file);
                    files.Add(relativePath);
                }
                
                return files.OrderBy(f => f).ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get repository files: {ex.Message}", ex);
            }
        }
    }
} 