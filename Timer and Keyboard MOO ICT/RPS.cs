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
using ZeldaWPF;

namespace RPS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class CanvasHandlerRockPaperScissors
    {
        public bool isWin { get; set; } = false;
        public Boss boss { get; set; }
        int rounds = 1;
        int timerPerRound = 5;
        bool gameover = false;
        string[] CPUchoiceList = { "rock", "paper", "scissor", "paper", "scissor", "rock" };
        int randomNumber = 0;
        Random rnd = new Random();
        string CPUchoice;
        string playerChoice;
        int playerwins;
        int AIwins;
        Canvas canvas, mainCanvas;
        public DispatcherTimer gameTimer = new DispatcherTimer(), mainTimer;

        // Кнопки, прямоугольники, прочая фигня
        Button bntRock, btnScissors, btnPaper;
        Rectangle picPlayer, picCPU;
        TextBlock txtMessage, roundsText, txtTime;

        public CanvasHandlerRockPaperScissors(Canvas canvas, Canvas mainCanvas, DispatcherTimer mainTimer)
        {
            this.canvas = canvas;
            this.mainCanvas = mainCanvas;
            this.mainTimer = mainTimer;
            picPlayer = canvas.FindName("picPlayer") as Rectangle;
            picCPU = canvas.FindName("picCPU") as Rectangle;
            txtMessage = canvas.FindName("txtMessage") as TextBlock;
            roundsText = canvas.FindName("roundsText") as TextBlock;
            txtTime = canvas.FindName("txtTime") as TextBlock;
            bntRock = canvas.FindName("btnRock") as Button;
            btnScissors = canvas.FindName("btnScissors") as Button;
            btnPaper = canvas.FindName("btnPaper") as Button;

            btnPaper.Click += btnPaper_Click;
            btnScissors.Click += btnScissors_Click;
            bntRock.Click += btnRock_Click;

            playerChoice = "none";
            txtTime.Text = "5";

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += Timer_Tick;

            //gameTimer.Start();
        }

        public void Restart()
        {
            timerPerRound = 5;
            rounds = 1;
            AIwins = 0;
            playerwins = 0;
            gameover = false;
            txtMessage.Text = "Вы: " + playerwins + " - " + "Босс: " + AIwins;

            playerChoice = "none";
            txtTime.Text = "5";
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Question_button.png", UriKind.RelativeOrAbsolute)));
            picPlayer.Fill = imageBrush;
            picCPU.Fill = imageBrush;
        }

        private void btnRock_Click(object sender, EventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Fist_gg.png", UriKind.RelativeOrAbsolute)));
            picPlayer.Fill = imageBrush;
            playerChoice = "rock";
        }
        private void btnPaper_Click(object sender, EventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Paper_gg.png", UriKind.RelativeOrAbsolute)));
            picPlayer.Fill = imageBrush;
            playerChoice = "paper";
        }

        private void btnScissors_Click(object sender, EventArgs e)
        {
            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Scissors_gg.png", UriKind.RelativeOrAbsolute)));
            picPlayer.Fill = imageBrush;
            playerChoice = "scissor";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timerPerRound -= 1;

            txtTime.Text = timerPerRound.ToString();
            roundsText.Text = "Раунд: " + rounds;

            if (timerPerRound < 1)
            {
                gameTimer.Stop();
                timerPerRound = 5;

                randomNumber = rnd.Next(0, CPUchoiceList.Length);

                CPUchoice = CPUchoiceList[randomNumber];

                switch (CPUchoice)
                {
                    case "rock":
                        ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Fist_boss.png", UriKind.RelativeOrAbsolute)));
                        picCPU.Fill = imageBrush;
                        break;
                    case "paper":
                        ImageBrush imageBrush1 = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Paper_boss.png", UriKind.RelativeOrAbsolute)));
                        picCPU.Fill = imageBrush1;
                        break;
                    case "scissor":
                        ImageBrush imageBrush2 = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Scissors_boss.png", UriKind.RelativeOrAbsolute)));
                        picCPU.Fill = imageBrush2;
                        break;
                }


                if (rounds < 3)
                {
                    checkGame();
                }
                else
                {
                    if (playerwins >= AIwins)
                    {
                        boss.Health = 0;
                        MessageBox.Show("Вы выиграли эту игру");
                        
                        isWin = true;
                        
                    }
                    else
                    {
                        boss.Health = 4;
                        MessageBox.Show("Босс выиграл эту игру");
                    }
                    gameTimer.Stop();
                    canvas.Visibility = Visibility.Collapsed;
                    mainCanvas.Visibility = Visibility.Visible;
                    mainTimer.Start();
                    gameover = true;

                }
            }
        }

        private void checkGame()
        {

            // AI and player win rules

            if (playerChoice == "rock" && CPUchoice == "paper")
            {
                rounds += 1;
                AIwins += 1;

                MessageBox.Show("Вы проиграли этот раунд");

            }
            else if (playerChoice == "scissor" && CPUchoice == "rock")
            {
                rounds += 1;
                AIwins += 1;

                MessageBox.Show("Вы проиграли этот раунд");
            }
            else if (playerChoice == "paper" && CPUchoice == "scissor")
            {

                rounds += 1;
                AIwins += 1;

                MessageBox.Show("Вы проиграли этот раунд");

            }
            else if (playerChoice == "rock" && CPUchoice == "scissor")
            {
                rounds += 1;
                playerwins += 1;

                MessageBox.Show("Вы выиграли этот раунд");

            }
            else if (playerChoice == "paper" && CPUchoice == "rock")
            {
                rounds += 1;
                playerwins += 1;

                MessageBox.Show("Вы выиграли этот раунд");

            }
            else if (playerChoice == "scissor" && CPUchoice == "paper")
            {
                rounds += 1;
                playerwins += 1;

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

            ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Question_button.png", UriKind.RelativeOrAbsolute)));
            picPlayer.Fill = imageBrush;
            picCPU.Fill = imageBrush;
        }

        //private void restartGame(object sender, EventArgs e)
        //{
        //    playerwins = 0;
        //    AIwins = 0;
        //    rounds = 3;
        //    txtMessage.Text = "Вы: " + playerwins + " - " + "Босс: " + AIwins;

        //    playerChoice = "none";
        //    txtTime.Text = "5";

        //    gameTimer.Start();

        //    ImageBrush imageBrush = new ImageBrush(new BitmapImage(new Uri("../../Data\\Texture\\Question_button.png", UriKind.RelativeOrAbsolute)));
        //    picPlayer.Fill = imageBrush;
        //    picCPU.Fill = imageBrush;

        //    gameover = false;
        //}
    }
}
