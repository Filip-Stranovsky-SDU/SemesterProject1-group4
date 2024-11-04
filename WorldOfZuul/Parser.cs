using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfZuul
{
    public class Parser
    {
        private readonly CommandWords commandWords = new();

        public Command? GetCommand(string inputLine)
        {
            string[] words = inputLine.Split();

            if (words.Length == 0 || !commandWords.IsValidCommand(words[0]))
            {
                return null;
            }

            if (words.Length > 1)
            {
                string secondPart = String.Join(" ", words.Skip(1));
                return new Command(words[0], secondPart);
            }

            return new Command(words[0]);
        }
    }
// we want the words[1] to be merged with words[2], words [3]... - we want that in case we have characters with multiple names, etc.
}
