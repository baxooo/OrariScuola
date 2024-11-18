using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariScuola.Service.Models;

internal class Day
{
    public string Name { get; set; } = string.Empty;

    public DateTime Date { get; set; }
    public string[] Hours { get; set; } = new string[5];

    public override string ToString()
    {
        return Name + "\n" + Hours[0] + "\n" + Hours[1] + "\n" + Hours[2] + "\n" + Hours[3] + "\n" + Hours[4] + "\n" + Hours[5] + "\n";
    }
}
