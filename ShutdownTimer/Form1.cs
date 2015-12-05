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

namespace ShutdownTimer
{

    
    public partial class Form1 : Form
    {

        int delay = 0;
        bool shutdown = false;

        public Time setTime = new Time(99, 99, "AM");                //make setTime usable in multiple scopes

        
        public Form1()
        {
            InitializeComponent();
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
        }

        /**
         * actions for the timer ticks
         * */
        private void timer_Tick(object sender, EventArgs e)
        {
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
                Process.Start("shutdown", "/s /t 0");
            }
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
            if (chBoxAD.Checked)
            {
                delay = 900;
            }
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
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon.Visible = false;
            }
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
