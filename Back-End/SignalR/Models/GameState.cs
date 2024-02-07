using Back_End.Controllers;
using Back_End.Models;
using Back_End.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace Back_End.SignalR.Models;

public class GameState
{
    #region
    private static readonly int BLUE_BASE0_POSITION = 30;
    private static readonly int BLUE_BASE1_POSITION = 31;
    private static readonly int BLUE_BASE2_POSITION = 32;
    private static readonly int BLUE_BASE3_POSITION = 33;

    private static readonly int YELLOW_BASE0_POSITION = 44;
    private static readonly int YELLOW_BASE1_POSITION = 45;
    private static readonly int YELLOW_BASE2_POSITION = 46;
    private static readonly int YELLOW_BASE3_POSITION = 47;

    private static readonly int GREEN_BASE0_POSITION = 2;
    private static readonly int GREEN_BASE1_POSITION = 3;
    private static readonly int GREEN_BASE2_POSITION = 4;
    private static readonly int GREEN_BASE3_POSITION = 5;

    private static readonly int RED_BASE0_POSITION = 16;
    private static readonly int RED_BASE1_POSITION = 17;
    private static readonly int RED_BASE2_POSITION = 18;
    private static readonly int RED_BASE3_POSITION = 19;

    //

    private static readonly int YELLOW_FIGURE_ID0 = 0;
    private static readonly int YELLOW_FIGURE_ID1 = 1;
    private static readonly int YELLOW_FIGURE_ID2 = 2;
    private static readonly int YELLOW_FIGURE_ID3 = 3;

    private static readonly int GREEN_FIGURE_ID0 = 4;
    private static readonly int GREEN_FIGURE_ID1 = 5;
    private static readonly int GREEN_FIGURE_ID2 = 6;
    private static readonly int GREEN_FIGURE_ID3 = 7;

    private static readonly int RED_FIGURE_ID0 = 8;
    private static readonly int RED_FIGURE_ID1 = 9;
    private static readonly int RED_FIGURE_ID2 = 10;
    private static readonly int RED_FIGURE_ID3 = 11;

    private static readonly int BLUE_FIGURE_ID0 = 12;
    private static readonly int BLUE_FIGURE_ID1 = 13;
    private static readonly int BLUE_FIGURE_ID2 = 14;
    private static readonly int BLUE_FIGURE_ID3 = 15;
    #endregion

    #region
    //members
    private List<PlayerInfo> _players;
    private int _currentPlayerTurn = 0;
    private int _activePlayers = 0;

    private Figure[][] _figures = new Figure[4][]; //igrac 0, figure [0..3]
    private List<Figure> _positions = Enumerable.Repeat(Figure.Default, 56).ToList();
    private List<PlayerInfo> _bestPlayers = new();

    //props
    public List<PlayerInfo> Players { get => _players; set => _players = value; }
    public string Id { get; set; }
    public int CurrentPlayerTurn { get => _currentPlayerTurn; set => _currentPlayerTurn = value % MaximumNumberOfPlayers; }
    public int NextPlayerTurnId { get { ++CurrentPlayerTurn; return _currentPlayerTurn; } }
    public int MaximumNumberOfPlayers { get => 4; }
    public int ActivePlayers { get => _activePlayers; set => _activePlayers = value; }

    public bool GameOver { get; set; } = false;

    public MatchHistoryController MatchHistoryController;
    #endregion

    public GameState()
    {
        
    }
    public GameState(List<PlayerInfo> players, string gameId)
    {
        _players = players;
        Id = gameId;
        _activePlayers = MaximumNumberOfPlayers;

        _players.ForEach(p =>
        {
            p.InGame = true;
            p.GameId = gameId;
        });

        for (int i = 0; i < 4; i++)
        {
            _figures[i] = new Figure[4];
            if (i == 0)
            {
                _figures[i][0] = new Figure(YELLOW_FIGURE_ID0, Color.YELLOW);
                _figures[i][1] = new Figure(YELLOW_FIGURE_ID1, Color.YELLOW);
                _figures[i][2] = new Figure(YELLOW_FIGURE_ID2, Color.YELLOW);
                _figures[i][3] = new Figure(YELLOW_FIGURE_ID3, Color.YELLOW);
            }
            else if (i == 1)
            {
                _figures[i][0] = new Figure(GREEN_FIGURE_ID0, Color.GREEN);
                _figures[i][1] = new Figure(GREEN_FIGURE_ID1, Color.GREEN);
                _figures[i][2] = new Figure(GREEN_FIGURE_ID2, Color.GREEN);
                _figures[i][3] = new Figure(GREEN_FIGURE_ID3, Color.GREEN);
            }
            else if (i == 2)
            {
                _figures[i][0] = new Figure(RED_FIGURE_ID0, Color.RED);
                _figures[i][1] = new Figure(RED_FIGURE_ID1, Color.RED);
                _figures[i][2] = new Figure(RED_FIGURE_ID2, Color.RED);
                _figures[i][3] = new Figure(RED_FIGURE_ID3, Color.RED);
            }
            else if (i == 3)
            {
                _figures[i][0] = new Figure(BLUE_FIGURE_ID0, Color.BLUE);
                _figures[i][1] = new Figure(BLUE_FIGURE_ID1, Color.BLUE);
                _figures[i][2] = new Figure(BLUE_FIGURE_ID2, Color.BLUE);
                _figures[i][3] = new Figure(BLUE_FIGURE_ID3, Color.BLUE);
            }
        }

    }

    public List<PlayerMove> GeneratePossiblePlayerMoves(int diceNum)
    {
        Figure[] playerFigures = _figures[CurrentPlayerTurn];
        List<PlayerMove> possibleMoves = new();

        for (int i = 0; i < 4; i++)
        {
            PlayerMove move = null;

            if (playerFigures[i].Color == Color.YELLOW)
            {
                move = GenerateYellowFigureMove(playerFigures[i], diceNum);
            }
            else if (playerFigures[i].Color == Color.GREEN)
            {
                move = GenerateGreenFigureMove(playerFigures[i], diceNum);
            }
            else if (playerFigures[i].Color == Color.RED)
            {
                move = GenerateRedFigureMove(playerFigures[i], diceNum);
            }
            else if (playerFigures[i].Color == Color.BLUE)
            {
                move = GenerateBlueFigureMove(playerFigures[i], diceNum);
            }

            if (move != null)
            {
                possibleMoves.Add(move);
            }
        }

        return possibleMoves;

    }
    private PlayerMove GenerateYellowFigureMove(Figure figure, int diceNum)
    {
        if (diceNum == 6 && figure.InBase)
        {
            if (_positions[48].Color == Color.YELLOW)
            {
                return null;
            }
            return new PlayerMove(figure.Id, -1, 48);
        }
        else if (diceNum != 6 && figure.InBase)
        {
            return null;
        }
        else
        {
            int newPosition = (figure.Position + diceNum) % 56;

            if (figure.InHome &&
                (newPosition == YELLOW_BASE0_POSITION
                || newPosition == YELLOW_BASE1_POSITION
                || newPosition == YELLOW_BASE2_POSITION
                || newPosition == YELLOW_BASE3_POSITION)
                && _positions[newPosition].Color != Color.YELLOW)
            {
                return new PlayerMove(figure.Id, figure.Position, newPosition);
            }
            else if (figure.InHome && newPosition > YELLOW_BASE3_POSITION)
            {
                return null;
            }
            else if (newPosition == GREEN_BASE0_POSITION
                || newPosition == GREEN_BASE1_POSITION
                || newPosition == GREEN_BASE2_POSITION
                || newPosition == GREEN_BASE3_POSITION
                || GREEN_BASE3_POSITION > figure.Position && GREEN_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (newPosition == RED_BASE0_POSITION
                || newPosition == RED_BASE1_POSITION
                || newPosition == RED_BASE2_POSITION
                || newPosition == RED_BASE3_POSITION
                || RED_BASE3_POSITION > figure.Position && RED_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (newPosition == BLUE_BASE0_POSITION
               || newPosition == BLUE_BASE1_POSITION
               || newPosition == BLUE_BASE2_POSITION
               || newPosition == BLUE_BASE3_POSITION
               || BLUE_BASE3_POSITION > figure.Position && BLUE_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (figure.Position < YELLOW_BASE3_POSITION && newPosition > YELLOW_BASE3_POSITION)
            {
                return null;
            }

            if (_positions[newPosition].Color == Color.YELLOW)
            {
                return null;
            }
            return new PlayerMove(figure.Id, figure.Position, newPosition);
        }

    }
    private PlayerMove GenerateRedFigureMove(Figure figure, int diceNum)
    {
        if (diceNum == 6 && figure.InBase)
        {
            if (_positions[20].Color == Color.RED)
            {
                return null;
            }
            return new PlayerMove(figure.Id, -1, 20);
        }
        else if (diceNum != 6 && figure.InBase)
        {
            return null;
        }
        else
        {
            int newPosition = (figure.Position + diceNum) % 56;

            if (figure.InHome &&
                (newPosition == RED_BASE0_POSITION
                || newPosition == RED_BASE1_POSITION
                || newPosition == RED_BASE2_POSITION
                || newPosition == RED_BASE3_POSITION)
                && _positions[newPosition].Color != Color.RED)
            {
                return new PlayerMove(figure.Id, figure.Position, newPosition);
            }
            else if (figure.InHome && newPosition > RED_BASE3_POSITION)
            {
                return null;
            }
            else if (newPosition == BLUE_BASE0_POSITION
                || newPosition == BLUE_BASE1_POSITION
                || newPosition == BLUE_BASE2_POSITION
                || newPosition == BLUE_BASE3_POSITION
                || BLUE_BASE3_POSITION > figure.Position && BLUE_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (newPosition == YELLOW_BASE0_POSITION
                || newPosition == YELLOW_BASE1_POSITION
                || newPosition == YELLOW_BASE2_POSITION
                || newPosition == YELLOW_BASE3_POSITION
                || YELLOW_BASE3_POSITION > figure.Position && YELLOW_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (newPosition == GREEN_BASE0_POSITION
                || newPosition == GREEN_BASE1_POSITION
                || newPosition == GREEN_BASE2_POSITION
                || newPosition == GREEN_BASE3_POSITION
                || GREEN_BASE3_POSITION > figure.Position && GREEN_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (figure.Position < RED_BASE3_POSITION && newPosition > RED_BASE3_POSITION)
            {
                return null;
            }

            if (_positions[newPosition].Color == Color.RED)
            {
                return null;
            }
            return new PlayerMove(figure.Id, figure.Position, newPosition);
        }

    }
    private PlayerMove GenerateGreenFigureMove(Figure figure, int diceNum)
    {
        if (diceNum == 6 && figure.InBase)
        {
            if (_positions[6].Color == Color.GREEN)
            {
                return null;
            }
            return new PlayerMove(figure.Id, -1, 6);
        }
        else if (diceNum != 6 && figure.InBase)
        {
            return null;
        }
        else
        {
            int newPosition = (figure.Position + diceNum) % 56;

            if (figure.InHome &&
                (newPosition == GREEN_BASE0_POSITION
                || newPosition == GREEN_BASE1_POSITION
                || newPosition == GREEN_BASE2_POSITION
                || newPosition == GREEN_BASE3_POSITION)
                && _positions[newPosition].Color != Color.GREEN)
            {
                return new PlayerMove(figure.Id, figure.Position, newPosition);
            }
            else if (figure.InHome && newPosition > GREEN_BASE3_POSITION)
            {
                return null;
            }
            else if (newPosition == RED_BASE0_POSITION
                || newPosition == RED_BASE1_POSITION
                || newPosition == RED_BASE2_POSITION
                || newPosition == RED_BASE3_POSITION
                || RED_BASE3_POSITION > figure.Position && RED_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (newPosition == BLUE_BASE0_POSITION
                || newPosition == BLUE_BASE1_POSITION
                || newPosition == BLUE_BASE2_POSITION
                || newPosition == BLUE_BASE3_POSITION
                || BLUE_BASE3_POSITION > figure.Position && BLUE_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (newPosition == YELLOW_BASE0_POSITION
                || newPosition == YELLOW_BASE1_POSITION
                || newPosition == YELLOW_BASE2_POSITION
                || newPosition == YELLOW_BASE3_POSITION
                || YELLOW_BASE3_POSITION > figure.Position && YELLOW_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (figure.Position < GREEN_BASE3_POSITION && newPosition > GREEN_BASE3_POSITION)
            {
                return null;
            }

            if (_positions[newPosition].Color == Color.GREEN)
            {
                return null;
            }


            return new PlayerMove(figure.Id, figure.Position, newPosition);
        }
    }
    private PlayerMove GenerateBlueFigureMove(Figure figure, int diceNum)
    {
        if (diceNum == 6 && figure.InBase)
        {
            if (_positions[34].Color == Color.BLUE)
            {
                return null;
            }
            return new PlayerMove(figure.Id, -1, 34);
        }
        else if (diceNum != 6 && figure.InBase)
        {
            return null;
        }
        else
        {
            int newPosition = (figure.Position + diceNum) % 56;

            if (figure.InHome &&
                (newPosition == BLUE_BASE0_POSITION
                || newPosition == BLUE_BASE1_POSITION
                || newPosition == BLUE_BASE2_POSITION
                || newPosition == BLUE_BASE3_POSITION)
                && _positions[newPosition].Color != Color.BLUE)
            {
                return new PlayerMove(figure.Id, figure.Position, newPosition);
            }
            else if (figure.InHome && newPosition > BLUE_BASE3_POSITION)
            {
                return null;
            }
            else if (newPosition == YELLOW_BASE0_POSITION
                || newPosition == YELLOW_BASE1_POSITION
                || newPosition == YELLOW_BASE2_POSITION
                || newPosition == YELLOW_BASE3_POSITION
                || YELLOW_BASE3_POSITION > figure.Position && YELLOW_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (newPosition == GREEN_BASE0_POSITION
                || newPosition == GREEN_BASE1_POSITION
                || newPosition == GREEN_BASE2_POSITION
                || newPosition == GREEN_BASE3_POSITION
                || GREEN_BASE3_POSITION > figure.Position && GREEN_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (newPosition == RED_BASE0_POSITION
                || newPosition == RED_BASE1_POSITION
                || newPosition == RED_BASE2_POSITION
                || newPosition == RED_BASE3_POSITION
                || RED_BASE3_POSITION > figure.Position && RED_BASE3_POSITION < newPosition)
            {
                newPosition = (newPosition + 4) % 56;
            }
            else if (figure.Position < BLUE_BASE3_POSITION && newPosition > BLUE_BASE3_POSITION)
            {
                return null;
            }

            if (_positions[newPosition].Color == Color.BLUE)
            {
                return null;
            }
            return new PlayerMove(figure.Id, figure.Position, newPosition);
        }
    }

    public bool CheckIfPlayerValid(string connectionId)
    {

        if (_players[CurrentPlayerTurn].ConnectionId != connectionId) { return false; }
        return true;
    }
    public Action UpdateGameState(PlayerMove move)
    {
        Action retAction;   

        Figure attackingFigure = _figures[CurrentPlayerTurn][move.FigureId % 4];
        Figure attackedFigure = _positions[move.NewPosition];
        if (attackingFigure.InBase)
        {
            attackingFigure.InBase = false;

            if (attackedFigure == Figure.Default)
            {
                _positions[move.NewPosition] = attackingFigure;
                retAction = new BaseFigureOnEmptyField(attackingFigure.Id, move.NewPosition);
                attackingFigure.Position = move.NewPosition;
                return retAction;
            }
            else
            {
                _positions[move.NewPosition] = attackingFigure;
                retAction = new BaseFigureOnNonEmptyField(attackedFigure.Position, attackedFigure.Id, attackingFigure.Id, move.NewPosition);
                attackingFigure.Position = move.NewPosition;
                attackedFigure.InBase = true;
                attackedFigure.Position = -1;
                return retAction;
            }
        }
        else
        {
            if (attackedFigure == Figure.Default)
            {
                _positions[move.OldPosition] = Figure.Default;
                _positions[move.NewPosition] = attackingFigure;
                retAction = new FigureOnEmptyField(attackingFigure.Position, move.NewPosition);
                attackingFigure.Position = move.NewPosition;
                CheckIfNewPositionIsInHome(attackingFigure);
                return retAction;
            }
            else
            {
                _positions[move.NewPosition] = attackingFigure;
                _positions[move.OldPosition] = Figure.Default;
                retAction = new FigureOnFigure(attackedFigure.Position, attackedFigure.Id, attackingFigure.Position, move.NewPosition);
                attackedFigure.InBase = true;
                attackedFigure.Position = -1;
                attackingFigure.Position = move.NewPosition;
                return retAction;
            }
        }
    }
    public void CheckIfNewPositionIsInHome(Figure figure)
    {
        if (figure.Color == Color.YELLOW)
        {
            if (figure.Position == YELLOW_BASE0_POSITION
                || figure.Position == YELLOW_BASE1_POSITION
                || figure.Position == YELLOW_BASE2_POSITION
                || figure.Position == YELLOW_BASE3_POSITION)
            {
                figure.InHome = true;
            }
        }
        else if (figure.Color == Color.GREEN)
        {
            if (figure.Position == GREEN_BASE0_POSITION
                || figure.Position == GREEN_BASE1_POSITION
                || figure.Position == GREEN_BASE2_POSITION
                || figure.Position == GREEN_BASE3_POSITION)
            {
                figure.InHome = true;
            }
        }
        else if (figure.Color == Color.RED)
        {
            if (figure.Position == RED_BASE0_POSITION
                || figure.Position == RED_BASE1_POSITION
                || figure.Position == RED_BASE2_POSITION
                || figure.Position == RED_BASE3_POSITION)
            {
                figure.InHome = true;
            }
        }
        else if (figure.Color == Color.BLUE)
        {
            if (figure.Position == BLUE_BASE0_POSITION
                || figure.Position == BLUE_BASE1_POSITION
                || figure.Position == BLUE_BASE2_POSITION
                || figure.Position == BLUE_BASE3_POSITION)
            {
                figure.InHome = true;
            }
        }
    }
    public bool IsGameOver()
    {
        //SimulateGameOver();
        int figuresInHome = 0;
        int counter;
        for (int i = 0; i < 4; i++)
        {
            counter = 0;
            for (int j = 0; j < 4; j++)
            {
                if (_figures[i][j].InHome)
                    counter++;
            }
            if (counter == 4 && !_bestPlayers.Contains(_players[i]))
            {
                _bestPlayers.Add(_players[i]);
            }
            figuresInHome += counter;
        }

        GameOver = figuresInHome == 16;
        return GameOver;
    }

    public void GameOverNotifyPlayers(IHubContext<GameHub> hubContext)
    {
        hubContext.Clients.Client(_bestPlayers[0].ConnectionId).SendAsync("handleGameOver", 5);
        hubContext.Clients.Client(_bestPlayers[1].ConnectionId).SendAsync("handleGameOver", 3);
        hubContext.Clients.Client(_bestPlayers[2].ConnectionId).SendAsync("handleGameOver", 2);
        hubContext.Clients.Client(_bestPlayers[3].ConnectionId).SendAsync("handleGameOver", 1);

        HttpClient client = new();
        var json = JsonConvert.SerializeObject(new List<MatchHistoryUser>() {
            new MatchHistoryUser() {UserId = _bestPlayers[0].Id, Points = 5},
            new MatchHistoryUser() {UserId = _bestPlayers[1].Id, Points = 3},
            new MatchHistoryUser() {UserId = _bestPlayers[2].Id, Points = 2},
            new MatchHistoryUser() {UserId = _bestPlayers[3].Id, Points = 1}
        });
        var httpContent = new StringContent(json, encoding: Encoding.UTF8, "application/json");
        client.PostAsync($"http://{GameHub.SERVER_IP}:5295/MatchHistory/add-match", httpContent);
        client.DeleteAsync($"http://{GameHub.SERVER_IP}:5295/UnfinishedGame/remove/{Id}");
    }

    //ova metoda treba kasnije da se obrise
    private void SimulateGameOver()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j <4;j++)
            {
                _figures[i][j].InHome = true;
            }
        }
    }

    public void HandlePlayerLeaving()
    {
        if (GameOver)
        {
            return;
        }

        string gameState = "{\n\"figures\":{";

        gameState += "\n\"yellow\":[";
        gameState += _figures[0][0].StringState() + "," + _figures[0][1].StringState() + "," + _figures[0][2].StringState() + "," + _figures[0][3].StringState();
        gameState += "],";

        gameState += "\n\"green\":[";
        gameState += _figures[1][0].StringState() + "," + _figures[1][1].StringState() + "," + _figures[1][2].StringState() + "," + _figures[1][3].StringState();
        gameState += "],";

        gameState += "\n\"red\":[";
        gameState += _figures[2][0].StringState() + "," + _figures[2][1].StringState() + "," + _figures[2][2].StringState() + "," + _figures[2][3].StringState();
        gameState += "],";

        gameState += "\n\"blue\":[";
        gameState += _figures[3][0].StringState() + "," + _figures[3][1].StringState() + "," + _figures[3][2].StringState() + "," + _figures[3][3].StringState();
        gameState += "]";

        gameState += "\n},";

        gameState += "\n\"players\":[" + _players[0].Id + ", " + _players[1].Id + ", " + _players[2].Id + ", " + _players[3].Id + "],";

        gameState += "\n\"currentPlayerTurnId\":" + CurrentPlayerTurn;

        gameState += "\n}";

        List<int> userIds = _players.Select(p => p.Id).ToList();

        HttpClient client = new();
        var json = JsonConvert.SerializeObject(new
        {
            gameKey = Id,
            state = gameState,
            playerIds = userIds
        });

        var httpContent = new StringContent(json, encoding: Encoding.UTF8, "application/json");
        client.PostAsync($"http://{GameHub.SERVER_IP}:5295/UnfinishedGame/add", httpContent);
    }

    public string ReCreateState(JObject gameStateJSON, List<PlayerInfo> players)
    {
        _players = new();

        var playerIds = gameStateJSON["players"];
        _players.Add(players.Where(p => p.Id == (int)playerIds[0]).FirstOrDefault());
        _players.Add(players.Where(p => p.Id == (int)playerIds[1]).FirstOrDefault());
        _players.Add(players.Where(p => p.Id == (int)playerIds[2]).FirstOrDefault());
        _players.Add(players.Where(p => p.Id == (int)playerIds[3]).FirstOrDefault());



        string boja = "";
        for (int i = 0; i < 4; i++)
        {
            _figures[i] = new Figure[4];
            if (i == 0) boja = "yellow";
            else if (i == 1) boja = "green";
            else if (i == 2) boja = "red";
            else if (i == 3) boja = "blue";
            for (int j = 0; j < 4; j++)
            {
                Figure figure = new Figure();
                figure.Id = i * 4 + j;
                figure.Position = (int)gameStateJSON!["figures"]![boja]![j]!;
                if (figure.Position == -1) figure.InBase = true; else figure.InBase = false;
                figure.Color = (Color)i;
                if (IsFigureInHome(figure)) figure.InHome = true; else figure.InHome = false;
                if (!figure.InBase) _positions[figure.Position] = figure;
                _figures[i][j] = figure;
            }
        }
        return SerializeState();

    }

    private bool IsFigureInHome(Figure figure)
    {
        if (figure.Color == Color.YELLOW)
        {
            if (figure.Position >= 44 && figure.Position <= 47) return true;
        }
        else if (figure.Color == Color.GREEN)
        {
            if (figure.Position >= 2 && figure.Position <= 5) return true;
        }
        else if (figure.Color == Color.RED)
        {
            if (figure.Position >= 16 && figure.Position <= 19) return true;
        }
        else if (figure.Color == Color.BLUE)
        {
            if (figure.Position >= 30 && figure.Position <= 33) return true;
        }
        return false;
    }

    private string SerializeState()
    {
        string gameState = "{\n\"figures\":{";

        gameState += "\n\"yellow\":[";
        gameState += _figures[0][0].StringState() + "," + _figures[0][1].StringState() + "," + _figures[0][2].StringState() + "," + _figures[0][3].StringState();
        gameState += "],";

        gameState += "\n\"green\":[";
        gameState += _figures[1][0].StringState() + "," + _figures[1][1].StringState() + "," + _figures[1][2].StringState() + "," + _figures[1][3].StringState();
        gameState += "],";

        gameState += "\n\"red\":[";
        gameState += _figures[2][0].StringState() + "," + _figures[2][1].StringState() + "," + _figures[2][2].StringState() + "," + _figures[2][3].StringState();
        gameState += "],";

        gameState += "\n\"blue\":[";
        gameState += _figures[3][0].StringState() + "," + _figures[3][1].StringState() + "," + _figures[3][2].StringState() + "," + _figures[3][3].StringState();
        gameState += "]";

        gameState += "\n},\n";

        gameState += "\"currentPlayerTurnId\":" + CurrentPlayerTurn;
        gameState += "\n}";

        return gameState;
    }
}

public enum Color
{
    YELLOW,
    GREEN,
    RED,
    BLUE,
    WHITE
}

