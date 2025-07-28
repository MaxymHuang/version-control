# Git Version Control GUI

A simple, user-friendly GUI tool for Git version control, designed for people who are not familiar with Git command-line operations.

## Features

- **Easy Repository Management**: Browse and select directories to work with Git repositories
- **Repository Initialization**: Initialize new Git repositories with a single click
- **Visual Commit History**: View commit history in an intuitive tree view
- **Commit Details**: Click on any commit to see detailed information including:
  - Full commit ID
  - Author and email
  - Commit date and time
  - Complete commit message
- **Repository Status**: See current repository status including branch name and file counts
- **File Selection and Staging**: Select which files to include in commits with a user-friendly interface
- **Selective Committing**: Choose specific files to stage and commit, giving you full control over what gets included
- **File Status Tracking**: View the status of all files (modified, added, untracked, deleted, renamed)
- **Batch Operations**: Select all, deselect all, stage selected, and unstage selected files with ease
- **Repository Reset**: Hard reset to any previous commit with confirmation dialog (actually modifies files)
- **Discard All Changes**: Safely discard all uncommitted changes with confirmation
- **File Explorer**: Tree-like file browser similar to VS Code's explorer panel
- **Professional Branding**: Git icon integration for authentic Git experience

## Technical Stack

- **Frontend**: .NET 9 WPF (Windows Presentation Foundation)
- **Git Backend**: LibGit2Sharp v0.31.0 (C# wrapper for libgit2, which is written in C)
- **Target Platform**: Windows (.NET 9.0-windows)

## Prerequisites

- .NET 9.0 SDK or later
- Windows operating system
- Git (optional, as the application uses LibGit2Sharp internally)

## Building the Application

1. **Clone or download** this project to your local machine

2. **Open a command prompt** in the project directory

3. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

4. **Build the application**:
   ```bash
   dotnet build
   ```

5. **Run the application**:
   ```bash
   dotnet run
   ```

Alternatively, you can build a standalone executable:
```bash
dotnet publish -c Release -r win-x64 --self-contained
```

## How to Use

### 1. Starting the Application
- Launch the Git Version Control GUI
- You'll see a clean interface with repository selection options at the top

### 2. Working with Repositories

#### Option A: Open an Existing Repository
1. Click the **"Browse..."** button
2. Navigate to a directory that contains a Git repository (has a `.git` folder)
3. Select the directory
4. The application will automatically load the commit history and repository status

#### Option B: Initialize a New Repository
1. Click the **"Browse..."** button
2. Navigate to or create a new directory where you want to initialize a Git repository
3. Select the directory
4. Click the **"Initialize Git Repo"** button
5. Confirm the initialization when prompted

### 3. Viewing Commit History
- Once a repository is loaded, you'll see the commit history in the left panel
- Each commit shows:
  - Short commit ID (first 7 characters)
  - Shortened commit message
  - Author name
  - Commit date and time

### 4. Viewing Commit Details
- Click on any commit in the history list
- The right panel will display detailed information:
  - Full commit ID
  - Author name and email
  - Complete commit date and time
  - Full commit message

### 5. Refreshing Data
- Use the **"Refresh"** button to reload the repository data after external changes

### 6. Repository Explorer (New Feature)
- Switch to the **"Repository Explorer"** tab for a comprehensive view
- **File Explorer**: Tree-like file browser on the left showing all repository files
- **Commit History**: Middle panel showing all commits with details
- **Commit Details**: Right panel showing full commit information
- **Reset to Commit**: Click "HARD Reset to This Commit" button to reset to any previous commit
  - Shows confirmation dialog with commit details
  - Actually modifies files to match the selected commit
  - Removes all uncommitted changes and untracked files
  - This is a true hard reset that changes the working directory

### 7. File Selection and Committing
- Switch to the **"File Selection"** tab to manage files for commits
- **Refresh Files**: Click to reload the list of files in the working directory
- **Select All/Deselect All**: Quickly select or deselect all files
- **Stage Selected**: Add selected files to the staging area
- **Unstage Selected**: Remove selected files from the staging area
- **Discard All Changes**: Safely discard all uncommitted changes with confirmation
- **Commit Changes**: Create a new commit with staged files
  - Enter your name and email for the commit author
  - Write a descriptive commit message
  - Click "Commit Changes" to create the commit

#### File Status Types:
- **Modified**: Files that have been changed since the last commit
- **Added**: New files that have been staged
- **Untracked**: New files that haven't been staged yet
- **Deleted**: Files that have been removed
- **Renamed**: Files that have been renamed

## Project Structure

```
GitVersionControl/
├── Models/
│   ├── CommitInfo.cs          # Data model for commit information
│   ├── FileSelectionInfo.cs   # Data model for file selection and status
│   └── FileTreeItem.cs        # Data model for file tree structure
├── Services/
│   └── GitService.cs          # Git operations and repository management
├── App.xaml                   # WPF application definition
├── App.xaml.cs               # Application code-behind
├── MainWindow.xaml           # Main window UI layout
├── MainWindow.xaml.cs        # Main window logic and event handlers
├── GitVersionControl.csproj  # Project file with dependencies
└── README.md                 # This documentation file
```

## Key Components

### GitService
Handles all Git operations using LibGit2Sharp:
- Repository validation and initialization
- Commit history retrieval
- Commit details lookup
- Repository status information
- Working directory file management
- File staging and unstaging
- Commit creation with custom messages
- Repository reset to specific commits
- Discard all changes functionality
- File tree generation for explorer

### CommitInfo Model
Data structure representing commit information with properties for:
- Commit ID (full and short)
- Author details
- Commit message
- Date and time
- Formatted display text

### FileSelectionInfo Model
Data structure representing file selection information with properties for:
- File path and name
- File status (modified, added, untracked, deleted, renamed)
- Selection state
- Relative path for git operations

### FileTreeItem Model
Data structure representing file tree items with properties for:
- File/directory name and path
- Directory/file type identification
- Tree expansion state
- File icons based on file type
- Hierarchical children collection

### MainWindow
WPF interface providing:
- Directory selection and browsing
- Repository management buttons
- Commit history tree view
- Commit details display panel
- Status information display
- File selection and staging interface
- Commit creation with author information
- File explorer tree view
- Repository reset functionality
- Discard changes functionality

## Limitations (MVP Version)

This is an MVP (Minimum Viable Product) version with the following limitations:
- No branching functionality (create, switch, merge branches)
- No merge conflict resolution
- No remote repository operations (clone, push, pull)
- No diff visualization
- Windows-only support

## Future Enhancements

Potential features for future versions:
- Branch creation and switching
- Merge operations
- Remote repository support
- Diff visualization
- Cross-platform support (Linux, macOS)
- File comparison and diff views
- Advanced filtering and search in file lists

## Troubleshooting

### Common Issues

1. **"LibGit2Sharp not found" error**
   - Make sure you've run `dotnet restore` to install dependencies

2. **Application won't start**
   - Verify you have .NET 9.0 or later installed
   - Check that you're running on a Windows system

3. **Repository not loading**
   - Ensure the selected directory contains a valid Git repository
   - Check that you have read permissions for the directory

4. **Empty commit history**
   - The repository might be newly initialized with no commits
   - Verify that the repository has actual commits by using `git log` in command line

### Support

This is an MVP demonstration project. For issues or questions, refer to the source code and comments for understanding the implementation.

## License

This project is provided as-is for educational and demonstration purposes. 