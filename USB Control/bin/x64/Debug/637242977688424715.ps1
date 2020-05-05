Enable-BitLocker -MountPoint "E:" -UsedSpaceOnly -Password ("Password" | ConvertTo-SecureString -AsPlainText -Force) -PasswordProtector
