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

namespace _2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly GameLogic gameLogic;
        private TextBlock[,] PlayingField;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                gameLogic = new GameLogic(PopulatePlayingField());
                SetScore();
            }
            catch
            {
                Error();
            }
        }

        /// <summary>
        /// Populate array with textblock
        /// </summary>
        /// <returns>PlayingField</returns>
        private TextBlock[,] PopulatePlayingField()
        {
            PlayingField = new TextBlock[4, 4];

            PlayingField[0, 0] = Block0_0;
            PlayingField[0, 1] = Block0_1;
            PlayingField[0, 2] = Block0_2;
            PlayingField[0, 3] = Block0_3;
            PlayingField[1, 0] = Block1_0;
            PlayingField[1, 1] = Block1_1;
            PlayingField[1, 2] = Block1_2;
            PlayingField[1, 3] = Block1_3;
            PlayingField[2, 0] = Block2_0;
            PlayingField[2, 1] = Block2_1;
            PlayingField[2, 2] = Block2_2;
            PlayingField[2, 3] = Block2_3;
            PlayingField[3, 0] = Block3_0;
            PlayingField[3, 1] = Block3_1;
            PlayingField[3, 2] = Block3_2;
            PlayingField[3, 3] = Block3_3;

            return PlayingField;
        }

        /// <summary>
        /// Basic error message
        /// </summary>
        public void Error()
        {
            MessageBox.Show("Error. Pleasy try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Set score
        /// </summary>
        /// <param name="score">int with score</param>
        public void SetScore()
        {
            ScoreLabel.Content = gameLogic.CountScore().ToString();
        }

        #region Events Handlers
        //Event handler for keyDown
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //Movement up
                if ((e.Key == Key.W) || (e.Key == Key.Up))
                    gameLogic.UpMove();

                //Movement down
                if ((e.Key == Key.S) || (e.Key == Key.Down))
                    gameLogic.DownMove();

                //Movement left
                if ((e.Key == Key.A) || (e.Key == Key.Left))
                    gameLogic.LeftMove();

                //Movement right
                if ((e.Key == Key.D) || (e.Key == Key.Right))
                    gameLogic.RightMove();

                //Restart
                if (e.Key == Key.R)
                    gameLogic.NewGame(PlayingField);

                SetScore();
            }
            catch
            {
                Error();
            }
        }

        //New Game with click on the button
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                gameLogic.NewGame(PlayingField);
                SetScore();
            }
            catch
            {
                Error();
            }
        }
        #endregion
    }
}
