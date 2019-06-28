$gitVersion = git rev-list --all --count;
$name = git rev-parse --abbrev-ref HEAD;
$gitText= "{0}" -f [convert]::ToInt32($gitVersion, 10);

$assemblyFile = $args[0] + "\Properties\AssemblyInfo.cs";
$templateFile =  $args[0] + "\Properties\AssemblyInfo_Template.cs";

$newAssemblyContent = Get-Content $templateFile |
%{$_ -replace '0.0.0.0', ('1.2.7.' + $gitText) }

If (-not (Test-Path $assemblyFile) -or ((Compare-Object (Get-Content $assemblyFile) $newAssemblyContent))) {
    echo "Injecting Git Version Info to AssemblyInfo.cs"
    $newAssemblyContent > $assemblyFile;       
}