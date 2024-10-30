namespace WorldOfZuul
{

    public class Event
    {

    public bool IsActive { get; set; }
    public Interactable ParentInteractable { get; set; }
    public List<Event> ActivatesAfterFinish { get; set; }
    public string Description { get; private set; }
  
    
   
    public Event(string description, Interactable pi)
    {
        IsActive = false;
        ActivatesAfterFinish = new List<Event>();
        Description = description;
        ParentInteractable = pi;
    }

   
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
        foreach (Event nextEvent in ActivatesAfterFinish)
        {
            nextEvent.Run();
        }
        }
    }

    public class TextEvent : Event
    {   
        public string Text {get; set;} = "";

        public TextEvent(string description, Interactable pi, string text) : base(description, pi){
            Text = text; // Text to be printed when event gets run
        } // Constructor, uses constructor of Event but with added Text var
        
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
}