﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class CommandWords
    {
        public List<string> ValidCommands { get; } = new List<string> { 
            "north", "east", "south", "west", "look", "back", "quit", "help", "interact", "i", "save", "load", "status", "map"};

        public bool IsValidCommand(string command)
        {
            return ValidCommands.Contains(command);
        }
    }

}
