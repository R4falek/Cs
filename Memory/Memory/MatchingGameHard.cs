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
    public partial class MatchingGameHard : Form
    {
        private string name;
        private int fails = 0;
        private int time;
        private int initialTime;
        private bool canPlay = false;
        public MatchingGameHard(string name)
        {
            InitializeComponent();
            AssignIconsToSquares();
            timer4.Start();
            this.name = name;
            timer3.Interval = Settings.getInterval();
            label68.Text = Settings.getInterval().ToString();
            initialTime = Settings.getInitialTime();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (timer3.Enabled == true || timer4.Enabled == false || canPlay == false)
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

                timer3.Start();
            }
        }

        Random random = new Random();

        List<string> icons = new List<string>()
        {
            "!", "!", "N", "N", ",", ",", "k", "k",
            "b", "b", "v", "v", "w", "w", "z", "z",
            "m", "m", "n", "n", "s", "s", "l", "l"
        };

        private void AssignIconsToSquares()
        {
            foreach (Control control in tableLayoutPanel3.Controls)
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

        private void ShowTiles(int time)
        {
            label1.ForeColor = Color.Black;
            Thread.Sleep(time * 1000);
            label1.ForeColor = label1.BackColor;
        }

        Label firstClicked = null;

        Label secondClicked = null;


        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel3.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            timer4.Stop();

            float score = ((float)1 / time) * 1000;
            if (fails != 0)
            {
                score = ((float)1 / time + (float)1 / fails) * 1000;
            }
            string scoreBoard = saveScore((int)score);

            MessageBox.Show("Fails: " + fails + "\nTime: " + time + "\nScore: " + (int)score + "\n\nRanking:" + scoreBoard, "Congratulations");
            Close();
        }

        private string makeRanking(List<string[]> score)
        {
            string ranking = "\n";
            for (int i = 0; i < score.Count; i++)
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

            if (score < Int32.Parse(ts[ts.Count - 1][1]))
            {
                return makeRanking(ts);
            }
            else
            {
                for (int i = 0; i < ts.Count; i++)
                {
                    if (score >= Int32.Parse(ts[i][1]))
                    {
                        string[] add = { name, score.ToString() };
                        ts.Insert(i, add);
                        ts.RemoveAt(ts.Count - 1);
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

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            time++;
            if (canPlay)
            {
                this.Text = time.ToString();
                return;
            }

            if (time >= initialTime)
            {
                foreach (Control control in tableLayoutPanel3.Controls)
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

        private void label67_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label68.Text = (trackBar1.Value * 250).ToString();
            timer3.Interval = trackBar1.Value * 250;
        }

        private void label68_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (firstClicked == null && secondClicked == null && canPlay)
            {
                if (button3.Text == "Stop")
                {
                    button3.Text = "Resume";
                    timer4.Stop();
                }
                else
                {
                    button3.Text = "Stop";
                    timer4.Start();
                }
            }
        }
    }
}
