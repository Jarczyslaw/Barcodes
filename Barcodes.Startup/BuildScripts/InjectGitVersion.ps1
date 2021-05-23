$appName = "Barcodes";
$company = "JTJT";
$version = "1.6.0";

$gitVersion = git rev-list --all --count;
$gitVersionText= "{0}" -f [convert]::ToInt32($gitVersion, 10);
$branchName = git rev-parse --abbrev-ref HEAD;
$hash = git rev-parse HEAD;
$shortHash = git rev-parse --short HEAD;

$assemblyFile = $args[0] + "\Properties\AssemblyInfo.cs";
$templateFile = $args[0] + "\Properties\AssemblyInfo_Template.cs";

$newAssemblyContent = Get-Content $templateFile |
%{$_ -replace "{appName}", ($appName) } |
%{$_ -replace "{company}", ($company) } |
%{$_ -replace "{version}", ($version + "." + $gitVersionText) } |
%{$_ -replace "{branch}", ("Branch: " + $branchName) } |
%{$_ -replace "{hash}", ("Hash: " + $hash) } |
%{$_ -replace "{shortHash}", ("ShortHash: " + $shortHash) } |
%{$_ -replace "{year}", (Get-Date -Format "yyyy") }

If (-not (Test-Path $assemblyFile) -or ((Compare-Object (Get-Content $assemblyFile) $newAssemblyContent))) {
    echo "Injecting Git Version Info to AssemblyInfo.cs"
    $newAssemblyContent > $assemblyFile;       
}
Else {
	echo "Injecting Git Version Info skipped"
}