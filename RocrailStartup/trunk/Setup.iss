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
InstallDotNet=Install the Microsoft.NET 3.5 Framework first.

[Code]
const
  dotnet35URL = 'http://download.microsoft.com/download/7/0/3/703455ee-a747-4cc8-bd3e-98a615c3aedb/dotNetFx35setup.exe';

function InitializeSetup(): Boolean;
var
  msgRes : integer;
  errCode : integer;

begin
  Result := true;
  // Check for required dotnetfx 3.5 installation
  if (not RegKeyExists(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5')) then begin
    msgRes := MsgBox(CustomMessage('InstallDotNet'), mbError, MB_OKCANCEL);
    if(msgRes = 1) then begin
      ShellExec('Open', dotnet35URL, '', '', SW_SHOW, ewNoWait, errCode);
    end;
    Abort();
  end;
end;
