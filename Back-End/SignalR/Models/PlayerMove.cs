namespace Back_End.SignalR.Models;

public class PlayerMove
{
    public int FigureId { get; set; }
    public int NewPosition { get; set; }

    public PlayerMove(int figureId, int newPosition)
    {
        FigureId = figureId;
        NewPosition = newPosition;
    }
}
