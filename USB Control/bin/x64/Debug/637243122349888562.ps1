Enable-BitLocker -MountPoint "E:" -UsedSpaceOnly -Password ("23456781" | ConvertTo-SecureString -AsPlainText -Force) -PasswordProtector
