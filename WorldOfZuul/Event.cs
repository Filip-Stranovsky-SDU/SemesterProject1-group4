namespace WorldOfZuul
{

    public class Event
    {

    public bool IsActive { get; set; }
    public string? ParentInteractableName {get; set;}
    public List<Event> ActivatesAfterFinish { get; set; }
    public string Description { get; private set; }

    // Dictionary to store how resources change when event happens 
    public Dictionary<string, int> ChangeInResources {get; set;} = new();
    
    public Game? gameRef;
   
    public Event(string description, Interactable pi, Dictionary<string, int> changeInResources)
    {
        IsActive = false;
        ActivatesAfterFinish = new List<Event>();
        Description = description;
        ChangeInResources = changeInResources; // resource changes to this event
    }
   
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
        foreach (Event nextEvent in ActivatesAfterFinish)
        {
            nextEvent.Run();
        }
        }
    }

    public class TextEvent : Event
    {   
        public string Text {get; set;} = "";

        public TextEvent(string description, Interactable pi, string text, Dictionary<string, int> changeInResources) : base(description, pi, changeInResources){
            Text = text; // Text to be printed when event gets run
        } // Constructor, uses constructor of Event but with added Text var
        
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

        public QuizEvent(string description, Interactable pi, 
                        string text, List<Event> options) : base(description, pi){
            Text = text; // Text to be printed when event gets run
            Options = options;
        }
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