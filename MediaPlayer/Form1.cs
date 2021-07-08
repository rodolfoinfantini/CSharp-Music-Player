using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio;

namespace MediaPlayer
{
    public partial class Form1 : Form
    {
        WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();
        string path;
        bool isMoving = false;
        bool loop = false;
        int startVolume = 50;
        int tempVolume;
        List<int> orderArray = new List<int>();
        bool shuffle = false;
        int control = 0;
        int[] orderShuffle;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            player.settings.volume = 0;
            tempVolume = startVolume;
            string user = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            path = user + "\\Music";
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] FolderFiles = d.GetFiles("*.mp3");
            string str = "";
            foreach (FileInfo file in FolderFiles)
            {
                str = file.Name;
                listBox1.Items.Add(str.Replace(".mp3", ""));
            }
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                orderArray.Add(i);
            }
            Random rnd = new Random();
            int[] shuffle = orderArray.OrderBy(x => rnd.Next()).ToArray();
            orderShuffle = shuffle;
            if(listBox1.Items.Count == 0)
            {
                DialogResult result = MessageBox.Show("There is no music in your musics folder!", "Attention!");
                if (result == DialogResult.OK)
                    this.Close();
            }
            else
            {
                groupBox1.Text = Convert.ToString(listBox1.Items.Count) + " Songs";
                listBox1.SelectedIndex = 0;
                label2.Text = Convert.ToString(listBox1.SelectedItem);
                player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                timer2.Enabled = true;
            }
            //label1.Text = getDuration("C:\\Users\\rodol\\Music\\Sounds\\cavalo.wav");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(player.playState).Equals("wmppsPaused"))
            {
                player.controls.play();
                button1.BackgroundImage = MediaPlayer.Properties.Resources.pause;
            }
            else
            {
                player.controls.pause();
                button1.BackgroundImage = MediaPlayer.Properties.Resources.play;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*string[] filePaths = Directory.GetFiles(@"C:\Users\rodol\Music", "*.mp3", SearchOption.TopDirectoryOnly);
            
            for (int i = 0; i < filePaths.Length; i++)
            {
                listBox1.Items.Add(filePaths[i]);
            }*/
            
            



            /*

            OpenFileDialog opnfl = new OpenFileDialog();
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            opnfl.Filter = "Audio Files|*.MP3";
            if(opnfl.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                files = opnfl.SafeFileNames;
                paths = opnfl.FileNames;
                for(int i = 0; i < files.Length; i++)
                {
                    listBox1.Items.Add(files[i]);
                }
            }*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!Convert.ToString(player.playState).Equals("wmppsUndefined")){
                    label6.Text = Convert.ToString(player.currentMedia.durationString);
                    label7.Text = Convert.ToString(player.controls.currentPositionString);

                    if (isMoving)
                    {

                    }
                    else
                    {
                        trackBar2.Maximum = Convert.ToInt32(player.currentMedia.duration);
                        trackBar2.Value = Convert.ToInt32(player.controls.currentPosition);
                    }
                    
                    //if((Convert.ToInt32(player.controls.currentPosition) != 0) && (Convert.ToInt32(player.currentMedia.duration) == Convert.ToInt32(player.controls.currentPosition)))
                    if(Convert.ToString(player.playState).Equals("wmppsStopped"))
                    {
                        if(listBox1.SelectedIndex + 1 == listBox1.Items.Count)
                        {
                            if (loop)
                            {
                                if (shuffle)
                                {
                                    if (control >= orderShuffle.Length)
                                    {
                                        control = 0;
                                        listBox1.SelectedIndex = orderShuffle[control];
                                        label2.Text = Convert.ToString(listBox1.SelectedItem);
                                        player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                                        control++;
                                    }
                                    else
                                    {
                                        listBox1.SelectedIndex = orderShuffle[control];
                                        label2.Text = Convert.ToString(listBox1.SelectedItem);
                                        player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                                        control++;
                                    }
                                }
                                else
                                {
                                    listBox1.SelectedIndex = 0;
                                    label2.Text = Convert.ToString(listBox1.SelectedItem);
                                    player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                                }
                            }
                            else
                            {

                                listBox1.SelectedIndex = orderShuffle[control];
                                label2.Text = Convert.ToString(listBox1.SelectedItem);
                                player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                                control++;
                            }
                        }
                        else
                        {
                            if (shuffle)
                            {
                                if(control >= orderShuffle.Length)
                                {
                                    if (loop)
                                    {
                                        control = 0;
                                        listBox1.SelectedIndex = orderShuffle[control];
                                        label2.Text = Convert.ToString(listBox1.SelectedItem);
                                        player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                                        control++;
                                    }
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    listBox1.SelectedIndex = orderShuffle[control];
                                    label2.Text = Convert.ToString(listBox1.SelectedItem);
                                    player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                                    control++;
                                }
                            }
                            else
                            {
                                listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
                                label2.Text = Convert.ToString(listBox1.SelectedItem);
                                player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                            }
                        }
                    }
                }
                else
                {
                    label6.Text = "00:00";
                    label7.Text = "00:00";
                    
                }
            }
            catch
            {

            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            player.settings.volume = trackBar1.Value;
            
            if (trackBar1.Value >= 90)
                pictureBox2.BackgroundImage = MediaPlayer.Properties.Resources.sound_3;
            else if (trackBar1.Value >= 50 && trackBar1.Value < 90)
                pictureBox2.BackgroundImage = MediaPlayer.Properties.Resources.sound_2;
            else if (trackBar1.Value > 0 && trackBar1.Value < 50)
                pictureBox2.BackgroundImage = MediaPlayer.Properties.Resources.sound_1;
            else if (trackBar1.Value == 0)
                pictureBox2.BackgroundImage = MediaPlayer.Properties.Resources.sound_0;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void trackBar2_MouseCaptureChanged(object sender, EventArgs e)
        {
            changedTime();
            isMoving = false;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            isMoving = true;
        }
        private void changedTime()
        {
            player.controls.currentPosition = trackBar2.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if(player.controls.currentPosition >= 3)
            {
                player.controls.currentPosition = 0;
            }
            else
            {
                if(listBox1.SelectedIndex == 0)
                {
                    if (loop)
                    {
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        label2.Text = Convert.ToString(listBox1.SelectedItem);
                        player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                    }
                    else
                    {
                        player.controls.currentPosition = 0;
                    }
                }
                else
                {
                    listBox1.SelectedIndex = listBox1.SelectedIndex - 1;
                    label2.Text = Convert.ToString(listBox1.SelectedItem);
                    player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex + 1 == listBox1.Items.Count)
            {

                if (loop)
                {
                    listBox1.SelectedIndex = 0;
                    label2.Text = Convert.ToString(listBox1.SelectedItem);
                    player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
                }
            }
            else
            {
                listBox1.SelectedIndex = listBox1.SelectedIndex + 1;
                label2.Text = Convert.ToString(listBox1.SelectedItem);
                player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (loop)
            {
                loop = false;
                pictureBox1.BackgroundImage = MediaPlayer.Properties.Resources.loop;
            }
            else
            {
                loop = true;
                pictureBox1.BackgroundImage = MediaPlayer.Properties.Resources.loop_on;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            player.controls.pause();
            player.controls.currentPosition = 0;
            button1.BackgroundImage = MediaPlayer.Properties.Resources.play;
            label2.Text = Convert.ToString(listBox1.SelectedItem);
            player.settings.volume = startVolume;
            timer2.Stop();
            timer2.Enabled = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label2.Text = Convert.ToString(listBox1.SelectedItem);
            player.URL = path + "\\" + listBox1.SelectedItem + ".mp3";
            button1.BackgroundImage = MediaPlayer.Properties.Resources.pause;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(trackBar1.Value == 0)
            {
                trackBar1.Value = tempVolume;
                player.settings.volume = tempVolume;
                if (trackBar1.Value >= 90)
                    pictureBox2.BackgroundImage = MediaPlayer.Properties.Resources.sound_3;
                else if (trackBar1.Value >= 50 && trackBar1.Value < 90)
                    pictureBox2.BackgroundImage = MediaPlayer.Properties.Resources.sound_2;
                else if (trackBar1.Value > 0 && trackBar1.Value < 50)
                    pictureBox2.BackgroundImage = MediaPlayer.Properties.Resources.sound_1;
                else if (trackBar1.Value == 0)
                    pictureBox2.BackgroundImage = MediaPlayer.Properties.Resources.sound_0;
            }
            else
            {
                tempVolume = trackBar1.Value;
                trackBar1.Value = 0;
                player.settings.volume = 0;
                pictureBox2.BackgroundImage = MediaPlayer.Properties.Resources.sound_0;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            tempVolume = trackBar1.Value;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (shuffle)
            {
                shuffle = false;
                pictureBox3.BackgroundImage = MediaPlayer.Properties.Resources.shuffle;
            }
            else
            {
                shuffle = true;
                pictureBox3.BackgroundImage = MediaPlayer.Properties.Resources.shuffle_on;
            }
        }
        /*
        private string getDuration(string FilePath)
        {
            var time = new NAudio.AudioFileReader(FilePath);
            var length = time.TotalTime;
            return Convert.ToString(length);
        }*/
    }
}
