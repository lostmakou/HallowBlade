using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RPS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int rounds = 3;
        int timerPerRound = 6;
        bool gameover = false;
        string[] CPUchoiceList = { "rock", "paper", "scissor", "paper", "scissor", "rock" };
        int randomNumber = 0;
        Random rnd = new Random();
        string CPUchoice;
        string playerChoice;
        int playerwins;
        int AIwins;

        DispatcherTimer gameTimer = new DispatcherTimer();


        public MainWindow()
        {
            InitializeComponent();
            playerChoice = "none";
            txtTime.Text = "5";

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += Timer_Tick;

            gameTimer.Start();
        }

        private void btnRock_Click(object sender, EventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\79044\\Desktop\\игра\\HallowBlade-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Fist_gg.png", UriKind.RelativeOrAbsolute)));
            picPlayer.Fill = imageBrush;
            playerChoice = "rock";
        }
        private void btnPaper_Click(object sender, EventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\79044\\Desktop\\игра\\HallowBlade-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Paper_gg.png", UriKind.RelativeOrAbsolute)));
            picPlayer.Fill = imageBrush;
            playerChoice = "paper";
        }

        private void btnScissors_Click(object sender, EventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\79044\\Desktop\\игра\\HallowBlade-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Scissors_gg.png", UriKind.RelativeOrAbsolute)));
            picPlayer.Fill = imageBrush;
            playerChoice = "scissor";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timerPerRound -= 1;

            txtTime.Text = timerPerRound.ToString();
            roundsText.Text = "Rounds: " + rounds;

            if (timerPerRound < 1)
            {
                gameTimer.Stop();
                timerPerRound = 6;

                randomNumber = rnd.Next(0, CPUchoiceList.Length);

                CPUchoice = CPUchoiceList[randomNumber];

                switch (CPUchoice)
                {
                    case "rock":
                        ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\79044\\Desktop\\игра\\HallowBlade-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Fist_boss.png", UriKind.RelativeOrAbsolute)));
                        picCPU.Fill = imageBrush;
                        break;
                    case "paper":
                        ImageBrush imageBrush1 = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\79044\\Desktop\\игра\\HallowBlade-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Paper_boss.png", UriKind.RelativeOrAbsolute)));
                        picCPU.Fill = imageBrush1;
                        break;
                    case "scissor":
                        ImageBrush imageBrush2 = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\79044\\Desktop\\игра\\HallowBlade-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Scissors_boss.png", UriKind.RelativeOrAbsolute)));
                        picCPU.Fill = imageBrush2;
                        break;
                }


                if (rounds > 0)
                {
                    checkGame();
                }
                else
                {
                    if (playerwins > AIwins)
                    {
                        MessageBox.Show("Вы выиграли эту игру");
                    }
                    else
                    {
                        MessageBox.Show("Босс выиграл эту игру");
                    }

                    gameover = true;
                }
            }
        }

        private void checkGame()
        {

            // AI and player win rules

            if (playerChoice == "rock" && CPUchoice == "paper")
            {

                AIwins += 1;

                rounds -= 1;

                MessageBox.Show("Вы проиграли этот раунд");

            }
            else if (playerChoice == "scissor" && CPUchoice == "rock")
            {
                AIwins += 1;

                rounds -= 1;

                MessageBox.Show("Вы проиграли этот раунд");
            }
            else if (playerChoice == "paper" && CPUchoice == "scissor")
            {

                AIwins += 1;

                rounds -= 1;

                MessageBox.Show("Вы проиграли этот раунд");

            }
            else if (playerChoice == "rock" && CPUchoice == "scissor")
            {

                playerwins += 1;

                rounds -= 1;

                MessageBox.Show("Вы выиграли этот раунд");

            }
            else if (playerChoice == "paper" && CPUchoice == "rock")
            {

                playerwins += 1;

                rounds -= 1;

                MessageBox.Show("Вы выиграли этот раунд");

            }
            else if (playerChoice == "scissor" && CPUchoice == "paper")
            {
                playerwins += 1;

                rounds -= 1;

                MessageBox.Show("Вы выиграли этот раунд");

            }
            else if (playerChoice == "none")
            {
                MessageBox.Show("Сделайте выбор!");
            }
            else
            {
                MessageBox.Show("Ничья!");

            }

            startNextRound();
        }

        public void startNextRound()
        {

            if (gameover)
            {
                return;
            }

            txtMessage.Text = "Вы: " + playerwins + " - " + "Босс: " + AIwins;

            playerChoice = "none";

            gameTimer.Start();

            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\79044\\Desktop\\игра\\HallowBlade-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Question_button.png", UriKind.RelativeOrAbsolute)));
            picPlayer.Fill = imageBrush;
            picCPU.Fill = imageBrush;
        }

        private void restartGame(object sender, EventArgs e)
        {
            playerwins = 0;
            AIwins = 0;
            rounds = 3;
            txtMessage.Text = "Вы: " + playerwins + " - " + "Босс: " + AIwins;

            playerChoice = "none";
            txtTime.Text = "5";

            gameTimer.Start();

            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("C:\\Users\\79044\\Desktop\\игра\\HallowBlade-main\\Timer and Keyboard MOO ICT\\Data\\Texture\\Question_button.png", UriKind.RelativeOrAbsolute)));
            picPlayer.Fill = imageBrush;
            picCPU.Fill = imageBrush;

            gameover = false;
        }
    }
}
