using System.Text.Json;

namespace WorldOfZuul
{
    public class Game
    {
        public Player Player {get; private set;}
        public bool ContinuePlaying {get; set;} = true;
        private Room? currentRoom;
        private Room? previousRoom;

        public Dictionary<string, Room>? Rooms {get; private set;}
        public Dictionary<string, Interactable>? Interactables {get; private set;}
        public Dictionary<string, Event>? Events {get; private set;}

        public Game()
        {
            Player = new Player(this);
            CreateRooms();
        }

        private void CreateRooms()
        {       
            using StreamReader reader = new(@$".\JsonFiles\Rooms.json");
            string? jsonString = reader.ReadToEnd();
            Rooms = JsonSerializer.Deserialize<Dictionary<string, Room>>(jsonString);
        }
      /*  
        private void CreateInteractibles()
        {       
            using StreamReader reader = new(@$"{AppContext.BaseDirectory}\JsonFiles\Interactables.json");
            string jsonString = reader.ReadToEnd();
            Interactables = JsonSerializer.Deserialize<Dictionary<string, Interactable>>(jsonString);
        }

        private void CreateEvents()
        {       
            using StreamReader reader = new(@$"{AppContext.BaseDirectory}\JsonFiles\Events.json");
            string jsonString = reader.ReadToEnd();
            Events = JsonSerializer.Deserialize<Dictionary<string, Event>>(jsonString);
        }

    */
        public void Play()
        {   
            currentRoom = Rooms["village-of-ix"];
            Parser parser = new();

            PrintWelcome();

            while (ContinuePlaying)
            {
                Console.WriteLine(currentRoom?.ShortDescription);
                Console.Write("> ");

                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Please enter a command.");
                    continue;
                }

                Command? command = parser.GetCommand(input);

                if (command == null)
                {
                    Console.WriteLine("I don't know that command.");
                    continue;
                }

                switch(command.Name)
                {
                    case "look":
                        Console.WriteLine(currentRoom?.LongDescription);
                        break;

                    case "back":
                        if (previousRoom == null)
                            Console.WriteLine("You can't go back from here!");
                        else
                        {
                            Room? tmp = currentRoom;
                            currentRoom = previousRoom;
                            previousRoom = tmp;
                        }
                        break;

                    case "north":
                    case "south":
                    case "east":
                    case "west":
                        Move(command.Name);
                        break;

                    case "quit":
                        ContinuePlaying = false;
                        break;

                    case "help":
                        PrintHelp();
                        break;

                    case "interact":
                    case "i":
                        ManageInteract(command.SecondWord);
                        
                        break;

                    default:
                        Console.WriteLine("I don't know what command.");
                        break;
                }
            }

            Console.WriteLine("Thank you for playing World of Zuul!");
        }

        private void ManageInteract(string name)
        {
            if (currentRoom?.Interactables.ContainsKey(name) == true)
            {
                string id = currentRoom?.Interactables[name];
                Interactables[id].Interact(); // the Interact() method needs to know the particular NPC you're referring to
            }
            else
            {
                Console.WriteLine("Nothing to interact with in here");
            }
        }

        private void Move(string direction)
        {
            if (currentRoom?.Exits.ContainsKey(direction) == true)
            {
                previousRoom = currentRoom;
                currentRoom = Rooms[ currentRoom.Exits[direction] ];
            }
            else
            {
                Console.WriteLine($"You can't go {direction}!");
            }
        }


        private static void PrintWelcome()
        {
            Console.WriteLine("Welcome to the World of Zuul!");
            Console.WriteLine("World of Zuul is a new, incredibly boring adventure game.");
            PrintHelp();
            Console.WriteLine();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("You are lost. You are alone. You wander");
            Console.WriteLine("around the university.");
            Console.WriteLine();
            Console.WriteLine("Navigate by typing 'north', 'south', 'east', or 'west'.");
            Console.WriteLine("Type 'look' for more details.");
            Console.WriteLine("Type 'back' to go to the previous room.");
            Console.WriteLine("Type 'help' to print this message again.");
            Console.WriteLine("Type 'quit' to exit the game.");
        }
    }
}
