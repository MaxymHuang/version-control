[Setup]
AppName=Git Version Control GUI
AppVersion=1.0.0
AppPublisher=Your Company Name
AppPublisherURL=https://yourcompany.com
AppSupportURL=https://yourcompany.com/support
AppUpdatesURL=https://yourcompany.com/updates
DefaultDirName={autopf}\GitVersionControl
DefaultGroupName=Git Version Control GUI
AllowNoIcons=yes
LicenseFile=
InfoBeforeFile=
InfoAfterFile=
OutputDir=Output
OutputBaseFilename=GitVersionControl-Setup
SetupIconFile=Assets\git-icon-black.png
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1; Check: not IsAdminInstallMode

[Files]
Source: "bin\Release\net9.0-windows\win-x64\publish\GitVersionControl.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net9.0-windows\win-x64\publish\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net9.0-windows\win-x64\publish\*.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net9.0-windows\win-x64\publish\cs\*"; DestDir: "{app}\cs"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\de\*"; DestDir: "{app}\de"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\es\*"; DestDir: "{app}\es"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\fr\*"; DestDir: "{app}\fr"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\it\*"; DestDir: "{app}\it"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\ja\*"; DestDir: "{app}\ja"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\ko\*"; DestDir: "{app}\ko"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\pl\*"; DestDir: "{app}\pl"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\pt-BR\*"; DestDir: "{app}\pt-BR"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\ru\*"; DestDir: "{app}\ru"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\tr\*"; DestDir: "{app}\tr"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\zh-Hans\*"; DestDir: "{app}\zh-Hans"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\net9.0-windows\win-x64\publish\zh-Hant\*"; DestDir: "{app}\zh-Hant"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\Git Version Control GUI"; Filename: "{app}\GitVersionControl.exe"
Name: "{group}\{cm:UninstallProgram,Git Version Control GUI}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\Git Version Control GUI"; Filename: "{app}\GitVersionControl.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\Git Version Control GUI"; Filename: "{app}\GitVersionControl.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\GitVersionControl.exe"; Description: "{cm:LaunchProgram,Git Version Control GUI}"; Flags: nowait postinstall skipifsilent

[Code]
function InitializeSetup(): Boolean;
begin
  Result := True;
end; 