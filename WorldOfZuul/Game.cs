using System.Text.Json;

namespace WorldOfZuul
{
    public class Game
    {
        public Player Player {get; private set;}
        public bool ContinuePlaying {get; set;} = true;
        public Room CurrentRoom { get; internal set; }
        private Room? previousRoom;

        public Dictionary<string, Room> Rooms {get; private set;} =[];
        public Dictionary<string, Interactable> Interactables {get; private set;}=[];
        public Dictionary<string, Event> Events {get; private set;}=[];

        
        public Game()
        {
            Player = new Player(this);
            CreateRooms();
            CreateInteractibles();
            CreateEvents();
            CurrentRoom = Rooms["village-of-ix"];
        }

        private void CreateRooms()
        {       
            using StreamReader reader = new("./JsonFiles/rooms.json"); // forward '/' to not use @
            string? jsonString = reader.ReadToEnd();
            Rooms = JsonSerializer.Deserialize<Dictionary<string, Room>>(jsonString)??throw new(""); // if this is null, it returns this value
        }
      
        private void CreateInteractibles()
        {       
            using StreamReader reader = new(@$".\JsonFiles\npcs.json");
            string jsonString = reader.ReadToEnd();
            

            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };

            Interactables = JsonSerializer.Deserialize<Dictionary<string, Interactable>>(jsonString, options)??throw new("TODO");
            
            foreach(var entry in Interactables){
                entry.Value.setupGameRef(this);
            }
        }

        private void CreateEvents()
        {       
            using StreamReader reader = new(@$".\JsonFiles\events.json");
            using StreamReader reader1 = new(@$".\JsonFiles\textEvents.json");
            using StreamReader reader2 = new(@$".\JsonFiles\quizEvents.json");
            

            var options = new JsonSerializerOptions{ IncludeFields = true };

            string jsonString = reader.ReadToEnd();
            Events = JsonSerializer.Deserialize<Dictionary<string, Event>>(jsonString, options)??throw new("TODO");

            jsonString = reader1.ReadToEnd();
            var textEvents = JsonSerializer.Deserialize<Dictionary<string, TextEvent>>(jsonString, options)??throw new("TODO");
            
            jsonString = reader2.ReadToEnd();
            var quizEvents = JsonSerializer.Deserialize<Dictionary<string, QuizEvent>>(jsonString, options)??throw new("TODO");

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


        public void Menu()
        {
            while (true)
            {
                string choice = ShowMainMenu();
                switch (choice)
                {
                    case "1":
                        Play();
                        break;
                    case "2":
                        SaveLoad.LoadGame(this);
                        Play();
                        break;
                    case "3":
                        return;
                }
            }
        }


        private string ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("       WORLD OF ZUUL");
            Console.WriteLine("================================");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("3. Exit");
            Console.WriteLine("================================");
            Console.Write("Enter your choice (1-3): ");
            
            return Console.ReadLine() ?? "";
        }
        
        
    
        public void Play()
        {   
            PrintWelcome();
            
            Parser parser = new();

        

            while (ContinuePlaying)
            {
                Console.WriteLine(CurrentRoom?.ShortDescription);
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
                        Console.WriteLine(CurrentRoom?.LongDescription);
                        break;

                    case "back":
                        if (previousRoom == null)
                            Console.WriteLine("You can't go back from here!");
                        else
                        {
                            Room? tmp = CurrentRoom;
                            CurrentRoom = previousRoom;
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

                    case "status":
                        Player.DisplayResources();
                        break;

                    default:
                        Console.WriteLine("I don't know what command.");
                        break;

                    case "save":
                        SaveLoad.SaveGame(this);
                        break;

                    case "load":
                        SaveLoad.LoadGame(this);
                        break;

                    case "map":
                        string Map = ASCIImage.LoadImage(@$".\JsonFiles\MAP.txt"); 
                        Console.WriteLine(Map);
                        Console.WriteLine();
                        break;


                    
                }
            }

            Console.WriteLine("Thank you for playing World of Zuul!");
        }

        private void ManageInteract(string? name)
        {
            if(string.IsNullOrEmpty(name)) throw new("TODO");
            if (CurrentRoom.Interactables.ContainsKey(name) == true)
            {
                string id = CurrentRoom.Interactables[name];
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
            if (CurrentRoom.Exits.ContainsKey(direction) == true)
            {
                previousRoom = CurrentRoom;
                CurrentRoom = Rooms[ CurrentRoom.Exits[direction] ];
            }
            else
            {
                Console.WriteLine($"You can't go {direction}!");
            }
        }


        private static void PrintWelcome()
        {
            Console.WriteLine("Welcome to the SustainQuest world.");
            Console.WriteLine("");
            PrintHelp();
            Console.WriteLine();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Navigate by typing 'north', 'south', 'east', or 'west'.");
            Console.WriteLine("Type 'look' for more details.");
            Console.WriteLine("Type 'back' to go to the previous room.");
            Console.WriteLine("Type 'help' to print this message again.");
            Console.WriteLine("Type 'quit' to exit the game.");
            Console.WriteLine("Type 'interact' or 'i', press space and type in NPC's name to start events,\nfor example, i Petunia");
            Console.WriteLine("Type 'map' for a game map.");
        }

        public static void TypewriterEffect(string text, int delayMilliseconds = 50)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delayMilliseconds);
            }
            Console.WriteLine(); // New line at the end
        }
    }
}
