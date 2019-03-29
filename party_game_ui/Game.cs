using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace party_game_ui
{
    public partial class Game : Form
    {
        //ToDo: Implement visibility so the background becomes less transparent with every success
        //ToDo: Implement resizable UI so the application can be maximized
        //ToDo: Use proper Resources for Strings like " Zoek de schat", "Reset" and "Use Speech"
        public int visibility = 255;
        public Game()
        {
            InitializeComponent();
        }
        private void Game_Load(object sender, EventArgs e)
        {
            pictureBox1.Tag = Properties.Settings.Default.Tile1Word;
            pictureBox2.Tag = Properties.Settings.Default.Tile2Word;
            pictureBox3.Tag = Properties.Settings.Default.Tile3Word;
            pictureBox4.Tag = Properties.Settings.Default.Tile4Word;
            pictureBox5.Tag = Properties.Settings.Default.Tile5Word;
            pictureBox6.Tag = Properties.Settings.Default.Tile6Word;
            pictureBox7.Tag = Properties.Settings.Default.Tile7Word;
            pictureBox8.Tag = Properties.Settings.Default.Tile8Word;
            pictureBox9.Tag = Properties.Settings.Default.Tile9Word;
        }

        private void Answer_TextChanged(object sender, EventArgs e)
        {
            var succes = false;
            if (pictureBox1.Tag.ToString().ToLower() == Answer.Text.ToLower()) { succes = true; pictureBox1.Visible = false; };
            if (pictureBox2.Tag.ToString().ToLower() == Answer.Text.ToLower()) { succes = true; pictureBox2.Visible = false; };
            if (pictureBox3.Tag.ToString().ToLower() == Answer.Text.ToLower()) { succes = true; pictureBox3.Visible = false; };
            if (pictureBox4.Tag.ToString().ToLower() == Answer.Text.ToLower()) { succes = true; pictureBox4.Visible = false; };
            if (pictureBox5.Tag.ToString().ToLower() == Answer.Text.ToLower()) { succes = true; pictureBox5.Visible = false; };
            if (pictureBox6.Tag.ToString().ToLower() == Answer.Text.ToLower()) { succes = true; pictureBox6.Visible = false; };
            if (pictureBox7.Tag.ToString().ToLower() == Answer.Text.ToLower()) { succes = true; pictureBox7.Visible = false; };
            if (pictureBox8.Tag.ToString().ToLower() == Answer.Text.ToLower()) { succes = true; pictureBox8.Visible = false; };
            if (pictureBox9.Tag.ToString().ToLower() == Answer.Text.ToLower()) { succes = true; pictureBox9.Visible = false; };

            if (succes)
            {
                visibility -= 255 / 9;
                //Image image = textBox1.Parent.BackgroundImage;
                //using (Graphics g = Graphics.FromImage(image))
                //{
                //    Pen pen = new Pen(Color.FromArgb(visibility, 255, 255, 255), image.Width);
                //    g.DrawLine(pen, -1, -1, image.Width, image.Height);
                //    g.Save();
                //}
                //textBox1.Parent.BackgroundImage = image;
                new System.Media.SoundPlayer(Properties.Resources.tada).Play();
                Answer.Text = "";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ShowHint(pictureBox1);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ShowHint(pictureBox2);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ShowHint(pictureBox3);
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ShowHint(pictureBox4);
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ShowHint(pictureBox5);
        }
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ShowHint(pictureBox6);
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            ShowHint(pictureBox7);
        }
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ShowHint(pictureBox8);
        }
        private void pictureBox9_Click(object sender, EventArgs e)
        {
            ShowHint(pictureBox9);
        }
        private void ShowHint(PictureBox tile)
        {
            var ary = tile.Tag.ToString().ToCharArray();
            Array.Reverse(ary);
            tile.Parent.Text = new string(ary);
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            Answer.Text = "";
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = true;
            pictureBox5.Visible = true;
            pictureBox6.Visible = true;
            pictureBox7.Visible = true;
            pictureBox8.Visible = true;
            pictureBox9.Visible = true;
            visibility = 255;
            //Image image = button1.Parent.BackgroundImage;
            //using (Graphics g = Graphics.FromImage(image))
            //{
            //    Pen pen = new Pen(Color.FromArgb(visibility, 255, 255, 255), image.Width);
            //    g.DrawLine(pen, -1, -1, image.Width, image.Height);
            //    g.Save();
            //}
            //button1.Parent.BackgroundImage = image;

        }

        private async void UseSpeech_Click(object sender, EventArgs e)
        {
            UseSpeech.Enabled = false;
            new System.Media.SoundPlayer(Properties.Resources.Speech_On).Play();
            var resultText = await RecognizeSpeechAsync();
            //var resultText = (await new SpeechRecognizer(SpeechConfig.FromSubscription(Properties.Settings.Default.SubscriptionKey, Properties.Settings.Default.Region)).RecognizeOnceAsync()).Text;
            Answer.Text = resultText.TrimEnd('.');// resultText.TrimEnd(".".ToCharArray());
            new System.Media.SoundPlayer(Properties.Resources.Speech_Off).Play();
            UseSpeech.Enabled = true;
        }
        public static async Task<string> RecognizeSpeechAsync()
        {
            Application.UseWaitCursor = true;
            var config = SpeechConfig.FromSubscription(Properties.Settings.Default.SubscriptionKey, Properties.Settings.Default.Region);
            config.SpeechRecognitionLanguage = Properties.Settings.Default.SpeechRecognitionLanguage;
            var recognizer = new SpeechRecognizer(config);
            var result = await recognizer.RecognizeOnceAsync();
            Application.UseWaitCursor = false;

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                return result.Text;
            }
            else if (result.Reason == ResultReason.NoMatch)
            {
                return "NOMATCH: Speech could not be recognized.";
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = CancellationDetails.FromResult(result);
                var cancellationdetails = "CANCELLED: Reason=" + cancellation.Reason;

                if (cancellation.Reason == CancellationReason.Error)
                {
                    cancellationdetails += ", ErrorCode=" + cancellation.ErrorCode;
                    cancellationdetails += ", ErrorDetails=" + cancellation.ErrorDetails;
                }
                return cancellationdetails;
            }
            return "RECOGNIZESPEECHASYNC: This shouldn't happen";
        }

    }
}
