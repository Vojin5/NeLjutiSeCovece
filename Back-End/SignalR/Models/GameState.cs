namespace Back_End.SignalR.Models;

public class GameState
{
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

    /// <summary>
    /// </summary>

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

    //members
    private List<PlayerInfo> _players;
    private static int IdGenerator = 0;
    private int nextPlayer = 0;
    private int kockica = 0;

    private Figure[][] _figures = new Figure[4][]; //igrac 0, figure [0..3]
    private List<Figure> _positions = Enumerable.Repeat(Figure.Default, 56).ToList();

    //props
    public List<PlayerInfo> Players { get => _players; set => _players = value; }
    public int Id { get; set; } = IdGenerator++;
    public int CurrentPlayerTurn { get; set; }
    public int NextPlayerTurnId { get => ++CurrentPlayerTurn % 4; }

    public GameState(List<PlayerInfo> players)
    {
        _players = players;
        CurrentPlayerTurn = 0;
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
        Console.WriteLine(CurrentPlayerTurn % 4);
        Figure[] playerFigures = _figures[CurrentPlayerTurn % 4];
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
    //ostale su provere za baze samih boja i da li se na putu nalazi figura iste boje
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

            if (newPosition == GREEN_BASE0_POSITION
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

            if (newPosition == BLUE_BASE0_POSITION
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

            if (newPosition == RED_BASE0_POSITION
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

            if (newPosition == YELLOW_BASE0_POSITION
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
        if (_players[CurrentPlayerTurn % 4].ConnectionId != connectionId) return false;
        return true;
    }
    public Action UpdateGameState(PlayerMove move)
    {
        Action retAction;

        /*Figure figure = _figures[CurrentPlayerTurn % 4][move.FigureId % 4];
        if (figure.InBase)
        {
            figure.InBase = false;
        }

        figure.Position = move.NewPosition;
        Figure figureAtPos = _positions[move.NewPosition];

        if (figureAtPos != Figure.Default)
        {
            figureAtPos.InBase = true;
            figureAtPos.Position = -1;

            retAction = new FigureOnFigure(figure.)
        }

        _positions[move.NewPosition] = figure;*/

        Figure attackingFigure = _figures[CurrentPlayerTurn % 4][move.FigureId % 4];
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
}

public enum Color
{
    YELLOW,
    GREEN,
    BLUE,
    RED,
    WHITE
}

