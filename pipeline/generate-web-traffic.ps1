while ($true)
{
    (New-Object Net.WebClient).DownloadString("http://localhost:32768/")
    Start-Sleep -Milliseconds 2000
}