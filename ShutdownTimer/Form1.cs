///<summary>
///Project: Shutdown Timer
///Author: Branden Huggins
///Date: 12/22/2015
///Version: 00001
///</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Reflection;

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

        struct LASTINPUTINFO ///<summary>Describes the last input value from the system</summary>
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        bool minimizedToTray;///<value>tells whether or not the program is minimized to the tray</value>

        int delay = 0; ///<value>variable for the delay time</value>
        bool shutdown = false; ///<value>tells whether or not the system should be shutdown</value>

        public Time setTime = new Time(99, 99, "AM"); ///<value>make setTime usable in multiple scopes, set time is the time in which the system will start shutting down</value>

        private ContextMenu trayMenu; ///<value>the tray menu</value>

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
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                ShowWindow();
            }
            base.WndProc(ref message);
        }

        ///button used to set the time
        private void btnSetTime_MouseClick(object sender, MouseEventArgs e)
        {
            bool correctFields = true;      //initialize checker for valid input
            int theHour = 0;                //initialize hour

            ///validate hour
            if (Int32.TryParse(txtHour.Text, out theHour))
            {
                if (theHour < 1 || theHour > 12)
                {
                    lblStatus.Text = "Invalid Hour";
                    correctFields = false;
                }
            }
            else
            {
                lblStatus.Text = "Invalid Hour";
                correctFields = false;
            }

            int theMinute = 0;              ///initialize minute

            ///validate minute
            if (correctFields)
            {
                if (Int32.TryParse(txtMin.Text, out theMinute))
                {
                    if (theMinute < 0 || theMinute > 60)
                    {
                        lblStatus.Text = "Invalid Minute";
                        correctFields = false;
                    }
                }
                else
                {
                    lblStatus.Text = "Invalid Minute";
                    correctFields = false;
                }
            }


            String theToD = comToD.Text;    ///initialize time of day

            ///set time if fields are correct
            ///display time
            if (correctFields)
            {
                setTime = new Time(theHour, theMinute, theToD);
                lblShutdownTime.Text = "Shutdown time: " + setTime.ToString();

                System.IO.File.WriteAllText("settings.txt", setTime.ToString());
            }


        }

        ///form load actions
        private void Form1_Load(object sender, EventArgs e)
        {
            comDelay.SelectedIndex = 2;
            comToD.SelectedIndex = 0;        ///make AM selected
            ///display current time
            lblCurrentTime.Text = "Current Time: " + DateTime.Now.ToString("hh:mm tt");

            ///load previous settings from text file
            try
            {
                using (TextReader sr = File.OpenText("settings.txt"))
                {
                    char[] delimeterChars = { ' ', ':' };
                    string line = sr.ReadLine();
                    string[] bits = line.Split(delimeterChars);
                    int initHour = int.Parse(bits[0]);
                    int initMinute = int.Parse(bits[1]);
                    string initToD = bits[2];
                    setTime = new Time(initHour, initMinute, initToD);
                    lblShutdownTime.Text = "Shutdown time: " + setTime.ToString();
                    txtHour.Text = setTime.getHour().ToString("00");
                    txtMin.Text = setTime.getMinute().ToString("00");
                    comToD.Text = setTime.getToD();
                    sr.Close();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

            ///match check boxes with Application Settings
            chBoxRS.Checked = Properties.Settings.Default.startUp;
            chBoxAD.Checked = Properties.Settings.Default.delay;
            comDelay.Text = Properties.Settings.Default.delayTime;
            ///tray menu
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Settings", Settings);
            trayMenu.MenuItems.Add("Exit", OnExit);
            notifyIcon.ContextMenu = trayMenu;
        }

        
        /// <summary>
        /// actions for the timer ticks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (delay > 0)
                delay -= 1;                     ///decrement delay

            ///if moved in the last second and activity delay then set delay
            if (GetLastInputTime() < 1001)
            {
                if (chBoxAD.Checked)
                {
                    if (Int32.TryParse(comDelay.Text, out delay))
                    {
                        delay = Int32.Parse(comDelay.Text) * 60;
                    }
                    else
                    {
                        delay = 900;
                    }
                }
            }

            lblCD.Text = "Current Delay: " + delay + " secs";
            ///display current time
            lblCurrentTime.Text = "Current Time: " + DateTime.Now.ToString("hh:mm tt");
            ///if shutdown time then shutdown
            if (setTime.getHour().ToString("00") == DateTime.Now.ToString("hh")
                && setTime.getMinute().ToString("00") == DateTime.Now.ToString("mm")
                && setTime.getToD() == DateTime.Now.ToString("tt"))
            {
                shutdown = true;                ///set shutdown to true  
            }

            ///if delay 0 and shutdown set, then actually shutdown
            if (delay < 1 && shutdown)
            {
                Process.Start("shutdown.exe", "-s -f -t 0");
            }
        }


        /// <summary>
        /// returns the time the system has been idle
        /// </summary>
        /// <returns></returns>
        static uint GetLastInputTime()
        {
            uint idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicks = (uint)Environment.TickCount; ///this is the time since the system started

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime; ///this is the time that last input was received

                idleTime = envTicks - lastInputTick; ///this is the time the system has been idle
            }

            return idleTime;
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
                reg.SetValue("ShutdownTimer", Application.ExecutablePath.ToString());
                Properties.Settings.Default.startUp = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.startUp = false;
                Properties.Settings.Default.Save();
                reg.DeleteValue("ShutdownTimer", false);
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
                delay = 0;                          //changes delay so that the program can mmediately start a delayed shutdown
            }
        }

        /// <summary>
        /// minimizing and normalize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                MinimizeToTray();
            }
            else if (FormWindowState.Normal == this.WindowState)
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
        void MinimizeToTray()
        {
            notifyIcon.DoubleClick += new EventHandler(NotifyIconClick);
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(500);
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            minimizedToTray = true;
        }

        /// <summary>
        /// shows the window and gets rid of the tray
        /// </summary>
        public void ShowWindow()
        {
            if (minimizedToTray)
            {
                notifyIcon.Visible = false;
                this.Show();
                this.WindowState = FormWindowState.Normal;
                minimizedToTray = false;
            }
            else
            {
                WinApi.ShowToFront(this.Handle);
            }
        }

        /// <summary>
        /// action of clicking the notify icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NotifyIconClick(Object sender, System.EventArgs e)
        {
            ShowWindow();
        }


        /// <summary>
        /// action for exiting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, EventArgs e)
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
            Properties.Settings.Default.delayTime = comDelay.Text;
            Properties.Settings.Default.Save();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    static public class SingleInstance
    {
        public static readonly int WM_SHOWFIRSTINSTANCE =
            WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
        static Mutex mutex;
        static public bool Start()
        {
            bool onlyInstance = false;
            string mutexName = String.Format("Local\\{0}", ProgramInfo.AssemblyGuid);

            mutex = new Mutex(true, mutexName, out onlyInstance);
            return onlyInstance;
        }
        static public void ShowFirstInstance()
        {
            WinApi.PostMessage(
                (IntPtr)WinApi.HWND_BROADCAST,
                WM_SHOWFIRSTINSTANCE,
                IntPtr.Zero,
                IntPtr.Zero);
        }
        static public void Stop()
        {
            mutex.ReleaseMutex();
        }
    }

    static public class WinApi
    {
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public static int RegisterWindowMessage(string format, params object[] args)
        {
            string message = String.Format(format, args);
            return RegisterWindowMessage(message);
        }

        public const int HWND_BROADCAST = 0xffff;
        public const int SW_SHOWNORMAL = 1;

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImportAttribute("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImportAttribute("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void ShowToFront(IntPtr window)
        {
            ShowWindow(window, SW_SHOWNORMAL);
            SetForegroundWindow(window);
        }
    }

    /// <summary>
    /// gets information about the program
    /// </summary>
    static public class ProgramInfo
    {
        static public string AssemblyGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
                if (attributes.Length == 0)
                {
                    return String.Empty;
                }
                return ((System.Runtime.InteropServices.GuidAttribute)attributes[0]).Value;
            }
        }
        static public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
            }
        }
    }
}
