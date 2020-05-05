Enable-BitLocker -MountPoint "D:" -UsedSpaceOnly -Password ("Password" | ConvertTo-SecureString -AsPlainText -Force) -PasswordProtector
