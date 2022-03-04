using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;

using System.Net;
using Newtonsoft.Json;

using Microsoft.Win32;

namespace LoadBingPicture
{
    public partial class frmMain : Form
    {
        #region windows stuff

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, uint fWinIni);

        private const uint SPI_SETDESKWALLPAPER = 0x14;
        private const uint SPIF_UPDATEINIFILE = 0x1;
        private const uint SPIF_SENDWININICHANGE = 0x2;


        // for the I-Net check
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern int InternetAttemptConnect(uint res);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetConnectedState(out int flags, int reserved);

        #endregion

        private string title;
        private string copyright;

        private bool initial = true;
        private bool closeForm = false;

        private string bingData = string.Empty;

        private string[] cultures = {"en-AU", "pt-BR", "zh-CN", "de-DE",
                                     "fr-FR", "en-IN", "ja-JP", "en-CA",
                                     "en-NZ", "es-ES", "en-US", "en-GB" }; 

        public enum Style : int
        {
            Fill,
            Fit,
            Span,
            Stretch,
            Tile,
            Center
        }

        public frmMain()
        {
            InitializeComponent();

            #region set data path
            bingData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                                                 Environment.SpecialFolderOption.Create);

            bingData = bingData + "\\LoadBingPicture";
            if (!Directory.Exists(bingData))
            {
                Directory.CreateDirectory(bingData);
            }
            #endregion

            #region set context menu
            ContextMenu m_ContextMenu = new ContextMenu();
            MenuItem menuEntryExit = new MenuItem("Close", new EventHandler(ExitApplicationHandler));
            MenuItem menuEntryShow = new MenuItem("Settings", new EventHandler(ShowMainFormHandler));

            m_ContextMenu.MenuItems.Add(menuEntryShow);
            m_ContextMenu.MenuItems.Add(menuEntryExit);

            notifyIcon1.ContextMenu = m_ContextMenu;
            #endregion

            comboBox1.Items.Add("Australien");
            comboBox1.Items.Add("Brasilien");
            comboBox1.Items.Add("China");
            comboBox1.Items.Add("Deutschland");
            comboBox1.Items.Add("Frankreich");
            comboBox1.Items.Add("Indien");
            comboBox1.Items.Add("Japan");
            comboBox1.Items.Add("Kanada");
            comboBox1.Items.Add("Neuseeland");
            comboBox1.Items.Add("Spanisch");
            comboBox1.Items.Add("Vereinigte Staaten");
            comboBox1.Items.Add("Vereinigtes Königreich");

            comboBox1.SelectedIndex = Convert.ToInt32(Properties.Settings.Default["Region"]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // restore settings
            switch (Properties.Settings.Default["Resolution"])
            {
                case 0:
                    selResolution00.Checked = true;
                    break;
                case 1:
                    selResolution01.Checked = true;
                    break;
                case 2:
                    selResolution02.Checked = true;
                    break;
                default:
                    selResolution01.Checked = true;
                    break;
            }

            chkInfo.Checked = Convert.ToBoolean(Properties.Settings.Default["ShowDescription"]);

            // start timer
            timer1.Enabled = true;
            timer1_Tick(this, new EventArgs());
        }

        private void addListbox(string txt)
        {
            listBox1.Items.Add(DateTime.Now.ToLongTimeString() + " " + txt);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private string downloadData()
        {
            addListbox("----------------");
            addListbox("Search new image");

            WebClient client = new WebClient();
            var webData = client.DownloadData("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=" + cultures[comboBox1.SelectedIndex]);
            string webString = Encoding.UTF8.GetString(webData);

            addListbox("Received JSON");

            // save JSON
            using (StreamWriter writetext = new StreamWriter(bingData + "\\bing.json"))
            {
                writetext.Write(webString);
            }

            dynamic stuff = JsonConvert.DeserializeObject(webString);

            title = (string)stuff.images[0].title;
            copyright = (string)stuff.images[0].copyright;

            notifyIcon1.Text = DateTime.Now.ToShortDateString() + Environment.NewLine + title;

            string downloadLink = "https://www.bing.com" + stuff.images[0].urlbase;
            downloadLink += "_";

            switch (Properties.Settings.Default["Resolution"])
            {
                case 0:
                    downloadLink += "1366x768.jpg";
                    break;
                default:
                case 1:
                    downloadLink += "1920x1080.jpg";
                    break;
                case 2:
                    downloadLink += "UHD.jpg";
                    break;
            }

            Uri myUri = new Uri(downloadLink);
            string param1 = System.Web.HttpUtility.ParseQueryString(myUri.Query).Get("id");

            if (!File.Exists(Path.Combine(bingData, param1)))
            {
                addListbox("Try to download file");

                client.DownloadFile(downloadLink,
                                Path.Combine(bingData, param1));

                addListbox("File for today downloaded");

            }
            else
                addListbox("File already downloaded");

            client.Dispose();
            return param1;
        }

        private void makeDesktop(string filename, bool force = false)
        {
            addListbox("Set registry to Stretch");
            SetKey(Style.Stretch);

            string newFilename = filename.Substring(0, filename.Length - 4);
            newFilename = newFilename + "mod" + filename.Substring(filename.Length - 4);

            if (!File.Exists(Path.Combine(bingData, newFilename)))
            {
                addListbox("Add information to actual image");

                Image image1 = new Bitmap(Path.Combine(bingData, filename));
                Image image2 = new Bitmap(Properties.Resources.back75);

                int sz = image1.Height;
                int back = sz / 10;

                Font stringFont = new Font("Arial", 16);

                switch (Properties.Settings.Default["Resolution"])
                {
                    case 0:
                        stringFont = new Font("Arial", 11);
                        break;
                    default:
                    case 1:
                        //
                        break;
                    case 2:
                        stringFont = new Font("Arial", 48);
                        break;
                }

                if (chkInfo.Checked)
                {
                    using (Graphics gr = Graphics.FromImage(image1))
                    {
                        SizeF l1 = gr.MeasureString(title, stringFont);
                        SizeF l2 = gr.MeasureString(copyright, stringFont);

                        int yValue = (int)l1.Height / 2;

                        float l = l1.Width;
                        if (l2.Width > l) l = l2.Width;

                        int x = image1.Width - (int)l - 50;

                        gr.DrawImage(image2,
                            new Rectangle(new Point(x, back * 8), new Size((int)l + 20, (3 * yValue) + (2 * (int)l1.Height))));

                        gr.DrawString(title,
                            stringFont,
                            new SolidBrush(Color.White),
                            new PointF(x + 10, back * 8 + yValue));

                        gr.DrawString(copyright,
                            stringFont,
                            new SolidBrush(Color.White),
                            new PointF(x + 10, back * 8 + yValue + (int)l1.Height + yValue));
                    }
                }

                image1.Save(Path.Combine(bingData, newFilename), ImageFormat.Jpeg);
                addListbox("Saved new actual image");

                image1.Dispose();
                image2.Dispose();
            }
            else
            {
                addListbox("Actual file is up to date");
            }

            DisplayPicture(Path.Combine(bingData, newFilename), true);
            addListbox("Changed desktop");

            pictureBox1.Image = null;
            Application.DoEvents();

            Image image = new Bitmap(Path.Combine(bingData, filename));
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = image;

            foreach (string f in Directory.GetFiles(bingData,"*.jpg"))
            {
                if(f != Path.Combine(bingData, filename) && f != Path.Combine(bingData, newFilename))
                {
                    try
                    {
                        File.Delete(f);
                    }
                    catch { }
                }
            }
        }

        private void DisplayPicture(string file_name, bool update_registry)
        {
            try
            {
                // If we should update the registry,
                // set the appropriate flags.
                uint flags = 0;
                if (update_registry)
                    flags = SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE;

                // Set the desktop background to this file.
                if (!SystemParametersInfo(SPI_SETDESKWALLPAPER,
                    0, file_name, flags))
                {
                    MessageBox.Show("SystemParametersInfo failed.",
                        "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying picture " +
                    file_name + ".\n" + ex.Message,
                    "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void SetKey(Style style)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

            if (style == Style.Fill)
            {
                key.SetValue(@"WallpaperStyle", 10.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Fit)
            {
                key.SetValue(@"WallpaperStyle", 6.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Span) // Windows 8 or newer only!
            {
                key.SetValue(@"WallpaperStyle", 22.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Stretch)
            {
                key.SetValue(@"WallpaperStyle", 2.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
            if (style == Style.Tile)
            {
                key.SetValue(@"WallpaperStyle", 0.ToString());
                key.SetValue(@"TileWallpaper", 1.ToString());
            }
            if (style == Style.Center)
            {
                key.SetValue(@"WallpaperStyle", 0.ToString());
                key.SetValue(@"TileWallpaper", 0.ToString());
            }
        }

        #region form resize / closing handling
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.WindowsShutDown) || (closeForm))
            {
                // other
            }
            else
            {
                Hide();
                e.Cancel = true;
            }
        }

        private void ShowMainFormHandler(object sender, EventArgs e)
        {
            // aktivate form and set position midle of screen
            Show();

            WindowState = FormWindowState.Normal;
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width / 2) - (this.Width / 2),
                                      (Screen.PrimaryScreen.Bounds.Height / 2) - (this.Height / 2));
        }

        private void ExitApplicationHandler(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to close this?", "Stop the this", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                closeForm = true;
                Application.Exit();
            }
        }

        #endregion

        private static int ERROR_SUCCESS = 0;
        public static bool IsInternetConnected()
        {
            int dwConnectionFlags = 0;
            if (!InternetGetConnectedState(out dwConnectionFlags, 0))
                return false;

            if (InternetAttemptConnect(0) != ERROR_SUCCESS)
                return false;

            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtUpdate.Text = "Next update in " + (60 - DateTime.Now.Minute).ToString() + " min";

            if (initial)
            {
                if (IsInternetConnected())
                {
                    string name = downloadData();
                    makeDesktop(name);
                    initial = false;
                }
            }
            else
            {
                // once per hour we will check
                if (DateTime.Now.Minute == 00)
                {
                    string name = downloadData();
                    makeDesktop(name);
                }
            }
        }

        private void selResolution_CheckedChanged(object sender, EventArgs e)
        {
            if (selResolution00.Checked)
                Properties.Settings.Default["Resolution"] = 0;
            if (selResolution01.Checked)
                Properties.Settings.Default["Resolution"] = 1;
            if (selResolution02.Checked)
                Properties.Settings.Default["Resolution"] = 2;

            Properties.Settings.Default.Save();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            string name = downloadData();
            makeDesktop(name);

            timer1.Enabled = true;
        }

        private void chkInfo_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["ShowDescription"] =Convert.ToInt32(chkInfo.Checked);
            Properties.Settings.Default.Save();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                Properties.Settings.Default["Region"] = comboBox1.SelectedIndex;
                Properties.Settings.Default.Save();
            }
        }
    }
}
