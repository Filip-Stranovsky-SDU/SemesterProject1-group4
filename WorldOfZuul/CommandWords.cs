using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class CommandWords
    {
        public List<string> ValidCommands { get; } = new List<string> { 
<<<<<<< HEAD
            "north", "east", "south", "west", "look", "back", "quit", "help", "interact", "i", "save", "load" };
=======
            "north", "east", "south", "west", "look", "back", "quit", "help", "interact", "i", "status" };
>>>>>>> b0b275419042f66a66980b1ab04f9bdb6012751d

        public bool IsValidCommand(string command)
        {
            return ValidCommands.Contains(command);
        }
    }

}
