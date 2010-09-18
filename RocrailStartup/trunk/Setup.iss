; -- Setup.iss --
; Installer for Rocrail starter
;

#define AppId      "RocrailStartup_v1"
#define AppName    "RocrailStartup"
#define AppShortName "RocrailStartup"
#define AppExeName   "RocrailStartup"
#define AppVersion GetFileVersion("App\bin\Debug\RocrailStartup.exe")

[Setup]
AppId={#AppId}
AppName={#AppName}
AppCopyright=TallApplications
AppVerName={#AppName} {#AppVersion}
AppVersion={#AppVersion}
AppPublisher=MGV
AppPublisherURL=http://www.modelspoorgroepvenlo.nl
DefaultDirName={pf}\MGV
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
SetupIconFile=..\..\Graphics\Icons\Train.ico
UninstallDisplayIcon={app}\{#AppExeName}.exe
Compression=lzma
SolidCompression=yes
OutputDir=..\Setup
OutputBaseFileName=RocrailStartupSetup
VersionInfoDescription={#AppName} installer
VersionInfoVersion={#AppVersion}
AllowUNCPath=no
ArchitecturesInstallIn64BitMode=x64
ArchitecturesAllowed=x86 x64
SourceDir=App\bin\Debug

[Files]
Source: "RocrailStartup.exe"; DestDir: "{app}"; Flags: replacesameversion;

[Icons]
Name: "{group}\{#AppShortName}"; Filename: "{app}\RocrailStartup.exe"; WorkingDir: "{app}"

[Run]
Filename: "{app}\RocrailStartup.exe"; Description: "{cm:StartApp}"; Flags: postinstall nowait skipifsilent;

[UninstallDelete]
Type: filesandordirs; Name: "{app}";

[CustomMessages]
StartApp=Start {#AppName}




