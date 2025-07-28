using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using GitVersionControl.Models;
using GitVersionControl.Services;

namespace GitVersionControl
{
    public partial class MainWindow : Window
    {
        private readonly GitService _gitService;
        private string _currentRepositoryPath = string.Empty;
        private List<CommitInfo> _commits = new();
        private List<FileSelectionInfo> _workingDirectoryFiles = new();

        public MainWindow()
        {
            InitializeComponent();
            _gitService = new GitService();
            
            // Set initial state
            UpdateUI();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Repository Directory",
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select Folder",
                Filter = "Folders|*.",
                ValidateNames = false
            };

            // Use a folder browser instead
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select a directory for Git repository",
                ShowNewFolderButton = true
            };

            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var selectedPath = folderDialog.SelectedPath;
                LoadRepository(selectedPath);
            }
        }

        private void InitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentRepositoryPath))
            {
                MessageBox.Show("Please select a directory first.", "No Directory Selected", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (_gitService.IsValidRepository(_currentRepositoryPath))
                {
                    MessageBox.Show("This directory already contains a Git repository.", 
                                   "Repository Exists", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show($"Initialize Git repository in:\n{_currentRepositoryPath}?", 
                                           "Confirm Initialization", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _gitService.InitializeRepository(_currentRepositoryPath);
                    MessageBox.Show("Git repository initialized successfully!", "Success", 
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadRepository(_currentRepositoryPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize repository:\n{ex.Message}", "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentRepositoryPath))
            {
                LoadRepository(_currentRepositoryPath);
            }
        }

        private void LoadRepository(string path)
        {
            try
            {
                _currentRepositoryPath = path;
                RepositoryPathTextBox.Text = path;

                if (_gitService.IsValidRepository(path))
                {
                    // Load commit history
                    _commits = _gitService.GetCommitHistory(path);
                    LoadCommitHistory();
                    
                    // Load working directory files
                    LoadWorkingDirectoryFiles();
                    
                    // Update status
                    StatusTextBlock.Text = _gitService.GetRepositoryStatus(path);
                    
                    RefreshButton.IsEnabled = true;
                }
                else
                {
                    _commits.Clear();
                    CommitHistoryTreeView.Items.Clear();
                    _workingDirectoryFiles.Clear();
                    FilesDataGrid.ItemsSource = null;
                    StatusTextBlock.Text = "Selected directory is not a Git repository. Click 'Initialize Git Repo' to create one.";
                    RefreshButton.IsEnabled = false;
                    ClearCommitDetails();
                }

                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading repository:\n{ex.Message}", "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCommitHistory()
        {
            CommitHistoryTreeView.Items.Clear();
            
            if (_commits.Any())
            {
                foreach (var commit in _commits)
                {
                    var item = new TreeViewItem
                    {
                        Header = commit,
                        Tag = commit
                    };
                    
                    CommitHistoryTreeView.Items.Add(item);
                }
            }
            else
            {
                var item = new TreeViewItem
                {
                    Header = "No commits found",
                    IsEnabled = false
                };
                CommitHistoryTreeView.Items.Add(item);
            }
        }

        private void CommitHistoryTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem item && item.Tag is CommitInfo commit)
            {
                ShowCommitDetails(commit);
            }
            else
            {
                ClearCommitDetails();
            }
        }

        private void ShowCommitDetails(CommitInfo commit)
        {
            try
            {
                // Get full commit details
                var fullCommit = _gitService.GetCommitDetails(_currentRepositoryPath, commit.Id);
                
                if (fullCommit != null)
                {
                    NoSelectionText.Visibility = Visibility.Collapsed;
                    DetailsContent.Visibility = Visibility.Visible;
                    
                    CommitIdTextBox.Text = fullCommit.Id;
                    AuthorTextBox.Text = $"{fullCommit.Author} <{fullCommit.Email}>";
                    DateTextBox.Text = fullCommit.FormattedDate;
                    MessageTextBox.Text = fullCommit.Message;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading commit details:\n{ex.Message}", "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearCommitDetails()
        {
            NoSelectionText.Visibility = Visibility.Visible;
            DetailsContent.Visibility = Visibility.Collapsed;
            
            CommitIdTextBox.Text = string.Empty;
            AuthorTextBox.Text = string.Empty;
            DateTextBox.Text = string.Empty;
            MessageTextBox.Text = string.Empty;
        }

        private void UpdateUI()
        {
            // Enable/disable buttons based on current state
            InitButton.IsEnabled = !string.IsNullOrEmpty(_currentRepositoryPath);
            
            // Enable/disable file selection buttons based on repository state
            bool hasRepository = !string.IsNullOrEmpty(_currentRepositoryPath) && _gitService.IsValidRepository(_currentRepositoryPath);
            RefreshFilesButton.IsEnabled = hasRepository;
            SelectAllButton.IsEnabled = hasRepository && _workingDirectoryFiles.Any();
            DeselectAllButton.IsEnabled = hasRepository && _workingDirectoryFiles.Any();
            StageSelectedButton.IsEnabled = hasRepository && _workingDirectoryFiles.Any(f => f.IsSelected);
            UnstageSelectedButton.IsEnabled = hasRepository && _workingDirectoryFiles.Any(f => f.IsSelected);
            CommitButton.IsEnabled = hasRepository;
        }

        // File Selection Event Handlers
        private void RefreshFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentRepositoryPath))
            {
                MessageBox.Show("Please select a repository first.", "No Repository", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                LoadWorkingDirectoryFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing files:\n{ex.Message}", "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var file in _workingDirectoryFiles)
            {
                file.IsSelected = true;
            }
            FilesDataGrid.Items.Refresh();
            UpdateUI();
        }

        private void DeselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var file in _workingDirectoryFiles)
            {
                file.IsSelected = false;
            }
            FilesDataGrid.Items.Refresh();
            UpdateUI();
        }

        private void StageSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedFiles = _workingDirectoryFiles.Where(f => f.IsSelected).ToList();
            
            if (!selectedFiles.Any())
            {
                MessageBox.Show("Please select files to stage.", "No Files Selected", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                foreach (var file in selectedFiles)
                {
                    _gitService.StageFile(_currentRepositoryPath, file.RelativePath);
                }
                
                MessageBox.Show($"Successfully staged {selectedFiles.Count} file(s).", "Success", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Refresh the file list
                LoadWorkingDirectoryFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error staging files:\n{ex.Message}", "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UnstageSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedFiles = _workingDirectoryFiles.Where(f => f.IsSelected).ToList();
            
            if (!selectedFiles.Any())
            {
                MessageBox.Show("Please select files to unstage.", "No Files Selected", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                foreach (var file in selectedFiles)
                {
                    _gitService.UnstageFile(_currentRepositoryPath, file.RelativePath);
                }
                
                MessageBox.Show($"Successfully unstaged {selectedFiles.Count} file(s).", "Success", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Refresh the file list
                LoadWorkingDirectoryFiles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error unstaging files:\n{ex.Message}", "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CommitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AuthorNameTextBox.Text) || string.IsNullOrEmpty(AuthorEmailTextBox.Text))
            {
                MessageBox.Show("Please enter author name and email.", "Missing Information", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(CommitMessageTextBox.Text))
            {
                MessageBox.Show("Please enter a commit message.", "Missing Commit Message", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var success = _gitService.CommitChanges(
                    _currentRepositoryPath,
                    CommitMessageTextBox.Text,
                    AuthorNameTextBox.Text,
                    AuthorEmailTextBox.Text
                );

                if (success)
                {
                    MessageBox.Show("Changes committed successfully!", "Success", 
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Clear the commit form
                    CommitMessageTextBox.Text = string.Empty;
                    
                    // Refresh both commit history and file list
                    LoadRepository(_currentRepositoryPath);
                    LoadWorkingDirectoryFiles();
                }
                else
                {
                    MessageBox.Show("Failed to commit changes. No changes to commit.", "No Changes", 
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error committing changes:\n{ex.Message}", "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadWorkingDirectoryFiles()
        {
            try
            {
                _workingDirectoryFiles = _gitService.GetWorkingDirectoryFiles(_currentRepositoryPath);
                FilesDataGrid.ItemsSource = _workingDirectoryFiles;
                UpdateUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading working directory files:\n{ex.Message}", "Error", 
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
} 