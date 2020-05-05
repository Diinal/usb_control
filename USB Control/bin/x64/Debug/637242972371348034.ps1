Add-BitLockerKeyProtector -MountPoint "D:" -Password ("Password" | ConvertTo-SecureString -AsPlainText -Force) -PasswordProtector
