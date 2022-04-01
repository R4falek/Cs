using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Memory
{
    public partial class MatchingGameMedium : Form
    {
        private string name;
        private int fails = 0;
        private int time;
        private int initialTime;
        private bool canPlay = false;
        public MatchingGameMedium(string name)
        {
            InitializeComponent();
            AssignIconsToSquares();
            timer2.Start();
            this.name = name;
            timer1.Interval = Settings.getInterval();
            label18.Text = Settings.getInterval().ToString();
            initialTime = Settings.getInitialTime();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (timer1.Enabled == true || timer2.Enabled == false || canPlay == false)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                CheckForWinner();
                fails++;

                if (firstClicked.Text == secondClicked.Text)
                {
                    fails--;
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                timer1.Start();
            }
        }

        Random random = new Random();

        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z",
            "m", "m", "n", "n"
        };

        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    //iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        Label firstClicked = null;

        Label secondClicked = null;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            timer2.Stop();

            float score = ((float)1 / time) * 1000;
            if (fails != 0)
            {
                score = ((float)1 / time + (float)1 / fails) * 1000;
            }
            string scoreBoard = saveScore((int)score);

            MessageBox.Show("Fails: " + fails + "\nTime: " + time + "\nScore: " + (int)score + "\n\nRanking:" + scoreBoard, "Congratulations");          
            Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            time++;
            if (canPlay)
            {
                this.Text = time.ToString();
                return;
            }

            if (time >= initialTime)
            {
                foreach (Control control in tableLayoutPanel1.Controls)
                {
                    Label iconLabel = control as Label;

                    if (iconLabel != null)
                    {
                        iconLabel.ForeColor = iconLabel.BackColor;
                    }
                }
                time = 0;
                canPlay = true;
            }
        }

        private string makeRanking(List<string[]> score)
        {
            string ranking="\n";
            for(int i = 0; i < score.Count; i++)
            {
                int place = i + 1;
                ranking += "#" + place + " " + score[i][0] + " " + score[i][1] + "\n"; 
            }
            return ranking;
        }

        private string saveScore(int score)
        {   
            string readText = File.ReadAllText(@"C:\Users\rafal\source\repos\Memory\Memory\score.txt");
            string[] board = readText.Split(' ');
            List<string[]> ts = new List<string[]>();
            for (int i = 0; i < board.Length; i += 2)
            {
                string[] add = { board[i], board[i + 1] };
                ts.Add(add);
            }

            if (score < Int32.Parse(ts[ts.Count-1][1]))
            {
                return makeRanking(ts);
            }
            else
            {
                for(int i = 0; i < ts.Count; i++)
                {
                    if(score >= Int32.Parse(ts[i][1]))
                    {
                        string[] add = { name, score.ToString() };
                        ts.Insert(i, add);
                        ts.RemoveAt(ts.Count-1);
                        break;
                    }
                }
                string scoreBoard = "";
                foreach (string[] s in ts)
                {
                    scoreBoard += s[0] + " " + s[1] + " ";
                }
                scoreBoard = scoreBoard.Remove(scoreBoard.Length - 1);
                File.WriteAllText(@"C:\Users\rafal\source\repos\Memory\Memory\score.txt", scoreBoard);
                return makeRanking(ts);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(firstClicked == null && secondClicked == null && canPlay)
            {
                if(button1.Text == "Stop")
                {
                    button1.Text = "Resume";
                    timer2.Stop();
                }
                else
                {
                    button1.Text = "Stop";
                    timer2.Start();
                }
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label18.Text = (trackBar2.Value*250).ToString();
            timer1.Interval = trackBar2.Value * 250;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
