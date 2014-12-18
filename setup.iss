[Setup]
AppName=Amp
AppVersion=1.0.1
AppVerName=Amp 1.0.1
AppPublisher=Adrian L Lange
AppPublisherURL=https://github.com/p3lim/Amp#readme
DefaultDirName={pf}\Amp
DefaultGroupName=Amp
AllowNoIcons=yes
LicenseFile=D:\Code\repos\Amp\LICENSE
OutputDir=D:\Code\repos\Amp\bin\Release
OutputBaseFilename=Amp-1.0.1
Compression=lzma
SolidCompression=yes

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: " "; Flags: unchecked
Name: "runonstartup"; Description: "{cm:RunOnStartup,Amp}"; GroupDescription: " "; Flags: checkablealone

[CustomMessages]
RunOnStartup=Run %1 on startup

[Files]
Source: "D:\Code\repos\Amp\bin\Debug\Amp.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Code\repos\Amp\LICENSE"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Code\repos\Amp\README.md"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Amp"; Filename: "{app}\Amp.exe"
Name: "{group}\{cm:ProgramOnTheWeb,Amp}"; Filename: "https://github.com/p3lim/Amp#readme"
Name: "{group}\{cm:UninstallProgram,Amp}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\Amp"; Filename: "{app}\Amp.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\Amp.exe"; Description: "{cm:LaunchProgram,Amp}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: dirifempty; Name: "{app}"

[Code]
const
	RegPath = 'Software\Microsoft\Windows\CurrentVersion\Run';

procedure CurStepChanged(CurStep: TSetupStep);
begin
	if CurStep = ssPostInstall then begin
		if IsTaskSelected('runonstartup') then
			RegWriteStringValue(HKCU, RegPath, 'Amp', '"' + ExpandConstant('{app}') + '\Amp.exe"');
		if not IsTaskSelected('runonstartup') and RegValueExists(HKCU, RegPath, 'Amp') then
			RegDeleteValue(HKCU, RegPath, 'Amp');
	end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
	if CurUninstallStep = usUninstall then begin
		if RegValueExists(HKCU, RegPath, 'Amp') then
			RegDeleteValue(HKCU, RegPath, 'Amp');
	end;
end;
