namespace WorldOfZuul;


public class Event{

    public bool IsActive { get; set; }
    public string? ParentInteractableName {get; set;}
    public List<string>? ActivatesAfterFinish { get; set; }
    public string? Description { get; private set; }
    public Dictionary<string, int>? ChangeInResources {get; set;}
    
    public Game? gameRef;
    
    /*
    public Event(string description)
    {
        IsActive = false;
        ActivatesAfterFinish = new List<string>();
        Description = description;
    }*/
   
    // Method to start the event

    public bool Run()
    {
        if (!IsActive)
        {
            return false;
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
        /*
        public TextEvent(string description, Interactable pi, string text) : base(description){
            Text = text; // Text to be printed when event gets run
        } // Constructor, uses constructor of Event but with added Text var
        */
        public bool Run(){
            if (!IsActive)
            {
                return false;
            }
            Console.Write(Text);
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
            int n = input[0] - 'a';
            if(n >= Options.Count || n < 0){
                input = "";
                continue;
            }
            // Apply resource changes for a selected option
            // Each option will contain specific resource changes
            Options[n].Run();
        }


        CompleteEvent();
        return true;
    }


    }
