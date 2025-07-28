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
    }
} 