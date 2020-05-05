Enable-BitLocker -MountPoint "E:" -UsedSpaceOnly -Password ("12345678" | ConvertTo-SecureString -AsPlainText -Force) -PasswordProtector
