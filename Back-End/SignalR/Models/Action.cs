namespace Back_End.SignalR.Models;

public class Action
{
    public int ActionType { get; set; }
    public Action(int actionType)
    {
        ActionType = actionType;
    }
}

public class BaseFigureOnEmptyField : Action
{
    public int OldPosition { get; set; }
    public int NewPosition { get; set; }

    public BaseFigureOnEmptyField(int oldPosition, int newPosition) : base(0)
    {
        OldPosition = oldPosition;
        NewPosition = newPosition;
    }
}

public class BaseFigureOnNonEmptyField : Action
{
    public int OldPosition1 { get; set; }
    public int NewPosition1 { get; set; }
    public int OldPosition2 { get; set; }
    public int NewPosition2 { get; set; }

    public BaseFigureOnNonEmptyField(int oldPosition1, int newPosition1, int oldPosition2, int newPosition2) : base(1)
    {
        OldPosition1 = oldPosition1;
        NewPosition1 = newPosition1;
        OldPosition2 = oldPosition2;
        NewPosition2 = newPosition2;
    }
}

public class FigureOnEmptyField : Action
{
    public int OldPosition { get; set; }
    public int NewPosition { get; set; }

    public FigureOnEmptyField(int oldPosition, int newPosition) : base(2)
    {
        OldPosition = oldPosition;
        NewPosition = newPosition;
    }
}

public class FigureOnFigure : Action
{
    public int OldPosition1 { get; set; }
    public int NewPosition1 { get; set; }
    public int OldPosition2 { get; set; }
    public int NewPosition2 { get; set; }

    public FigureOnFigure(int oldPosition1, int newPosition1, int oldPosition2, int newPosition2) : base(3)
    {
        OldPosition1 = oldPosition1;
        NewPosition1 = newPosition1;
        OldPosition2 = oldPosition2;
        NewPosition2 = newPosition2;
    }
}
