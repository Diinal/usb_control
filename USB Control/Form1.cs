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

            TimerCallback tm = new TimerCallback(monitoring);
            System.Threading.Timer timer = new System.Threading.Timer(tm, 0, 0, 2000);


        }

        private void monitoring(object obj)
        {
            Thread.Sleep(1000);
            Action action1 = () => current_connections.Items.Clear();
            if (InvokeRequired)
                Invoke(action1);
            else
                action1();

            string[] current_devices = get_current_devices();
            Console.WriteLine(current_devices.Length);

            Action action2 = () => current_connections.Items.AddRange(current_devices);
            if (InvokeRequired)
                Invoke(action2);
            else
                action2();

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
            if (WinPolicies.OpenSubKey("DeviceInstall", true) == null)
            {
                RegistryKey DeviceInstallRestrictions = WinPolicies.CreateSubKey("DeviceInstall").CreateSubKey("Restrictions");
                DeviceInstallRestrictions.SetValue("DenyUnspecified", 1);
            }
            else
            {
                RegistryKey DeviceInstallRestrictions = WinPolicies.OpenSubKey("DeviceInstall", true).OpenSubKey("Restrictions", true);
                DeviceInstallRestrictions.SetValue("DenyUnspecified", 1);
            }


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
            if (WinPolicies.OpenSubKey("DeviceInstall", true) == null)
            {
                RegistryKey DeviceInstallRestrictions = WinPolicies.CreateSubKey("DeviceInstall").CreateSubKey("Restrictions");
                DeviceInstallRestrictions.SetValue("DenyUnspecified", 0);
            }
            else
            {
                RegistryKey DeviceInstallRestrictions = WinPolicies.OpenSubKey("DeviceInstall", true).OpenSubKey("Restrictions", true);
                DeviceInstallRestrictions.SetValue("DenyUnspecified", 0);
            }
        }

        private void add_device_Click(object sender, EventArgs e)
        {

        }
    }
}
