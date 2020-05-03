using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;
using System.Security.Cryptography;


namespace USB_Control
{

    public partial class main_window : Form
    {
        string curDeviceSelValue;
        string whiteDeviceSelValue;
        bool backgroundThreadIsRunning = true;
        Thread monitoringThread;

        public main_window()
        {
            InitializeComponent();

            //RegistryKey rkLocalMachine = Registry.LocalMachine;
            //RegistryKey enums = rkLocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USBSTOR", true);
            //foreach (String key in enums.GetSubKeyNames())
              //  enums.DeleteSubKey(key);
            //enums.Close();
            //enums.DeleteSubKeyTree("Disk&Ven_JetFlash&Prod_Transcend_4GB&Rev_8.07");
            //rkLocalMachine.DeleteSubKeyTree(@"SYSTEM\CurrentControlSet\Enum\USBSTOR");


            var current_devices = get_current_devices();
            current_connections.Items.AddRange(current_devices.ToArray());

            //TimerCallback tm = new TimerCallback(monitoring);
            //System.Threading.Timer timer = new System.Threading.Timer(tm, 0, 0, 2000);
            //Thread myThread = new Thread(monitoring);
            monitoringThread = new Thread(monitoring);
            monitoringThread.IsBackground = true;
            monitoringThread.Start();

        }

        private void monitoring(object obj)
        {
            while (backgroundThreadIsRunning)
            {
                var currentDevices = get_current_devices();

                //Action clearItemsCurrent = () => current_connections.Items.Clear();
                Action clearItemsCurrent = () => {
                    if (!IfEqual(current_connections.Items.Cast<string>().ToList(), currentDevices))
                    {
                        current_connections.Items.Clear();
                    }
                };
                if (InvokeRequired)
                    Invoke(clearItemsCurrent);
                else
                    clearItemsCurrent();

                //Action setNewRangeCurrent = () => current_connections.Items.AddRange(currentDevices);
                Action setNewRangeCurrent = () => {
                    if (!IfEqual(current_connections.Items.Cast<string>().ToList(), currentDevices))
                    {
                        current_connections.Items.AddRange(currentDevices.ToArray());
                    }
                };
                if (InvokeRequired)
                    Invoke(setNewRangeCurrent);
                else
                    setNewRangeCurrent();

                //Console.WriteLine(currentDevices.Length);

                var whiteDevices = get_white_devices();
                //Action clearItemsWhite = () => white_list.Items.Clear();
                Action clearItemsWhite = () =>
                {
                    if (!IfEqual(white_list.Items.Cast<string>().ToList(), whiteDevices))
                    {
                        white_list.Items.Clear();
                    }
                };
                if (InvokeRequired)
                    Invoke(clearItemsWhite);
                else
                    clearItemsWhite();

                //Action setNewRangeWhite = () => white_list.Items.AddRange(get_white_devices());
                Action setNewRangeWhite = () =>
                {
                    if (!IfEqual(white_list.Items.Cast<string>().ToList(), whiteDevices))
                    {
                        white_list.Items.AddRange(whiteDevices.ToArray());
                    }
                };
                if (InvokeRequired)
                    Invoke(setNewRangeWhite);
                else
                    setNewRangeWhite();

                Thread.Sleep(1000);
            }

        }

        bool IfEqual(List<string> list1, List<string> list2)
        {
            var diff1 = list1.Except(list2);
            var diff2 = list2.Except(list1);

            return !diff1.Any() && !diff2.Any();
        }

        private List<string> get_current_devices()
        {
            RegistryKey rkLocalMachine = Registry.LocalMachine;
            //RegistryKey enums = rkLocalMachine.OpenSubKey("SYSTEM", false).OpenSubKey("CurrentControlSet", false).OpenSubKey("Services", false).OpenSubKey("USBSTOR", false).OpenSubKey("Enum", false);
            RegistryKey enums = rkLocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\disk\Enum");
            List<string> current_devices = new List<string>();
            //string[] current_devices = enums.GetValueNames();

            foreach (string valueName in enums.GetValueNames())
            {
                Regex r = new Regex(@"USBSTOR\\.*\\.*");
                Match m = r.Match(enums.GetValue(valueName).ToString());
                //Match m = r.Match(valueName);
                if (m.Success == true)
                {
                    current_devices.Add(enums.GetValue(valueName).ToString());
                }
            }

            return current_devices;
        }

        private List<string> get_white_devices()
        {
            RegistryKey rkLocalMachine = Registry.LocalMachine;
            RegistryKey WinPolicies = rkLocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows", false);
            //RegistryKey DeviceInstall = get_key(WinPolicies, "DeviceInstall", false);
            //RegistryKey DeviceInstallRestrictions = get_key(DeviceInstall, "Restrictions", false);
            RegistryKey DeviceInstallRestrictions = WinPolicies.OpenSubKey(@"DeviceInstall\Restrictions");

            List<string> white_devices = new List<string>();

            if (DeviceInstallRestrictions != null && DeviceInstallRestrictions.GetValue("AllowInstanceIDs") != null && int.Parse(DeviceInstallRestrictions.GetValue("AllowInstanceIDs").ToString()) == 1)
            {
                RegistryKey AllowInstanceIDs = DeviceInstallRestrictions.OpenSubKey("AllowInstanceIDs", false);

                if (AllowInstanceIDs != null)
                {
                    foreach (string valueName in AllowInstanceIDs.GetValueNames())
                    {
                        Regex r = new Regex(@"\d"); // Соответствует любая цифра, восклицательный знак, решётка или буква h. Если нужны только цифры, то @"\d".
                        Match m = r.Match(valueName);
                        if (m.Success == true)
                        {
                            var a = AllowInstanceIDs.GetValue(valueName).ToString();
                            white_devices.Add(AllowInstanceIDs.GetValue(valueName).ToString());
                        }
                    }
                }
            }

            return white_devices;
        }

        private RegistryKey get_key(RegistryKey parent, string keyname, bool writable = true)
        {
            RegistryKey result;

            if (parent.OpenSubKey(keyname, writable) == null)
            {
                result = parent.CreateSubKey(keyname);
            }
            else
            {
                result = parent.OpenSubKey(keyname, writable);
            }
            return result;
        }
    

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
        "Данное приложение представляет собой решение по контролю подключений USB устройств.\nСоздано в качестветехнической части курсовой работы.\nАвтор: Лошманов Даниил",
        "О программе",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);
        }

        private void deny_connections_Click(object sender, EventArgs e)
        {
            RegistryKey rkLocalMachine = Registry.LocalMachine;
            RegistryKey enums = rkLocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USBSTOR", true);
            
            foreach (string USBClassKeyName in enums.GetSubKeyNames())
            {
                RegistryKey usb_classes = enums.OpenSubKey(USBClassKeyName, true);
                foreach (string USBDeviceKeyName in usb_classes.GetSubKeyNames())
                {
                    RegistryKey usb_device = usb_classes.OpenSubKey(USBDeviceKeyName, true);
                    foreach (string param in usb_device.GetValueNames())
                    {
                        usb_device.DeleteValue(param);
                    }
                }
            }

            RegistryKey WinPolicies = rkLocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows", true);
            RegistryKey DeviceInstall = get_key(WinPolicies, "DeviceInstall");
            RegistryKey DeviceInstallRestrictions = get_key(DeviceInstall, "Restrictions");
            DeviceInstallRestrictions.SetValue("DenyUnspecified", 1);


        }

        private void allow_connections_Click(object sender, EventArgs e)
        {
            RegistryKey rkLocalMachine = Registry.LocalMachine;
            RegistryKey enums = rkLocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USBSTOR", true);

            foreach (string USBClassKeyName in enums.GetSubKeyNames())
            {
                RegistryKey usb_classes = enums.OpenSubKey(USBClassKeyName, true);
                foreach (string USBDeviceKeyName in usb_classes.GetSubKeyNames())
                {
                    RegistryKey usb_device = usb_classes.OpenSubKey(USBDeviceKeyName, true);
                    foreach (string param in usb_device.GetValueNames())
                    {
                        usb_device.DeleteValue(param);
                    }
                }
            }

            RegistryKey WinPolicies = rkLocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows", true);
            RegistryKey DeviceInstall = get_key(WinPolicies, "DeviceInstall");
            RegistryKey DeviceInstallRestrictions = get_key(DeviceInstall, "Restrictions");
            DeviceInstallRestrictions.SetValue("DenyUnspecified", 0);
        }

        private void add_device_Click(object sender, EventArgs e)
        {
            RegistryKey rkLocalMachine = Registry.LocalMachine;
            RegistryKey WinPolicies = rkLocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows", true);
            RegistryKey DeviceInstall = get_key(WinPolicies, "DeviceInstall");
            RegistryKey DeviceInstallRestrictions = get_key(DeviceInstall, "Restrictions");
            DeviceInstallRestrictions.SetValue("AllowInstanceIDs", 1);

            RegistryKey AllowInstanceIDs = get_key(DeviceInstallRestrictions, "AllowInstanceIDs");

            //string DeviceInstanceID = Regex.Match(curDeviceSelValue, @".*\\(.*)").Groups[1].ToString();
            string DeviceInstanceID = curDeviceSelValue;
            //добавить проверку наличия id в ключе
            bool whiteDeviceExist = false;
            int whiteDeviceNumber = 1;

            foreach (string valueName in AllowInstanceIDs.GetValueNames())
            {
                if (AllowInstanceIDs.GetValue(valueName).ToString() == DeviceInstanceID)
                {
                    whiteDeviceExist = true;
                }
                if (valueName == whiteDeviceNumber.ToString())
                {
                    whiteDeviceNumber += 1;
                }
            }

            if (!whiteDeviceExist)
            {
                AllowInstanceIDs.SetValue($"{whiteDeviceNumber}", DeviceInstanceID);
            }

            white_list.Items.Clear();
            white_list.Items.AddRange(get_white_devices().ToArray());

        }

        private void delete_device_Click(object sender, EventArgs e)
        {
            if (get_white_devices().ToArray().Length > 0) {
                RegistryKey rkLocalMachine = Registry.LocalMachine;
                RegistryKey WinPolicies = rkLocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows", true);
                RegistryKey DeviceInstall = get_key(WinPolicies, "DeviceInstall");
                RegistryKey DeviceInstallRestrictions = get_key(DeviceInstall, "Restrictions");
                DeviceInstallRestrictions.SetValue("AllowInstanceIDs", 1);

                RegistryKey AllowInstanceIDs = get_key(DeviceInstallRestrictions, "AllowInstanceIDs");
                string DeviceInstanceID = whiteDeviceSelValue;

                foreach (string valueName in AllowInstanceIDs.GetValueNames())
                {
                    if (AllowInstanceIDs.GetValue(valueName).ToString() == DeviceInstanceID)
                    {
                        AllowInstanceIDs.DeleteValue(valueName);
                    }
                }

            }
        }

        private void current_connections_SelectedIndexChanged(object sender, EventArgs e)
        {
            curDeviceSelValue = current_connections.SelectedItem.ToString();
        }

        private void white_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            whiteDeviceSelValue = white_list.SelectedItem.ToString();
        }

        private void main_window_FormClosed(object sender, FormClosedEventArgs e)
        {
            backgroundThreadIsRunning = false;
            monitoringThread.Abort();
            System.Windows.Forms.Application.Exit();
        }

        private void dropField_DragEnter(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                SourceField.Items.Add(file);
            }
        }

        private void dropField_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                // запишем в нашу переменную путь к папке
                string fileName = openFileDialog.FileName;
                SourceField.Items.Add(fileName);
            }
        }

        private void openFolderButtonDestination_Click(object sender, EventArgs e)
        {
            // показать диалог выбора папки
            DialogResult result = folderBrowserDialog.ShowDialog();

            // если папка выбрана и нажата клавиша `OK` - значит можно получить путь к папке
            if (result == DialogResult.OK)
            {
                // запишем в нашу переменную путь к папке
                string folderName = folderBrowserDialog.SelectedPath;
                DestinationField.Items.Add(folderName);
            }
        }

        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fille the buffer with the generated data
                    rng.GetBytes(data);
                }
            }

            return data;
        }

        private void FileEncrypt(string inputFile, string filePathDestination, string password)
        {
            //http://stackoverflow.com/questions/27645527/aes-encryption-on-large-files

            //generate random salt
            byte[] salt = GenerateRandomSalt();

            //create output file name
            FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create);

            //convert password string to byte arrray
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            //Set Rijndael symmetric encryption algorithm
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;

            //http://stackoverflow.com/questions/2659214/why-do-i-need-to-use-the-rfc2898derivebytes-class-in-net-instead-of-directly
            //"What it does is repeatedly hash the user password along with the salt." High iteration counts.
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);

            //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
            AES.Mode = CipherMode.CFB;

            // write salt to the begining of the output file, so in this case can be random every time
            fsCrypt.Write(salt, 0, salt.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            //create a buffer (1mb) so only this amount will allocate in the memory and not the whole file
            byte[] buffer = new byte[2097152];//2097152  1048576
            int read;

            try
            {
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Application.DoEvents(); // -> for responsive GUI, using Task will be better!
                    cs.Write(buffer, 0, read);
                }

                // Close up
                fsIn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                cs.Close();
                fsCrypt.Close();
            }

            string sourceFileName = Regex.Match(inputFile, @".*\\(.*\..*)").Groups[1].ToString();
            if (filePathDestination[filePathDestination.Length - 1] == @"\".ToCharArray()[0])
            {
                filePathDestination += sourceFileName;
            }
            else
            {
                filePathDestination += "\\" + sourceFileName;
            }
            //filePathDestination += sourceFileName;
            File.Move(inputFile + ".aes", filePathDestination + ".aes");
        }

        private void EncryptFile(string filePathSource, string filePathDestination,  byte[] key)
        {
            string tempFileName = Path.GetTempFileName();
            string sourceFileName = Regex.Match(filePathSource, @".*\\(.*\..*)").Groups[1].ToString()+ ".enc";  

            using (SymmetricAlgorithm cipher = Aes.Create())
            using (FileStream fileStream = File.OpenRead(filePathSource))
            using (FileStream tempFile = File.Create(tempFileName))
            {
                cipher.Key = key;
                // aes.IV will be automatically populated with a secure random value
                byte[] iv = cipher.IV;

                // Write a marker header so we can identify how to read this file in the future
                tempFile.WriteByte(69);
                tempFile.WriteByte(78);
                tempFile.WriteByte(67);
                tempFile.WriteByte(82);
                tempFile.WriteByte(89);
                tempFile.WriteByte(80);
                tempFile.WriteByte(84);
                tempFile.WriteByte(69);
                tempFile.WriteByte(68);

                tempFile.Write(iv, 0, iv.Length);

                using (var cryptoStream =
                    new CryptoStream(tempFile, cipher.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    fileStream.CopyTo(cryptoStream);
                }
            }

            //File.Delete(filePath);
            if (filePathDestination[filePathDestination.Length-1] == @"\".ToCharArray()[0])
            {
                filePathDestination += sourceFileName;
            }
            else
            {
                filePathDestination += "\\" + sourceFileName;
            }
            //filePathDestination += sourceFileName;
            File.Move(tempFileName, filePathDestination);
            Console.WriteLine("Encrypt completed");
        }

        private void FileDecrypt(string inputFile, string outputFile, string password)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            string sourceFileName = Regex.Match(inputFile, @".*\\(.*\..*)").Groups[1].ToString();
            sourceFileName = sourceFileName.Substring(0, sourceFileName.Length - 3);
            string tempFileName = Path.GetTempFileName();
            FileStream fsOut = new FileStream(tempFileName, FileMode.Create);

            int read;
            byte[] buffer = new byte[2097152];//2097152 1048576

            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    Application.DoEvents();
                    fsOut.Write(buffer, 0, read);
                }
            }
            catch (CryptographicException ex_CryptographicException)
            {
                Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error by closing CryptoStream: " + ex.Message);
            }
            finally
            {
                fsOut.Close();
                fsCrypt.Close();
            }

            if (outputFile[outputFile.Length - 1] == @"\".ToCharArray()[0])
            {
                outputFile += sourceFileName;
            }
            else
            {
                outputFile += "\\" + sourceFileName;
            }
            File.Move(tempFileName, outputFile);
        }

        public static void DecryptFile(string filePathSource, string filePathDestination, byte[] key)
        {
            string tempFileName = Path.GetTempFileName();
            string sourceFileName = Regex.Match(filePathSource, @".*\\(.*\..*)").Groups[1].ToString();
            sourceFileName = sourceFileName.Substring(0, sourceFileName.Length - 3);

            using (SymmetricAlgorithm cipher = Aes.Create())
            using (FileStream fileStream = File.OpenRead(filePathSource))
            using (FileStream tempFile = File.Create(tempFileName))
            {
                cipher.Key = key;
                byte[] iv = new byte[cipher.BlockSize / 8];
                byte[] headerBytes = new byte[9];
                int remain = headerBytes.Length;

                while (remain != 0)
                {
                    int read = fileStream.Read(headerBytes, headerBytes.Length - remain, remain);

                    if (read == 0)
                    {
                        throw new EndOfStreamException();
                    }

                    remain -= read;
                }

                if (headerBytes[0] != 69 ||
                    headerBytes[1] != 78 ||
                    headerBytes[2] != 67 ||
                    headerBytes[3] != 82 ||
                    headerBytes[4] != 89 ||
                    headerBytes[5] != 80 ||
                    headerBytes[6] != 84 ||
                    headerBytes[7] != 69 ||
                    headerBytes[8] != 68)
                {
                    throw new InvalidOperationException();
                }

                remain = iv.Length;

                while (remain != 0)
                {
                    int read = fileStream.Read(iv, iv.Length - remain, remain);

                    if (read == 0)
                    {
                        throw new EndOfStreamException();
                    }

                    remain -= read;
                }

                cipher.IV = iv;

                using (var cryptoStream =
                    new CryptoStream(tempFile, cipher.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    fileStream.CopyTo(cryptoStream);
                }
            }

            //File.Delete(filePath);
            //filePathDestination += "\\new_file.txt";
            if (filePathDestination[filePathDestination.Length - 1] == @"\".ToCharArray()[0])
            {
                filePathDestination += sourceFileName;
            }
            else
            {
                filePathDestination += "\\" + sourceFileName;
            }
            File.Move(tempFileName, filePathDestination);
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            byte[] key = Encoding.ASCII.GetBytes(KeyTextBox.Text);
            string filePathDestination = DestinationField.Items[0].ToString();
            foreach (string filePathSource in SourceField.Items)
            {
                //EncryptFile(filePathSource, filePathDestination, key);
                //FileEncrypt(filePathSource, filePathDestination, KeyTextBox.Text);
                AES_Encrypt(filePathSource, filePathDestination, key);
            }
            
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            byte[] key = Encoding.ASCII.GetBytes(KeyTextBox.Text);
            string filePathDestination = DestinationField.Items[0].ToString();
            foreach (string filePathSource in SourceField.Items)
            {
                //DecryptFile(filePathSource, filePathDestination, key);
                //FileDecrypt(filePathSource, filePathDestination, KeyTextBox.Text);
                AES_Decrypt(filePathSource, filePathDestination, key);
            }
        }

        private void buttonClearSource_Click(object sender, EventArgs e)
        {
            SourceField.Items.Clear();
        }

        private void buttonClearDestinations_Click(object sender, EventArgs e)
        {
            DestinationField.Items.Clear();
        }



        private static void AES_Encrypt(string inputFile, string outputFile, byte[] passwordBytes)
        {
            string sourceFileName = Regex.Match(inputFile, @".*\\(.*\..*)").Groups[1].ToString() + ".enc";
            byte[] saltBytes = GenerateRandomSalt();
            string cryptFile = outputFile;

            string tempFileName = Path.GetTempFileName();
            FileStream fsCrypt = new FileStream(tempFileName, FileMode.Create);
            fsCrypt.Write(saltBytes, 0, saltBytes.Length);


            RijndaelManaged AES = new RijndaelManaged();

            AES.KeySize = 256;
            AES.BlockSize = 128;


            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.Zeros;

            AES.Mode = CipherMode.CBC;

            CryptoStream cs = new CryptoStream(fsCrypt,
                 AES.CreateEncryptor(),
                CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);


            fsIn.Close();
            cs.Close();
            fsCrypt.Close();

            if (outputFile[outputFile.Length - 1] == @"\".ToCharArray()[0])
            {
                outputFile += sourceFileName;
            }
            else
            {
                outputFile += "\\" + sourceFileName;
            }
            File.Move(tempFileName, outputFile);

        }

        private static void AES_Decrypt(string inputFile, string outputFile, byte[] passwordBytes)
        {
            string tempFileName = Path.GetTempFileName();
            string sourceFileName = Regex.Match(inputFile, @".*\\(.*\..*)").Groups[1].ToString();
            sourceFileName = sourceFileName.Substring(0, sourceFileName.Length - 3);

            byte[] saltBytes = new byte[32];
            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            fsCrypt.Read(saltBytes, 0, saltBytes.Length);

            RijndaelManaged AES = new RijndaelManaged();

            AES.KeySize = 256;
            AES.BlockSize = 128;


            var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.Zeros;

            AES.Mode = CipherMode.CBC;

            CryptoStream cs = new CryptoStream(fsCrypt,
                AES.CreateDecryptor(),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(tempFileName, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

            if (outputFile[outputFile.Length - 1] == @"\".ToCharArray()[0])
            {
                outputFile += sourceFileName;
            }
            else
            {
                outputFile += "\\" + sourceFileName;
            }
            File.Move(tempFileName, outputFile);
        }
    }


}

