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


            var current_devices = get_current_devices();
            current_connections.Items.AddRange(current_devices.ToArray());

            //TimerCallback tm = new TimerCallback(monitoring);
            //System.Threading.Timer timer = new System.Threading.Timer(tm, 0, 0, 2000);
            Thread myThread = new Thread(monitoring);
            myThread.Start();

        }

        private void monitoring(object obj)
        {
            while (true)
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

            if (DeviceInstallRestrictions != null && int.Parse(DeviceInstallRestrictions.GetValue("AllowInstanceIDs").ToString()) == 1)
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
            foreach (string valueName in AllowInstanceIDs.GetValueNames())
            {
                if (AllowInstanceIDs.GetValue(valueName).ToString() == DeviceInstanceID)
                {
                    whiteDeviceExist = true;
                }
            }

            if (!whiteDeviceExist)
            {
                AllowInstanceIDs.SetValue($"{white_list.Items.Count + 1}", DeviceInstanceID);
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

    }
}
