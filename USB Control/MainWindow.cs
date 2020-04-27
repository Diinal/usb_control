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


namespace USB_Control
{

    public partial class main_window : Form
    {
        string curDeviceSelValue;
        string whiteDeviceSelValue;

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


            string[] current_devices = get_current_devices();
            current_connections.Items.AddRange(current_devices);

            //TimerCallback tm = new TimerCallback(monitoring);
            //System.Threading.Timer timer = new System.Threading.Timer(tm, 0, 0, 2000);
            Thread myThread = new Thread(monitoring);
            myThread.Start();

        }

        private void monitoring(object obj)
        {
            while (true)
            {
                Action clearItemsCurrent = () => current_connections.Items.Clear();
                if (InvokeRequired)
                    Invoke(clearItemsCurrent);
                else
                    clearItemsCurrent();

                Action setNewRangeCurrent = () => current_connections.Items.AddRange(get_current_devices());
                if (InvokeRequired)
                    Invoke(setNewRangeCurrent);
                else
                    setNewRangeCurrent();

                Console.WriteLine(get_current_devices().Length);

                Action clearItemsWhite = () => white_list.Items.Clear();
                if (InvokeRequired)
                    Invoke(clearItemsWhite);
                else
                    clearItemsWhite();

                Action setNewRangeWhite = () => white_list.Items.AddRange(get_white_devices());
                if (InvokeRequired)
                    Invoke(setNewRangeWhite);
                else
                    setNewRangeWhite();

                Thread.Sleep(1000);
            }

        }

        private string[] get_current_devices()
        {
            RegistryKey rkLocalMachine = Registry.LocalMachine;
            RegistryKey enums = rkLocalMachine.OpenSubKey("SYSTEM", false).OpenSubKey("CurrentControlSet", false).OpenSubKey("Services", false).OpenSubKey("USBSTOR", false).OpenSubKey("Enum", false);
            List<string> current_devices = new List<string>();
            //string[] current_devices = enums.GetValueNames();

            if (int.Parse(enums.GetValue("Count").ToString()) > 0)
            {
                foreach (string valueName in enums.GetValueNames())
                {
                    Regex r = new Regex(@"\d"); // Соответствует любая цифра, восклицательный знак, решётка или буква h. Если нужны только цифры, то @"\d".
                    Match m = r.Match(valueName);
                    if (m.Success == true)
                    {
                        current_devices.Add(enums.GetValue(valueName).ToString());
                    }
                }
            }

            return current_devices.ToArray();
        }

        private string[] get_white_devices()
        {
            RegistryKey rkLocalMachine = Registry.LocalMachine;
            RegistryKey WinPolicies = rkLocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows", false);
            RegistryKey DeviceInstall = get_key(WinPolicies, "DeviceInstall", false);
            RegistryKey DeviceInstallRestrictions = get_key(DeviceInstall, "Restrictions", false);

            List<string> white_devices = new List<string>();

            if (int.Parse(DeviceInstallRestrictions.GetValue("AllowDeviceIDs").ToString()) > 0)
            {
                RegistryKey AllowDeviceIDs = DeviceInstallRestrictions.OpenSubKey("AllowDeviceIDs", false);

                foreach (string valueName in AllowDeviceIDs.GetValueNames())
                {
                    Regex r = new Regex(@"\d"); // Соответствует любая цифра, восклицательный знак, решётка или буква h. Если нужны только цифры, то @"\d".
                    Match m = r.Match(valueName);
                    if (m.Success == true)
                    {
                        white_devices.Add(AllowDeviceIDs.GetValue(valueName).ToString());
                    }
                }
            }

            return white_devices.ToArray();
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
            DeviceInstallRestrictions.SetValue("AllowDeviceIDs", 1);

            RegistryKey AllowDeviceIDs = get_key(DeviceInstallRestrictions, "AllowDeviceIDs");

            string DeviceID = Regex.Match(curDeviceSelValue, @".*\\(.*)").Groups[1].ToString();

            //добавить проверку наличия id в ключе
            bool whiteDeviceExist = false;
            foreach (string valueName in AllowDeviceIDs.GetValueNames())
            {
                if (AllowDeviceIDs.GetValue(valueName).ToString() == DeviceID)
                {
                    whiteDeviceExist = true;
                }
            }

            if (whiteDeviceExist)
            {
                AllowDeviceIDs.SetValue($"{white_list.Items.Count + 1}", DeviceID);
            }

            white_list.Items.Clear();
            white_list.Items.AddRange(get_white_devices());

        }

        private void current_connections_SelectedIndexChanged(object sender, EventArgs e)
        {
            curDeviceSelValue = current_connections.SelectedItem.ToString();
        }

        private void white_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            whiteDeviceSelValue = white_list.SelectedItem.ToString();
        }
    }
}
