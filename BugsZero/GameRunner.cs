using System;
using System.Collections.Generic;
using System.Text;

namespace BugsZero
{

    public class GameRunner
    {

        private static bool notAWinner;

        static void Main(string[] args)
        {
            Random rand = new Random();
            var consoleLog = new ConsoleLog();
            PlayGame(rand, consoleLog);

        }

        public static void PlayGame(Random rand, ConsoleLog log = null)
        {
            Game aGame = new Game(log);

            aGame.Add("Chet");
            aGame.Add("Pat");
            aGame.Add("Sue");

            do
            {

                aGame.Roll(rand.Next(5) + 1);

                if (rand.Next(9) == 7)
                {
                    notAWinner = aGame.WrongAnswered();
                }
                else
                {
                    notAWinner = aGame.CorrectlyAnswered();
                }



            } while (notAWinner);
        }
    }
}
