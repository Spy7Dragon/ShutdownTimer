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

namespace ShutdownTimer
{

    public partial class Form1 : Form
    {
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        bool minimizedToTray;

        int delay = 0;
        bool shutdown = false;

        public Time setTime = new Time(99, 99, "AM");                //make setTime usable in multiple scopes

        private ContextMenu trayMenu;

        public Form1()
        {
            InitializeComponent();
            this.Text = "ShutdownTimer";
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                ShowWindow();
            }
            base.WndProc(ref message);
        }

                                            //button used to set the time
        private void btnSetTime_MouseClick(object sender, MouseEventArgs e)
        {
            bool correctFields = true;      //initialize checker for valid input
            int theHour = 0;                //initialize hour

                                            //validate hour
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

            int theMinute = 0;              //initialize minute

                                            //validate minute
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

            
            String theToD = comToD.Text;    //initialize time of day

                                            //set time if fields are correct
                                            //display time
            if (correctFields)
            {
                setTime = new Time(theHour, theMinute, theToD);
                lblShutdownTime.Text = "Shutdown time: " + setTime.ToString();
                //TODO print to file settings.txt

                System.IO.File.WriteAllText("settings.txt", setTime.ToString());
            }


        }

                                            //form load actions
        private void Form1_Load(object sender, EventArgs e)
        {
            comToD.SelectedIndex = 0;        //make AM selected
                                             //display current time
            lblCurrentTime.Text = "Current Time:    " + DateTime.Now.ToString("hh:mm tt");
           
            //load previous settings from text file
            try
            {
                using (TextReader sr = File.OpenText("settings.txt"))
                {
                    char[] delimeterChars = {' ', ':'};
                    string line = sr.ReadLine();
                    string[] bits = line.Split(delimeterChars);
                    int initHour = int.Parse(bits[0]);
                    int initMinute = int.Parse(bits[1]);
                    string initToD = bits[2];
                    setTime = new Time(initHour, initMinute, initToD);
                    lblShutdownTime.Text = "Shutdown time: " + setTime.ToString();
                    sr.Close();
                }
            }
            catch (Exception error)
            {
                //lblShutdownTime.Text = "Unable to read saved time.";
                Console.WriteLine(error.Message);
            }

            //match check boxes with Application Settings
            chBoxRS.Checked = Properties.Settings.Default.startUp;
            chBoxAD.Checked = Properties.Settings.Default.delay;

            //tray menu
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Settings", Settings);
            trayMenu.MenuItems.Add("Exit", OnExit);
            notifyIcon.ContextMenu = trayMenu;
        }

        /**
         * actions for the timer ticks
         * */
        private void timer_Tick(object sender, EventArgs e)
        {
            if (GetLastInputTime() < 1001)
            {
                if (chBoxAD.Checked)
                {
                    delay = 900;
                }
            }

            if (delay > 0)
            delay -= 1;                     //decrement delay
            lblCD.Text = "Current Delay: " + delay;
                                            //display current time
            lblCurrentTime.Text = "Current Time:    " + DateTime.Now.ToString("hh:mm tt");
                                            //if shutdown time then shutdown
            if (setTime.getHour().ToString("00") == DateTime.Now.ToString("hh")
                && setTime.getMinute().ToString("00") == DateTime.Now.ToString("mm")
                && setTime.getToD() == DateTime.Now.ToString("tt"))
            {
                shutdown = true;                //set shutdown to true  
            }

            if (delay < 1 && shutdown)
            {
                Process.Start("shutdown.exe", "-s -f -t 0");
            }
        }

        static uint GetLastInputTime()
        {
            uint idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicks = (uint)Environment.TickCount; //this is the time since the system started

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime; //this is the time that last input was received

                idleTime = envTicks - lastInputTick; //this is the time the system has been idle
            }

            return idleTime;
        }

        /**
         * actions for the run at startup check box change
         * */
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

        /**
         * actions for the delay checkbox
         **/
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

        private void Settings(object sender, EventArgs e)
        {
            ShowWindow();
        }

        void MinimizeToTray()
        {   
            notifyIcon.DoubleClick += new EventHandler(NotifyIconClick);
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(500);
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
            minimizedToTray = true;
        }

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

        void NotifyIconClick(Object sender, System.EventArgs e)
        {
            ShowWindow();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    static public class SingleInstance
    {
        public static readonly int WM_SHOWFIRSTINSTANCE =
            WinApi.RegisterWindowMessage("WM_SHOWFIRSTINSTANCE|{0}", ProgramInfo.AssemblyGuid);
        static Mutex mutex;
        static public bool Start()
        {
            bool onlyInstance = false;
            string mutexName = String.Format("Local\\{0}", ProgramInfo.AssemblyGuid);

            // if you want your app to be limited to a single instance
            // across ALL SESSIONS (multiple users & terminal services), then use the following line instead:
            // string mutexName = String.Format("Global\\{0}", ProgramInfo.AssemblyGuid);

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
