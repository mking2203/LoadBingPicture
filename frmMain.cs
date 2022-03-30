//
// Mark König, 03/2022
//

using System;
using System.Drawing;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Imaging;

using System.Net;


using Microsoft.Win32;

namespace LoadBingPicture
{
    public partial class frmMain : Form
    {
        private bool initial = true;
        private bool closeForm = false;

        private string bingDataPath = string.Empty;
        private string bingThumbsPath = string.Empty;
        private LoadJson bingData;

        private string[] cultures = {"en-AU", "pt-BR", "zh-CN", "de-DE",
                                     "fr-FR", "en-IN", "ja-JP", "en-CA",
                                     "en-NZ", "es-ES", "en-US", "en-GB" };

        private int selectedImage = 0;

        public frmMain()
        {
            InitializeComponent();

            #region set data path

            bingDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                                                 Environment.SpecialFolderOption.Create);

            bingDataPath = bingDataPath + "\\LoadBingPicture";
            if (!Directory.Exists(bingDataPath))
            {
                Directory.CreateDirectory(bingDataPath);
            }
            bingThumbsPath = bingDataPath + "\\Thumbs";
            if (!Directory.Exists(bingThumbsPath))
            {
                Directory.CreateDirectory(bingThumbsPath);
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

            // init json downloader
            bingData = new LoadJson();
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
            addListbox("Start update Bing");

            // download json data   
            bool result = bingData.DownloadJson(bingDataPath, cultures[comboBox1.SelectedIndex]);
            if (result)
            {
                addListbox("Received JSON");

                WebClient client = new WebClient();

                foreach (LoadJson.BingPicture b in bingData.BingPictures)
                {
                    {
                        string url = b.baseurl + "_320x240.jpg";

                        Uri myUri = new Uri(url);
                        string param1 = System.Web.HttpUtility.ParseQueryString(myUri.Query).Get("id");

                        b.thumb = param1;

                        if (!File.Exists(Path.Combine(this.bingThumbsPath, param1)))
                        {
                            addListbox("Try to download thumb " + param1);

                            client.DownloadFile(url,
                                                Path.Combine(this.bingThumbsPath, param1));

                            addListbox("File downloaded");
                        }
                    }
                }
                client.Dispose();
            }
            else
                addListbox("Download JSON failed");

            if (bingData.BingPictures[selectedImage] != null)
            {
                string txt = bingData.BingPictures[selectedImage].title + Environment.NewLine + bingData.BingPictures[selectedImage].copyright;
                if (txt.Length > 63)
                {
                    txt = txt.Substring(0, 59);
                    txt += " ...";
                }

                notifyIcon1.Text = txt;

                string downloadLink = bingData.BingPictures[selectedImage].baseurl;
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

                if (!File.Exists(Path.Combine(this.bingDataPath, param1)))
                {
                    addListbox("Try to download file");

                    WebClient client = new WebClient();
                    client.DownloadFile(downloadLink,
                                    Path.Combine(this.bingDataPath, param1));

                    client.Dispose();

                    addListbox("File for today downloaded");

                }
                else
                    addListbox("File already downloaded");

                return param1;
            }

            return string.Empty;
        }

        private void makeDesktop(string filename, bool force = false)
        {
            addListbox("Set registry to Fit");
            SetDesktop.SetKey(SetDesktop.DesktopStyle.Fit);

            string newFilename = filename.Substring(0, filename.Length - 4);
            newFilename = newFilename + "mod" + filename.Substring(filename.Length - 4);

            if (!File.Exists(Path.Combine(bingDataPath, newFilename)))
            {
                addListbox("Add information to actual image");
                Image image1 = ModPicture.ChangePicture(new Bitmap(Path.Combine(bingDataPath, filename)),
                                                        (int)Properties.Settings.Default["Resolution"],
                                                        bingData.BingPictures[0]
                                                        );

                image1.Save(Path.Combine(bingDataPath, newFilename), ImageFormat.Jpeg);
                addListbox("Saved new actual image");

                image1.Dispose();
            }
            else
            {
                addListbox("Actual file is up to date");
            }

            SetDesktop.DisplayPicture(Path.Combine(bingDataPath, newFilename), true);
            addListbox("Changed desktop");

            pictureBox1.Image = null;
            Application.DoEvents();

            Image image = new Bitmap(Path.Combine(bingDataPath, filename));
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = image;

            txtTitle.Text = bingData.BingPictures[0].title;
            txtDescription.Text = bingData.BingPictures[0].description;
            txtCopyright.Text = bingData.BingPictures[0].copyright;


            foreach (string f in Directory.GetFiles(bingDataPath, "*.jpg"))
            {
                if (f != Path.Combine(bingDataPath, filename) && f != Path.Combine(bingDataPath, newFilename))
                {
                    try
                    {
                        File.Delete(f);
                    }
                    catch { }
                }
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
            if (MessageBox.Show("Do you really want to stop the Bing picture loader?",
                                "Stop the Bing picture loader",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                closeForm = true;
                Application.Exit();
            }
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            txtUpdate.Text = "Next update in " + (60 - DateTime.Now.Minute).ToString() + " min";

            if (initial)
            {
                if (CheckInternet.IsInternetConnected())
                {
                    string name = downloadData();
                    if (name != string.Empty)
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
                    if (name != string.Empty)
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
            if (name != string.Empty)
                makeDesktop(name);

            timer1.Enabled = true;
        }

        private void chkInfo_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["ShowDescription"] = Convert.ToInt32(chkInfo.Checked);
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

        private void btnMinus_Click(object sender, EventArgs e)
        {
            if (selectedImage > 0)
            {
                selectedImage--;

                pictureBox1.Image = null;
                Application.DoEvents();

                txtTitle.Text = bingData.BingPictures[selectedImage].title;
                txtDescription.Text = bingData.BingPictures[selectedImage].description;
                txtCopyright.Text = bingData.BingPictures[selectedImage].copyright;

                Image image = new Bitmap(Path.Combine(bingThumbsPath, bingData.BingPictures[selectedImage].thumb));
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = image;
            }
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            if (selectedImage < 6)
            {
                selectedImage++;

                pictureBox1.Image = null;
                Application.DoEvents();

                txtTitle.Text = bingData.BingPictures[selectedImage].title;
                txtDescription.Text = bingData.BingPictures[selectedImage].description;
                txtCopyright.Text = bingData.BingPictures[selectedImage].copyright;

                Image image = new Bitmap(Path.Combine(bingThumbsPath, bingData.BingPictures[selectedImage].thumb));
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = image;
            }
        }
    }
}
