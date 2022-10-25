<#
.SYNOPSIS
This script copies Party binaries from installed GDK environment to the assets of Party Unity SDK.
Run this script to prepare the SDK before using it in your app when building for Game Core platform.
#>

if (!($env:GRDKLatest))
{
    Write-Host -ForegroundColor Red "Environment variable GRDKLatest is not found. Please make sure Microsoft GDK is installed."
    exit 1
}

$env:dst_dir = "$PSScriptRoot\..\..\Source\DLLs\GameCore"
$env:src_party_dir = "$env:GRDKLatest\ExtensionLibraries\PlayFab.Party.Cpp\Redist\CommonConfiguration\neutral"
$env:src_partyxbl_dir = "$env:GRDKLatest\ExtensionLibraries\PlayFab.PartyXboxLive.Cpp\Redist\CommonConfiguration\neutral"

# Clear destination directory
rm -rec -force $env:dst_dir -ErrorAction silent

# Copy Party binaries for Game Core and rename them as expected by Party Unity SDK
xcopy "$env:src_party_dir\Party.dll" "$env:dst_dir\PartyWin.dll*" /Y /R /F
xcopy "$env:src_party_dir\Party.pdb" "$env:dst_dir\PartyWin.pdb*" /Y /R /F
xcopy "$env:src_partyxbl_dir\PartyXboxLive.dll" "$env:dst_dir\PartyXboxLive.dll*" /Y /R /F
xcopy "$env:src_partyxbl_dir\PartyXboxLive.pdb" "$env:dst_dir\PartyXboxLive.pdb*" /Y /R /F
