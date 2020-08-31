using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _2048
{
    /// <summary>
    /// Class which handle game logic
    /// </summary>
    class GameLogic
    {
        public TextBlock[,] PlayingField { get; set; }
        private int[,] PlayingMatrix { get; set; }

        public GameLogic(TextBlock[,] playingField)
        {
            NewGame(playingField);
        }

        /// <summary>
        /// Start new game
        /// </summary>
        /// <param name="playingField">Array with textblocks</param>
        public void NewGame(TextBlock[,] playingField)
        {
            PlayingField = playingField;
            PlayingMatrix = new int[4, 4];

            //Populate PlayingMatrix wiht zeroes
            for (int y = 0; y < PlayingField.GetLength(1); y++)
            {
                for (int x = 0; x < PlayingField.GetLength(0); x++)
                {
                    PlayingMatrix[x, y] = 0;
                }
            }

            SyncFields();
        }

        /// <summary>
        /// Sync PlayingField with data in PlayingMatrix
        /// </summary>
        private void SyncFields()
        {
            if (CheckEnd() == "lose")
            {
                MessageBox.Show("You lose!");
                NewGame(PlayingField);
                return;
            }
            if (CheckEnd() == "win")
            {
                MessageBox.Show("You win");
                NewGame(PlayingField);
                return;
            }

            SpawnNewTile();

            //Load all tiles
            for (int y = 0; y < PlayingField.GetLength(1); y++)
            {
                for (int x = 0; x < PlayingField.GetLength(0); x++)
                {
                    TextBlock item = PlayingField[x, y];

                    switch (PlayingMatrix[x,y])
                    {
                        case 2:
                            item.Background = Brushes.White;
                            break;
                        case 4:
                            item.Background = Brushes.White;
                            break;
                        case 8:
                            item.Background = Brushes.Orange;
                            break;
                        case 16:
                            item.Background = Brushes.Orange;
                            break;
                        case 32:
                            item.Background = Brushes.OrangeRed;
                            break;
                        case 64:
                            item.Background = Brushes.OrangeRed;
                            break;
                        case 128:
                            item.Background = Brushes.Yellow;
                            break;
                        case 256:
                            item.Background = Brushes.Yellow;
                            break;
                        case 512:
                            item.Background = Brushes.Yellow;
                            break;
                        case 1024:
                            item.Background = Brushes.Yellow;
                            break;
                        case 2048:
                            item.Background = Brushes.Yellow;
                            break;
                    }

                    if (PlayingMatrix[x, y] != 0)
                        item.Text = PlayingMatrix[x, y].ToString();
                    else
                    {
                        item.Background = Brushes.LightGray;
                        item.Text = "";
                    }
                }
            }           
        }

        public int CountScore()
        {
            int sum = 0;

            for (int y = 0; y < PlayingField.GetLength(1); y++)
            {
                for (int x = 0; x < PlayingField.GetLength(0); x++)
                {
                    sum += PlayingMatrix[x, y];
                }
            }

            return sum;
        }

        /// <summary>
        /// Spawn new tile in random positon
        /// </summary>
        private void SpawnNewTile()
        {
            bool valid = false;
            Random random = new Random();
            int x = 0;
            int y = 0;

            //Generate number until it finds free tile
            while (valid == false)
            {
                x = random.Next(0, 4);
                y = random.Next(0, 4);

                if (PlayingMatrix[x, y] == 0)
                    valid = true;
            }

            //Chance for spawning 4
            if (random.Next(1, 11) == 1)
                PlayingMatrix[x, y] = 4;
            else
                PlayingMatrix[x, y] = 2;
        }

        /// <summary>
        /// Checking if win or defeate happended
        /// </summary>
        /// <returns>win/lose/""</returns>
        private string CheckEnd()
        {
            string outcome = "";
            bool freeSpace = false;

            for (int y = 0; y < PlayingMatrix.GetLength(1); y++)
            {
                for (int x = 0; x < PlayingMatrix.GetLength(0); x++)
                {
                    //Check win
                    if (PlayingMatrix[x, y] == 2048)
                    {
                        outcome = "win";
                        break;
                    }

                    //Check deafet
                    if (PlayingMatrix[x, y] == 0)
                        freeSpace = true;
                }
            }

            if ((outcome != "win") && (freeSpace))
                return outcome;
            else
                return "lose";
        }


        /// <summary>
        /// Move all possible blocks up
        /// </summary>
        public void UpMove()
        {
            //Make copy of playing matrix
            int[,] temporaryMatrix = PlayingMatrix;
            
            //Check possible outcomes of the move
            for (int y = 1; y < PlayingMatrix.GetLength(1); y++)
            {
                for (int x = 0; x < PlayingMatrix.GetLength(0); x++)
                {
                    if (temporaryMatrix[x, y] != 0)
                        CheckUp(x, y, 1, temporaryMatrix, false);
                }
            }

            //Copy new data into PlayingMatrix
            PlayingMatrix = temporaryMatrix;
            SyncFields();
        }

        /// <summary>
        /// Check upper tiles and move current tile if is it possible
        /// </summary>
        /// <param name="x">originalX</param>
        /// <param name="y">originalY</param>
        /// <param name="i">currentOffset</param>
        /// <param name="matrix">temporaryMatrix</param>
        /// <param name="match">if tiles was match</param>
        private void CheckUp(int x, int y, int i, int[,] matrix, bool match)
        {
            if (y - i >= 0)
            {
                //If tiles matches add them together
                if ((matrix[x,y - i] == matrix[x,y - i + 1]) && (match == false))
                {
                    matrix[x, y - i] = matrix[x, y - i + 1] * 2;
                    matrix[x, y - i + 1] = 0;
                    match = true;
                }
                //If tile is blank move current blank there
                else if (matrix[x, y - i] == 0)
                {
                    matrix[x, y - i] = matrix[x, y - i + 1];
                    matrix[x, y - i + 1] = 0;
                }
                //If tile is not blank nothing happend

                i += 1;
                CheckUp(x, y, i, matrix, match); 
            }
        }

        /// <summary>
        /// Move all possible blocks down
        /// </summary>
        public void DownMove()
        {
            //Make copy of playing matrix
            int[,] temporaryMatrix = PlayingMatrix;

            //Check possible outcomes of the move
            for (int y = PlayingMatrix.GetLength(1) - 2; y >= 0; y--)
            {
                for (int x = 0; x < PlayingMatrix.GetLength(0); x++)
                {
                    if (temporaryMatrix[x, y] != 0)
                        CheckDown(x, y, 1, temporaryMatrix, false);
                }
            }

            //Copy new data into PlayingMatrix
            PlayingMatrix = temporaryMatrix;
            SyncFields();
        }

        /// <summary>
        /// Check downward tiles and move current tile if is it possible
        /// </summary>
        /// <param name="x">originalX</param>
        /// <param name="y">originalY</param>
        /// <param name="i">currentOffset</param>
        /// <param name="matrix">temporaryMatrix</param>
        /// <param name="match">if tiles was match</param>
        private void CheckDown(int x, int y, int i, int[,] matrix, bool match)
        {
            if (y + i <= PlayingMatrix.GetLength(1) - 1)
            {
                //If tiles matches add them together
                if ((matrix[x, y + i] == matrix[x, y + i - 1]) && (match == false))
                {
                    matrix[x, y + i] = matrix[x, y + i - 1] * 2;
                    matrix[x, y + i - 1] = 0;
                    match = true;
                }
                //If tile is blank move current blank there
                else if (matrix[x, y + i] == 0)
                {
                    matrix[x, y + i] = matrix[x, y + i - 1];
                    matrix[x, y + i - 1] = 0;
                }
                //If tile is not blank nothing happend

                i += 1;
                CheckDown(x, y, i, matrix, match);
            }
        }

        /// <summary>
        /// Move all possible blocks left
        /// </summary>
        public void LeftMove()
        {
            //Make copy of playing matrix
            int[,] temporaryMatrix = PlayingMatrix;

            //Check possible outcomes of the move
            for (int y = 0; y < PlayingMatrix.GetLength(1); y++)
            {
                for (int x = 1; x < PlayingMatrix.GetLength(0); x++)
                {
                    if (temporaryMatrix[x, y] != 0)
                        CheckLeft(x, y, 1, temporaryMatrix, false);
                }
            }

            //Copy new data into PlayingMatrix
            PlayingMatrix = temporaryMatrix;
            SyncFields();
        }

        /// <summary>
        /// Check left tiles and move current tile if is it possible
        /// </summary>
        /// <param name="x">originalX</param>
        /// <param name="y">originalY</param>
        /// <param name="i">currentOffset</param>
        /// <param name="matrix">temporaryMatrix</param>
        /// <param name="match">if tiles was match</param>
        private void CheckLeft(int x, int y, int i, int[,] matrix, bool match)
        {
            if (x - i >= 0)
            {
                //If tiles matches add them together
                if ((matrix[x - i, y] == matrix[x - i + 1, y]) && (match == false))
                {
                    matrix[x - i, y] = matrix[x - i + 1, y] * 2;
                    matrix[x - i + 1, y] = 0;
                    match = true;
                }
                //If tile is blank move current blank there
                else if (matrix[x - i, y] == 0)
                {
                    matrix[x - i, y ] = matrix[x - i + 1, y];
                    matrix[x - i + 1, y] = 0;
                }
                //If tile is not blank nothing happend

                i += 1;
                CheckLeft(x, y, i, matrix, match);
            }
        }

        /// <summary>
        /// Move all possible blocks right
        /// </summary>
        public void RightMove()
        {
            //Make copy of playing matrix
            int[,] temporaryMatrix = PlayingMatrix;

            //Check possible outcomes of the move
            for (int y = 0; y < PlayingMatrix.GetLength(1); y++)
            {
                for (int x = PlayingMatrix.GetLength(0) - 2; x >= 0; x--)
                {
                    if (temporaryMatrix[x, y] != 0)
                        CheckRight(x, y, 1, temporaryMatrix, false);
                }
            }

            //Copy new data into PlayingMatrix
            PlayingMatrix = temporaryMatrix;
            SyncFields();
        }

        /// <summary>
        /// Check right tiles and move current tile if is it possible
        /// </summary>
        /// <param name="x">originalX</param>
        /// <param name="y">originalY</param>
        /// <param name="i">currentOffset</param>
        /// <param name="matrix">temporaryMatrix</param>
        /// <param name="match">if tiles was match</param>
        private void CheckRight(int x, int y, int i, int[,] matrix, bool match)
        {
            if (x + i <= PlayingMatrix.GetLength(0) - 1)
            {
                //If tiles matches add them together
                if ((matrix[x + i, y] == matrix[x + i - 1, y]) && (match == false))
                {
                    matrix[x + i, y] = matrix[x + i - 1, y] * 2;
                    matrix[x + i - 1, y] = 0;
                    match = true;
                }
                //If tile is blank move current blank there
                else if (matrix[x + i, y] == 0)
                {
                    matrix[x + i, y] = matrix[x + i - 1, y];
                    matrix[x + i - 1, y] = 0;
                }
                //If tile is not blank nothing happend

                i += 1;
                CheckRight(x, y, i, matrix, match);
            }
        }
    }
}
