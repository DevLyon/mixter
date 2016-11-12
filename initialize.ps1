if(![System.IO.File]::Exists('nuget.exe')){
   (New-Object Net.WebClient).DownloadFile('https://dist.nuget.org/win-x86-commandline/latest/nuget.exe', 'nuget.exe')
}

& ./nuget.exe restore

