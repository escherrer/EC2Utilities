; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "EC2 Utilities"
#define MyAppVersion "2.0.0"
#define MyAppPublisher "SystemSoup"
#define MyAppURL "http://systemsoup.com/"
#define MyAppExeName "EC2Utilities.Host.Console.exe"
#define NServiceBusHost "NServiceBus.Host.exe"
#define ConsoleBackupName "Set Up Backups"
#define InstallServiceBus "Install Service Bus"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{6FDF4CBA-4B70-477C-9CBF-52D39B5294C7}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputBaseFilename=EC2UtilitiesSetup
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\EC2Utilities.Host.Console\bin\Release\*"; DestDir: "{app}\Console"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "..\EC2Utilities.Host.WebApp\*"; DestDir: "{app}\Web"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\EC2Utilities.ServiceBus\bin\Release\*"; DestDir: "{app}\ServiceBus"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\Console\{#MyAppExeName}"; Parameters: "-s"; Description: "{cm:LaunchProgram,{#StringChange(ConsoleBackupName, '&', '&&')}}";
Filename: "{app}\ServiceBus\{#NServiceBusHost}"; Parameters: "/install /serviceName:EC2UtilitiesServiceBus /displayName:EC2_Utilities_Service_Bus /description:EC2_Utilities_Service_Bus NServiceBus.Integration"; Description: "{cm:LaunchProgram,{#StringChange(InstallServiceBus, '&', '&&')}}";
Filename: "net.exe"; Parameters: "start EC2_Utilities_Service_Bus"; Description: "Starting service bus"; 

[UninstallRun]
Filename: "{app}\Console\{#MyAppExeName}"; Parameters: "-u -r";
Filename: "{app}\ServiceBus\{#NServiceBusHost}"; Parameters: "/uninstall /serviceName:EC2UtilitiesServiceBus"; 


