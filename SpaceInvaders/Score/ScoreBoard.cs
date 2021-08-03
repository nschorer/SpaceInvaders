using System;
using System.Diagnostics;

namespace SpaceInvaders
{
    public class ScoreBoard
    {
        public enum Player
        {
            Player1,
            Player2,

            Unitialized
        }

        private ScoreBoard(Font Player1Score, Font Player2Score, Font HighScoreFont)
        {
            Score1Display = Player1Score;
            Score2Display = Player2Score;
            HighScoreDisplay = HighScoreFont;
            score1 = 0;
            score2 = 0;
            highScore = 0;
            SetCurrentPlayer(Player.Unitialized);
        }

        public static void Create(Font Player1Score, Font Player2Score, Font HighScoreFont)
        {
            // initialize the singleton here
            //Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new ScoreBoard(Player1Score, Player2Score, HighScoreFont);
            }
        }

        public static void AddToScore(int points)
        {

            Player player = GetCurrentPlayer();

            if (player == Player.Player1)
            {
                score1 += points;
                Score1Display.UpdateMessage(score1.ToString("D4"));
            }

            else if (player == Player.Player2)
            {
                score2 += points;
                Score2Display.UpdateMessage(score2.ToString("D4"));
            }

            else
            {
                Debug.Assert(false);
            }
        }

        private static Player GetCurrentPlayer()
        {
            // We'll change this later
            return currentPlayer;
        }

        public static void SetCurrentPlayer(ScoreBoard.Player player)
        {
            currentPlayer = player;
        }

        public static void CheckHighScore()
        {
            int newScore = score1;
            if (score2 > score1) newScore = score2;
            
            if (newScore > highScore)
            {
                highScore = newScore;
                HighScoreDisplay.UpdateMessage(highScore.ToString("D4"));
            }
        }

        public static void SetPlayerScoresToZero()
        {
            score1 = 0;
            score2 = 0;
        }

        public static void RefreshScoreDisplay()
        {
            Score1Display = FontMan.Find(Font.Name.Score1);
            Score2Display = FontMan.Find(Font.Name.Score2);
            HighScoreDisplay = FontMan.Find(Font.Name.HighScore);

            Score1Display.UpdateMessage(score1.ToString("D4"));
            Score2Display.UpdateMessage(score2.ToString("D4"));
            HighScoreDisplay.UpdateMessage(highScore.ToString("D4"));
        }

        static ScoreBoard pInstance;
        static Font Score1Display;
        static Font Score2Display;
        static Font HighScoreDisplay;
        static int score1;
        static int score2;
        static int highScore;
        static Player currentPlayer;
    }
}
