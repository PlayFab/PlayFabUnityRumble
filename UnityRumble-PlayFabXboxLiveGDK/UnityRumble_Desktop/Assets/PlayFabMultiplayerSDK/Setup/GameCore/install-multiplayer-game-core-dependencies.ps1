<#
.SYNOPSIS
This script copies dependency binaries from installed GDK environment to the assets of PlayFab Multiplayer Unity SDK.
Run this script to prepare the SDK before using it in your app when building for Game Core platform.
#>

if (!($env:GRDKLatest))
{
    Write-Host -ForegroundColor Red "Environment variable GRDKLatest is not found. Please make sure Microsoft GDK is installed."
    exit 1
}

$env:dst_dir = "$PSScriptRoot\..\..\Source\DLLs\GDK"
$env:src_xcurl_dir = "$env:GRDKLatest\ExtensionLibraries\Xbox.XCurl.API\Redist\CommonConfiguration\neutral"

# Copy dependency binaries
xcopy "$env:src_xcurl_dir\*" "$env:dst_dir\*" /Y /R /F
