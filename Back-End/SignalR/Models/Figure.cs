namespace Back_End.SignalR.Models;
public class Figure
{
    private static Figure defaultFigure = new Figure(-1);
    public int Id { get; set; }
    public int Position { get; set; }
    public bool InBase { get; set; }
    public bool InHome { get; set; }
    public int HomePosition { get; set; }
    public Color Color { get; set; }



    public static Figure Default { get => defaultFigure; }

    public Figure(int id)
    {
        Id = id;
        InBase = true;
        InHome = false;
        Color = Color.WHITE;
    }

    public Figure(int id, Color color)
    {
        Id = id;
        Color = color;
        InBase = true;
        InHome = false;
        Position = -1;
    }
}
