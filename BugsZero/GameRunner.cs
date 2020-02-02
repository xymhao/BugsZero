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
            playGame(rand);
        }

        public static void playGame(Random rand, ConsoleLog log = null)
        {
            Game aGame = new Game(log);

            aGame.add("Chet");
            aGame.add("Pat");
            aGame.add("Sue");


            do
            {

                aGame.roll(rand.Next(5) + 1);

                if (rand.Next(9) == 7)
                {
                    notAWinner = aGame.wrongAnswer();
                }
                else
                {
                    notAWinner = aGame.wasCorrectlyAnswered();
                }



            } while (notAWinner);
        }
    }
}
