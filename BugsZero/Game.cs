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
            if (IsPop()) return "Pop";

            if (IsScience()) return "Science";

            if (IsSports()) return "Sports";

            return "Rock";
        }

        private bool IsSports()
        {
            return _places[_currentPlayer] == 2
                || _places[_currentPlayer] == 6
                || _places[_currentPlayer] == 10;
        }

        private bool IsScience()
        {
            return _places[_currentPlayer] == 1
                || _places[_currentPlayer] == 5
                || _places[_currentPlayer] == 9;
        }

        private bool IsPop()
        {
            return _places[_currentPlayer] == 0
                   || _places[_currentPlayer] == 4
                   || _places[_currentPlayer] == 8;
        }

        public bool CorrectlyAnswered()
        {
            if (_inPenaltyBox[_currentPlayer])
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    AddCoiins();
                    bool winner = PlayerWin();
                    NextPlayer();
                    return winner;
                }
                else
                {
                    NextPlayer();
                    return true;
                }
            }
            else
            {
                AddCoiins();
                bool winner = PlayerWin();
                NextPlayer();
                return winner;
            }
        }

        public bool WrongAnswered()
        {
            if (_inPenaltyBox[_currentPlayer] 
                && !_isGettingOutOfPenaltyBox)
            {
                NextPlayer();
                return true;
            }

            SendToBox();
            NextPlayer();
            return true;
        }

        private void SendToBox()
        {
            _console.WriteLine("Question was incorrectly answered");
            _console.WriteLine(_players[_currentPlayer] + " was sent to the penalty box");
            _inPenaltyBox[_currentPlayer] = true;
        }

        private void NextPlayer()
        {
            _currentPlayer++;
            if (_currentPlayer == _players.Count)
                _currentPlayer = 0;
        }

        private void AddCoiins()
        {
            _console.WriteLine("Answer was correct!!!!");
            _purses[_currentPlayer]++;
            _console.WriteLine(_players[_currentPlayer]
                    + " now has "
                    + _purses[_currentPlayer]
                    + " Gold Coins.");
        }




        private bool PlayerWin()
        {
            return _purses[_currentPlayer] != 6;
        }
    }

}
