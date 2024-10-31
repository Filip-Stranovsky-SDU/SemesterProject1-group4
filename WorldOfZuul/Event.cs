namespace WorldOfZuul;


public class Event{

    public bool IsActive { get; set; }
    public string? ParentInteractableName {get; set;}
<<<<<<< HEAD
    public List<Event> ActivatesAfterFinish { get; set; }
    public string Description { get; private set; }

    // Dictionary to store how resources change when event happens 
    public Dictionary<string, int> ChangeInResources {get; set;} = new();
    
    public Game? gameRef;
   
    public Event(string description, Interactable pi, Dictionary<string, int> changeInResources)
=======
    public List<string>? ActivatesAfterFinish { get; set; }
    public string? Description { get; private set; }
    public Dictionary<string, int>? ChangeInResources {get; set;}
    
    public Game? gameRef;
    
    /*
    public Event(string description)
>>>>>>> b5cfbe6936e6600951cf5f06eb869ec2f30a7f25
    {
        IsActive = false;
        ActivatesAfterFinish = new List<string>();
        Description = description;
<<<<<<< HEAD
        ChangeInResources = changeInResources; // resource changes to this event
    }
=======
    }*/
>>>>>>> b5cfbe6936e6600951cf5f06eb869ec2f30a7f25
   
    // Method to start the event

    public bool Run()
    {
        if (!IsActive)
        {
            return false;
        }
        if (gameRef?.Player != null)
        {
            gameRef.Player.ChangeResources(ChangeInResources); // change resources on the player
        }
        CompleteEvent();
        return true;
    }



    protected void CompleteEvent()
    {
        IsActive = false;
        foreach(string eventToActivate in ActivatesAfterFinish){
            Console.WriteLine("XD");
        }
    }
}

public class TextEvent : Event
    {   
        public string Text {get; set;} = "";
<<<<<<< HEAD

        public TextEvent(string description, Interactable pi, string text, Dictionary<string, int> changeInResources) : base(description, pi, changeInResources){
=======
        /*
        public TextEvent(string description, Interactable pi, string text) : base(description){
>>>>>>> b5cfbe6936e6600951cf5f06eb869ec2f30a7f25
            Text = text; // Text to be printed when event gets run
        } // Constructor, uses constructor of Event but with added Text var
        */
        public bool Run(){
            if (!IsActive)
            {
                return false;
            }
            Console.Write(Text);
            if (gameRef?.Player != null)
            {
                gameRef.Player.ChangeResources(ChangeInResources); // Call ChangeResources on the player
            }
            CompleteEvent();
            return true;
        }
    }

public class QuizEvent: Event{

    public string Text {get; set;} = "";
    public List<Event> Options {get; set;} = new();
        /*
        public QuizEvent(string description, 
                        string text, List<Event> options) : base(description){
            Text = text; // Text to be printed when event gets run
            Options = options;
        }*/
    public bool Run(){
        if (!IsActive)
        {
            return false;
        }
        Console.Write(Text);

        string? input = "";
        while(input == ""){
            input = Console.ReadLine();
            if(input == null || input.Length != 1){
                input = "";
                continue;
                // If input is not a character it starts the loop over again
            }
<<<<<<< HEAD
            Console.Write(Text);

            string? input = "";

            while(input == ""){
                input = Console.ReadLine();
                if(input == null || input.Length != 1){
                    input = "";
                    continue;
                    // If input is not a character it starts the loop over again
                }
                int n = input[0] - 'a';
                if(n >= Options.Count || n < 0){
                    input = "";
                    continue;
                }
                // Apply resource changes for a selected option
                // Each option will contain specific resource changes
                Options[n].Run();
=======
            int n = input[0] - 'a';
            if(n >= Options.Count || n < 0){
                input = "";
                continue;
>>>>>>> b5cfbe6936e6600951cf5f06eb869ec2f30a7f25
            }
            // Apply resource changes for a selected option
            // Each option will contain specific resource changes
            Options[n].Run();
        }


        CompleteEvent();
        return true;
    }


    }
<<<<<<< HEAD
    
    // Option Class to represent individual choices in a QuizEvent
    public class Option
    {
        public string Text { get; set; }
        public Dictionary<string, int> ResourceChanges { get; set; } = new();

        public Option(string text, Dictionary<string, int> resourceChanges)
        {
            Text = text;
            ResourceChanges = resourceChanges;
        }

        public void Run()
        {
            Console.WriteLine(Text);

            // Apply resource changes when this option is selected
            if (gameRef?.Player != null)
            {
                gameRef.Player.ChangeResources(ResourceChanges); // Applies changes for this option
            }
        }
    }
}
=======
>>>>>>> b5cfbe6936e6600951cf5f06eb869ec2f30a7f25
