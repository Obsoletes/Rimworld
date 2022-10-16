param (
    [string] $SourceFolder,
    [string] $TranslateFolder,
    [switch] [bool] $CreateMissed
)
function Get-TranslateItem([System.IO.DirectoryInfo]$Folder) {
    $hashtable = @{}
    foreach ($file in $Folder.GetFiles('*.xml')) {
        foreach ($node in (Select-Xml -Path $file -XPath '/LanguageData/*' | Select-Object -ExpandProperty Node)) {
            $hashtable.Add($node.Name, @{
                    'Name' = $file.Name 
                    'Text' = $node.InnerText
                })
        }
    }
    $hashtable
}
function New-Miss ([string] $path, [hashtable]$Diff) {
    [string] $fileName = [datetime]::Now.ToString('yyyy-MM-dd.x\ml')
    $file = [System.IO.File]::OpenWrite([System.IO.Path]::Combine($path, $fileName))
    $writer = New-Object System.IO.StreamWriter $file
    $writer.WriteLine('<?xml version="1.0" encoding="UTF-8"?>')
    $writer.WriteLine('<LanguageData>')
    foreach ($item in $Diff.GetEnumerator()) {
        $writer.WriteLine("  <$($item.Name)>$($item.Value.Text)</$($item.Name)>")
    }
    $writer.WriteLine('</LanguageData>')
    $writer.Close()
}
function Compare-Translate([string] $SourceFolder, [string] $TranslateFolder) {
    if ([string]::IsNullOrEmpty($SourceFolder) -or -not [System.IO.Directory]::Exists($SourceFolder)) {
        Write-Error "Directory($SourceFolder) not found"
        return -1
    }
    if ([string]::IsNullOrEmpty($TranslateFolder) -or -not [System.IO.Directory]::Exists($TranslateFolder)) {
        Write-Error "Directory($TranslateFolder) not found"
        return -1
    }
    [hashtable] $source = Get-TranslateItem -Folder (New-Object System.IO.DirectoryInfo $SourceFolder)
    [hashtable] $translate = Get-TranslateItem -Folder (New-Object System.IO.DirectoryInfo $TranslateFolder)
    [hashtable] $result = @{}
    foreach ($entity in $source.GetEnumerator()) {
        if (-not $translate.Contains($entity.Name)) {
            $result.Add($entity.Name, $entity.Value)
        }
    }
    return $result
}
function Write-Help {
    Write-Host "Useage:"
    Write-Host "    .\Diff.ps1 -SourceFolder -TranslateFolder [-CreateMissed]"
    Write-Host "Example:"
    Write-Host "    .\Diff.ps1 -SourceFolder .\Source\ -TranslateFolder ..\Languages\ChineseSimplified\Keyed\"
}
[System.Environment]::CurrentDirectory = $PSScriptRoot
$result = Compare-Translate -SourceFolder $SourceFolder -TranslateFolder $TranslateFolder
if ($result -eq -1) {
    Write-Help
    exit 0
}
$S = $result.GetEnumerator() | Select-Object -Property Key, { $_.Value.Name }
Write-Host ($S | Out-String)
if ($CreateMissed) {
    New-Miss (Resolve-Path $TranslateFolder) $result
}