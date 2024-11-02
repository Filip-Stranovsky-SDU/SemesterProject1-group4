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
            CreateInteractibles();
            CreateEvents();
        }

        private void CreateRooms()
        {       
            using StreamReader reader = new(@$".\JsonFiles\rooms.json");
            string? jsonString = reader.ReadToEnd();
            Rooms = JsonSerializer.Deserialize<Dictionary<string, Room>>(jsonString);
        }
      
        private void CreateInteractibles()
        {       
            using StreamReader reader = new(@$".\JsonFiles\npcs.json");
            string jsonString = reader.ReadToEnd();
            Interactables = JsonSerializer.Deserialize<Dictionary<string, Interactable>>(jsonString);
            
            foreach(var entry in Interactables){
                entry.Value.setupGameRef(this);
            }
        }

        private void CreateEvents()
        {       
            using StreamReader reader = new(@$".\JsonFiles\events.json");
            using StreamReader reader1 = new(@$".\JsonFiles\textEvents.json");
            using StreamReader reader2 = new(@$".\JsonFiles\quizEvents.json");
            
            string jsonString = reader.ReadToEnd();
            Events = JsonSerializer.Deserialize<Dictionary<string, Event>>(jsonString);

            jsonString = reader1.ReadToEnd();
            var textEvents = JsonSerializer.Deserialize<Dictionary<string, TextEvent>>(jsonString);
            
            jsonString = reader1.ReadToEnd();
            var quizEvents = JsonSerializer.Deserialize<Dictionary<string, QuizEvent>>(jsonString);

            foreach(var entry in Events){
                entry.Value.setupGameRef(this);
            }

            foreach(var entry in textEvents){
                Events.Add(entry.Key, entry.Value);
                entry.Value.setupGameRef(this);
            }
            
            foreach(var entry in quizEvents){
                Events.Add(entry.Key, entry.Value);
                entry.Value.setupGameRef(this);
            }

            
        }

    
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
                /*foreach(var inter in Interactables){
                    Console.WriteLine(@$"{inter.Key} : {inter.Value}");
                }*/
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
