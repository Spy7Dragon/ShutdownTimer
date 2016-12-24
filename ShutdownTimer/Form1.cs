///<summary>
///Project: Shutdown Timer
///Author: Branden Huggins
///Date: 12/24/2016
///Version: 00002
///</summary>

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using ShutdownTimer.Properties;

///<summary>
///ShutdownTimer: Controls when the computer will be shutdown based on time and settings
///</summary>
namespace ShutdownTimer
{

    /// <summary>
    /// The main form for the program
    /// </summary>
    public partial class Form1 : Form
    {
        [StructLayout(LayoutKind.Sequential)] ///<remarks>The members of the object are laid out sequentially, in the order in which they appear when exported to unmanaged memory. The members are laid out according to the packing specified in StructLayoutAttribute.Pack, and can be noncontiguous.</remarks>
        private struct Lastinputinfo ///<summary>Describes the last input value from the system</summary>
        {
            private static readonly int SizeOf = Marshal.SizeOf(typeof(Lastinputinfo));

            [MarshalAs(UnmanagedType.U4)]
            public uint cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref Lastinputinfo plii);

        private bool m_minimized_to_tray;///<value>tells whether or not the program is minimized to the tray</value>
        private int m_delay; ///<value>variable for the delay time</value>
        private bool m_shutdown; ///<value>tells whether or not the system should be shutdown</value>

        public Time SetTime = new Time(99, 99, "AM"); ///<value>make setTime usable in multiple scopes, set time is the time in which the system will start shutting down</value>

        private ContextMenu m_tray_menu;

        ///<value>the tray menu</value>
        ///
        public string Path { get; } = "C:\\Users\\Public\\STsettings.txt";

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// shows the form if the program is already running.
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.sr_wm_showfirstinstance)
            {
                ShowWindow();
            }
            base.WndProc(ref message);
        }

        ///button used to set the time
        private void btnSetTime_MouseClick(object sender, MouseEventArgs e)
        {
            bool correct_fields = true;      //initialize checker for valid input
            int the_hour;                //initialize hour

            // validate hour
            if (int.TryParse(txtHour.Text, out the_hour))
            {
                if (the_hour < 1 || the_hour > 12)
                {
                    lblStatus.Text = Resources.Invalid_Hour;
                    correct_fields = false;
                }
            }
            else
            {
                lblStatus.Text = Resources.Invalid_Hour;
                correct_fields = false;
            }

            int the_minute = 0;              // initialize minute

            // validate minute
            if (correct_fields)
            {
                if (int.TryParse(txtMin.Text, out the_minute))
                {
                    if (the_minute < 0 || the_minute > 60)
                    {
                        lblStatus.Text = Resources.Invalid_Minute;
                        correct_fields = false;
                    }
                }
                else
                {
                    lblStatus.Text = Resources.Invalid_Minute;
                    correct_fields = false;
                }
            }


            string the_to_d = comToD.Text;    //initialize time of day

            //set time if fields are correct
            //display time
            if (correct_fields)
            {
                SetTime = new Time(the_hour, the_minute, the_to_d);
                lblShutdownTime.Text = Resources.Shutdown_Time + SetTime;
                // Write to file.
                string[] lines = File.ReadAllLines(Path);
                if (lines.Length > 0)
                {
                    lines[0] = SetTime.ToString();
                    File.WriteAllLines(Path, lines);
                }
                else
                {
                    string[] new_lines = {SetTime.ToString(), comDelay.Text};
                    File.AppendAllLines(Path, new_lines);
                }
                
            }
        }

        // form load actions
        private void Form1_Load(object sender, EventArgs e)
        {
            comToD.SelectedIndex = 0;        //make AM selected
            //display current time
            lblCurrentTime.Text = Resources.Current_Time + DateTime.Now.ToString("hh:mm tt");

            //load previous settings from text file
            try
            {
                using (TextReader sr = File.OpenText(Path))
                {
                    // Read time.
                    char[] delimeter_chars = { ' ', ':' };
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        string[] bits = line.Split(delimeter_chars);
                        int init_hour = int.Parse(bits[0]);
                        int init_minute = int.Parse(bits[1]);
                        string init_to_d = bits[2];
                        SetTime = new Time(init_hour, init_minute, init_to_d);
                    }
                    lblShutdownTime.Text = Resources.Shutdown_Time + SetTime;
                    txtHour.Text = SetTime.GetHour().ToString("00");
                    txtMin.Text = SetTime.GetMinute().ToString("00");
                    comToD.Text = SetTime.GetToD();
                    // Read saved delay.
                    line = sr.ReadLine();
                    if (line != null)
                    {
                        comDelay.Text = int.Parse(line).ToString();
                    }
                    sr.Close();
                }

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

            //match check boxes with Application Settings
            chBoxRS.Checked = Properties.Settings.Default.startUp;
            chBoxAD.Checked = Properties.Settings.Default.delay;
            //tray menu
            m_tray_menu = new ContextMenu();
            m_tray_menu.MenuItems.Add("Settings", Settings);
            m_tray_menu.MenuItems.Add("Exit", OnExit);
            notifyIcon.ContextMenu = m_tray_menu;

            // Choose selected indices.
            switch (int.Parse(comDelay.Text))
            {
                case 5:
                    comDelay.SelectedIndex = 0;
                    break;
                case 10:
                    comDelay.SelectedIndex = 1;
                    break;
                case 15:
                    comDelay.SelectedIndex = 2;
                    break;
                case 30:
                    comDelay.SelectedIndex = 3;
                    break;
                case 60:
                    comDelay.SelectedIndex = 4;
                    break;
                default:
                    comDelay.SelectedIndex = 2;
                    break;
            }
        }

        
        /// <summary>
        /// actions for the timer ticks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (m_delay > 0)
                m_delay -= 1;                     //decrement delay

            //if moved in the last second and activity delay then set delay
            if (GetLastInputTime() < 1001)
            {
                if (chBoxAD.Checked)
                {
                    m_delay = int.Parse(comDelay.Text) * 60;
                }
            }

            lblCD.Text = Resources.Current_Delay + m_delay + Resources.Secs;
            //display current time
            lblCurrentTime.Text = Resources.Current_Time + DateTime.Now.ToString("hh:mm tt");
            // if shutdown time then shutdown
            if (SetTime.GetHour().ToString("00") == DateTime.Now.ToString("hh")
                && SetTime.GetMinute().ToString("00") == DateTime.Now.ToString("mm")
                && SetTime.GetToD() == DateTime.Now.ToString("tt"))
            {
                m_shutdown = true;                // set shutdown to true  
            }

            // if delay 0 and shutdown set, then actually shutdown
            if (m_delay < 1 && m_shutdown)
            {
                Process.Start("shutdown.exe", "-s -t 00");
                Application.Exit();
            }
        }


        /// <summary>
        /// returns the time the system has been idle
        /// </summary>
        /// <returns></returns>
        private static uint GetLastInputTime()
        {
            uint idle_time = 0;
            Lastinputinfo last_input_info = new Lastinputinfo();
            last_input_info.cbSize = (uint)Marshal.SizeOf(last_input_info);
            last_input_info.dwTime = 0;

            uint env_ticks = (uint)Environment.TickCount; // this is the time since the system started

            if (GetLastInputInfo(ref last_input_info))
            {
                uint last_input_tick = last_input_info.dwTime; // this is the time that last input was received

                idle_time = env_ticks - last_input_tick; // this is the time the system has been idle
            }

            return idle_time;
        }

        /// <summary>
        /// actions for the run at startup check box change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chBoxRS_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey
                   ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (chBoxRS.Checked)
            {
                reg?.SetValue("ShutdownTimer", Application.ExecutablePath);
                Properties.Settings.Default.startUp = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.startUp = false;
                Properties.Settings.Default.Save();
                reg?.DeleteValue("ShutdownTimer", false);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        /// actions for the delay checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chBoxAD_CheckedChanged(object sender, EventArgs e)
        {
            if (chBoxAD.Checked)
            {
                Properties.Settings.Default.delay = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.delay = false;
                Properties.Settings.Default.Save();
                m_delay = 0;                          //changes delay so that the program can mmediately start a delayed shutdown
            }
        }

        /// <summary>
        /// minimizing and normalize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                MinimizeToTray();
            }
            else if (FormWindowState.Normal == WindowState)
            {
                ShowWindow();
            }
        }

        /// <summary>
        /// Settings options shows the window again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings(object sender, EventArgs e)
        {
            ShowWindow();
        }

        /// <summary>
        /// minimize to tray and setups the tray at the bottom
        /// </summary>
        private void MinimizeToTray()
        {
            notifyIcon.DoubleClick += NotifyIconClick;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(500);
            WindowState = FormWindowState.Minimized;
            Hide();
            m_minimized_to_tray = true;
        }

        /// <summary>
        /// shows the window and gets rid of the tray
        /// </summary>
        public void ShowWindow()
        {
            if (m_minimized_to_tray)
            {
                notifyIcon.Visible = false;
                Show();
                WindowState = FormWindowState.Normal;
                m_minimized_to_tray = false;
            }
            else
            {
                WinApi.ShowToFront(Handle);
            }
        }

        /// <summary>
        /// action of clicking the notify icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIconClick(object sender, EventArgs e)
        {
            ShowWindow();
        }


        /// <summary>
        /// action for exiting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// actions for setting the delay duration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comDelay_SelectedIndexChanged(object sender, EventArgs e)
        {
            int temp_int;
            bool valid_int = int.TryParse(comDelay.Text, out temp_int);
            m_delay = temp_int * 60;
            // Update file.
            if (valid_int)
            {
                var lines = File.ReadAllLines(Path);
                if (lines.Length > 1)
                {
                    lines[1] = temp_int.ToString();
                    File.WriteAllLines(Path, lines);
                }
                else
                {
                    string[] new_lines = {temp_int.ToString()};
                    File.AppendAllLines(Path, new_lines);
                }  
            }
        }
    }

    public static class WinApi
    {
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = string.Format(format, args);
            return RegisterWindowMessage(message);
        }

        public const int hwnd_broadcast = 0xffff;
        public const int sw_shownormal = 1;

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ShowToFront(IntPtr window)
        {
            ShowWindow(window, sw_shownormal);
            SetForegroundWindow(window);
        }
    }

    /// <summary>
    /// gets information about the program
    /// </summary>
    public static class ProgramInfo
    {
        public static string AssemblyGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(GuidAttribute), false);
                if (attributes.Length == 0)
                {
                    return string.Empty;
                }
                return ((GuidAttribute)attributes[0]).Value;
            }
        }
        public static string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute title_attribute = (AssemblyTitleAttribute)attributes[0];
                    if (title_attribute.Title != "")
                    {
                        return title_attribute.Title;
                    }
                }
                return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
            }
        }
    }
}
