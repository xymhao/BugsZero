using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BugsZero
{
    public class Game
    {
        private readonly List<string> _players = new List<string>();
        private readonly int[] _places = new int[6];
        private readonly int[] _purses = new int[6];
        private readonly bool[] _inPenaltyBox = new bool[6];

        private readonly Queue _popQuestions = new Queue();
        private readonly Queue _scienceQuestions = new Queue();
        private readonly Queue _sportsQuestions = new Queue();
        private readonly Queue _rockQuestions = new Queue();

        private readonly ConsoleLog _console;

        private int _currentPlayer = 0;
        private bool _isGettingOutOfPenaltyBox;

        public Game(ConsoleLog log)
        {
            _console = log ?? new ConsoleLog();
            for (int i = 0; i < 50; i++)
            {
                _popQuestions.Enqueue("Pop Question " + i);
                _scienceQuestions.Enqueue(("Science Question " + i));
                _sportsQuestions.Enqueue(("Sports Question " + i));
                _rockQuestions.Enqueue(CreateRockQuestion(i));
            }
        }

        private string CreateRockQuestion(int index)
        {
            return "Rock Question " + index;
        }

        public bool IsPlayable()
        {
            return _players.Count >= 2;
        }

        public bool Add(string playerName)
        {
            _players.Add(playerName);
            _places[_players.Count] = 0;
            _purses[_players.Count] = 0;
            _inPenaltyBox[_players.Count] = false;

            _console.WriteLine(playerName + " was added");
            _console.WriteLine("They are player number " + _players.Count);
            return true;
        }

        public void Roll(int roll)
        {
            _console.WriteLine(_players[_currentPlayer] + " is the current player");
            _console.WriteLine("They have rolled a " + roll);

            if (_inPenaltyBox[_currentPlayer])
            {
                if (roll % 2 != 0)
                {
                    _isGettingOutOfPenaltyBox = true;

                    _console.WriteLine(_players[_currentPlayer] + " is getting out of the penalty box");
                    MovePlayerAndAskQuestion(roll);
                }
                else
                {
                    _console.WriteLine(_players[_currentPlayer] + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }

            }
            else
            {

                MovePlayerAndAskQuestion(roll);
            }

        }

        private void MovePlayerAndAskQuestion(int roll)
        {
            _places[_currentPlayer] = _places[_currentPlayer] + roll;
            if (_places[_currentPlayer] > 11) _places[_currentPlayer] = _places[_currentPlayer] - 12;

            _console.WriteLine(_players[_currentPlayer]
                    + "'s new location is "
                    + _places[_currentPlayer]);
            _console.WriteLine("The category is " + CurrentCategory());
            AskQuestion();
        }

        private void AskQuestion()
        {
            if (CurrentCategory() == "Pop")
                _console.WriteLine(_popQuestions.Dequeue());
            if (CurrentCategory() == "Science")
                _console.WriteLine(_scienceQuestions.Dequeue());
            if (CurrentCategory() == "Sports")
                _console.WriteLine(_sportsQuestions.Dequeue());
            if (CurrentCategory() == "Rock")
                _console.WriteLine(_rockQuestions.Dequeue());
        }


        private string CurrentCategory()
        {
            if (_places[_currentPlayer] == 0) return "Pop";
            if (_places[_currentPlayer] == 4) return "Pop";
            if (_places[_currentPlayer] == 8) return "Pop";

            if (_places[_currentPlayer] == 1) return "Science";
            if (_places[_currentPlayer] == 5) return "Science";
            if (_places[_currentPlayer] == 9) return "Science";

            if (_places[_currentPlayer] == 2) return "Sports";
            if (_places[_currentPlayer] == 6) return "Sports";
            if (_places[_currentPlayer] == 10) return "Sports";
            return "Rock";
        }

        public bool CorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    _console.WriteLine("Answer was correct!!!!");
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;
                    _purses[_currentPlayer]++;
                    _console.WriteLine(_players[_currentPlayer]
                            + " now has "
                            + _purses[_currentPlayer]
                            + " Gold Coins.");

                    return PlayerWin();
                }
                else
                {
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count) _currentPlayer = 0;
                    return true;
                }
            }
            else
            {

                _console.WriteLine("Answer was corrent!!!!");
                _purses[_currentPlayer]++;
                _console.WriteLine(_players[_currentPlayer]
                        + " now has "
                        + _purses[_currentPlayer]
                        + " Gold Coins.");

                bool winner = PlayerWin();
                _currentPlayer++;
                if (_currentPlayer == _players.Count) _currentPlayer = 0;

                return winner;
            }
        }

        public bool WrongAnswered()
        {
            _console.WriteLine("Question was incorrectly answered");
            _console.WriteLine(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;

            _currentPlayer++;
            if (_currentPlayer == _players.Count) _currentPlayer = 0;
            return true;
        }


        private bool PlayerWin()
        {
            return _purses[_currentPlayer] != 6;
        }
    }

}
