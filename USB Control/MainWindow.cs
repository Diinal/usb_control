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
using System.Diagnostics;
using System.Runtime.InteropServices;



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

            RegistryKey rkLocalMachine = Registry.LocalMachine;
            RegistryKey WinPolicies = rkLocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows", true);
            RegistryKey DeviceInstall = get_key(WinPolicies, "DeviceInstall");
            RegistryKey DeviceInstallRestrictions = get_key(DeviceInstall, "Restrictions");
            string DenyUnspecified = "default"; ;

            if (DeviceInstallRestrictions.GetValue("DenyUnspecified") != null)
                DenyUnspecified = DeviceInstallRestrictions.GetValue("DenyUnspecified").ToString();

            switch (DenyUnspecified)
            {
                case "1":
                    radioButtonDeny.Checked = true;
                    break;
                case "0":
                    radioButtonAllow.Checked = true;
                    break;
                default:
                    break;
            }

            KeyTextBox.Text = "от 8 символов";
            KeyTextBox.ForeColor = Color.Gray;

            var current_devices = get_current_devices();
            current_connections.Items.AddRange(current_devices.ToArray());

            monitoringThread = new Thread(monitoring);
            monitoringThread.IsBackground = true;
            monitoringThread.Start();

        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void monitoring(object obj)
        {
            while (backgroundThreadIsRunning)
            {
                var currentDevices = get_current_devices();

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


                var whiteDevices = get_white_devices();
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
            RegistryKey enums = rkLocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\disk\Enum");
            List<string> current_devices = new List<string>();
            //string[] current_devices = enums.GetValueNames();

            foreach (string valueName in enums.GetValueNames())
            {
                Regex r = new Regex(@"USBSTOR\\.*\\.*");
                Match m = r.Match(enums.GetValue(valueName).ToString());

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
        "Данное приложение представляет собой решение по контролю подключений USB устройств.\nСоздано в качестве технической части дипломной работы.\nАвтор: Лошманов Даниил",
        "О программе",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information,
        MessageBoxDefaultButton.Button1,
        MessageBoxOptions.DefaultDesktopOnly);
        }

        private void add_device_Click(object sender, EventArgs e)
        {
            if (curDeviceSelValue != null)
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
                else
                {
                    ShowMessage("Данное устройство уже есть в белом списке!");
                }

                white_list.Items.Clear();
                white_list.Items.AddRange(get_white_devices().ToArray());
            }
        }

        private void delete_device_Click(object sender, EventArgs e)
        {
            if (get_white_devices().ToArray().Length > 0 && whiteDeviceSelValue != null) 
            {
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

                RegistryKey enums = rkLocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\USBSTOR", true);

                var DeviceID = Regex.Match(DeviceInstanceID, @".*\\(.*)").Groups[1].ToString();
                if (!DeviceID.Contains("&0")) DeviceID += "&0";
                foreach (string USBClassKeyName in enums.GetSubKeyNames())
                {
                    RegistryKey usb_classes = enums.OpenSubKey(USBClassKeyName, true);
                    foreach (string USBDeviceKeyName in usb_classes.GetSubKeyNames())
                    {
                        if (USBDeviceKeyName == DeviceID) {
                            RegistryKey usb_device = usb_classes.OpenSubKey(USBDeviceKeyName, true);
                            foreach (string param in usb_device.GetValueNames())
                            {
                                usb_device.DeleteValue(param);
                            }
                        }
                    }
                }

            }


        }

        private void current_connections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (current_connections.SelectedItem != null)
                curDeviceSelValue = current_connections.SelectedItem.ToString();
        }

        private void white_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (white_list.SelectedItem != null)
                whiteDeviceSelValue = white_list.SelectedItem.ToString();
        }

        private void main_window_FormClosed(object sender, FormClosedEventArgs e)
        {
            backgroundThreadIsRunning = false;
            monitoringThread.Abort();
            System.Windows.Forms.Application.Exit();
        }

        private void openFileButtonSource_Click(object sender, EventArgs e)
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

        private void encryptButton_Click(object sender, EventArgs e)
        {
            if (KeyTextBox.Text.Length >= 8 && KeyTextBox.Text != "от 8 символов" && SourceField.Items.Count >= 1 && DestinationField.Items.Count >= 1)
            {
                byte[] key = Encoding.ASCII.GetBytes(KeyTextBox.Text);
                string filePathDestination = DestinationField.Items[0].ToString();
                foreach (string filePathSource in SourceField.Items)
                {
                    AES_Encrypt(filePathSource, filePathDestination, key);
                }
                ShowMessage("Файлы зашифрованы!");
            }
            else
            {
                if (KeyTextBox.Text.Length < 8 || KeyTextBox.Text == "от 8 символов")
                    ShowMessage("Длина ключа должна быть больше, либо равна 8 символам!");
                if (SourceField.Items.Count < 1)
                    ShowMessage("Выберите файлы для шифрования!");
                if (DestinationField.Items.Count < 1)
                    ShowMessage("Выберите папку для зашифрованных файлов!");
            }
            
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            if (KeyTextBox.Text.Length >= 8 && KeyTextBox.Text != "от 8 символов" && SourceField.Items.Count >= 1 && DestinationField.Items.Count >= 1)
            {
                byte[] key = Encoding.ASCII.GetBytes(KeyTextBox.Text);
                string filePathDestination = DestinationField.Items[0].ToString();
                foreach (string filePathSource in SourceField.Items)
                {
                    AES_Decrypt(filePathSource, filePathDestination, key);
                }
                ShowMessage("Файлы дешифрованы!");
            }
            else
            {
                if (KeyTextBox.Text.Length < 8 || KeyTextBox.Text == "от 8 символов")
                    ShowMessage("Длина ключа должна быть больше, либо равна 8 символам!");
                if (SourceField.Items.Count < 1)
                    ShowMessage("Выберите файлы для дешифрования!");
                if (DestinationField.Items.Count < 1)
                    ShowMessage("Выберите папку для дешифрованных файлов!");
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
            try
            {
                File.Move(tempFileName, outputFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

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
            try
            {
                File.Move(tempFileName, outputFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void radioButtonConnections_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAllow.Checked == true)
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
            else if (radioButtonDeny.Checked == true)
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
        }

        private void KeyTextBox_Enter(object sender, EventArgs e)
        {
            KeyTextBox.Text = null;
            KeyTextBox.ForeColor = Color.Black;
        }

        private void CreateCryptoDevice_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            string folderName = "";

            // если папка выбрана и нажата клавиша `OK` - значит можно получить путь к папке
            if (result == DialogResult.OK)
            {
                // запишем в нашу переменную путь к папке
                folderName = folderBrowserDialog.SelectedPath;
                if (Regex.Match(folderName, @"(.:)").Success)
                    folderName = Regex.Match(folderName, @"(.:)").Groups[1].ToString();
            }

            if (folderName != "" && KeyTextBox.Text.Length >= 8 && KeyTextBox.Text != "от 8 символов")
            {
                CreateCryptoDevice.Text = "Выполняется...";

                string powershellfilename = DateTime.Now.Ticks + ".ps1";
                StreamWriter writer = new StreamWriter(powershellfilename);
                writer.WriteLine($"Enable-BitLocker -MountPoint \"{folderName}\" -UsedSpaceOnly -Password (\"{KeyTextBox.Text}\" | ConvertTo-SecureString -AsPlainText -Force) -PasswordProtector");
                writer.Flush();
                writer.Close();

                Process runBitLocker = new Process();
                runBitLocker.StartInfo.FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
                runBitLocker.StartInfo.Arguments = "-executionpolicy Unrestricted -File " + powershellfilename;
                runBitLocker.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                runBitLocker.Start();
                runBitLocker.WaitForExit();
                ShowMessage("Устройство зашифровано!");

                Thread.Sleep(10000);
                CreateCryptoDevice.Text = "Создать криптохранилище";

            }
            else
            {
                if (KeyTextBox.Text.Length < 8 || KeyTextBox.Text == "от 8 символов")
                    ShowMessage("Длина ключа должна быть больше, либо равна 8 символам!");
                if (folderName == "")
                    ShowMessage("Выберите устройтво для шифрования!");
            }
        }

    }


}

