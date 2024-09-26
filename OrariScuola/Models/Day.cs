namespace OrariScuola.Models;

internal class Day
{
    public string Name { get; set; } = string.Empty;

    public DateTime Date { get; set; }
    public string[] Hours { get; set; } = new string[6];

    public override string ToString()
    {
        return Name + "\n" + Hours[0] + "\n" + Hours[1] + "\n" + Hours[2] + "\n" + Hours[3] + "\n" + Hours[4] + "\n" + Hours[5] + "\n";
    }
}
