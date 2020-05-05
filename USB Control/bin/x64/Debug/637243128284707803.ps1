Enable-BitLocker -MountPoint "E:" -UsedSpaceOnly -Password ("qwerty123456" | ConvertTo-SecureString -AsPlainText -Force) -PasswordProtector
