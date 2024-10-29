namespace WorldOfZuul
{

    public class Event
    {

    public bool IsActive { get; set; }
    public Interactible ParentInteractible { get; set; }
    public List<Event> ActivatesAfterFinish { get; set; }
    public string Description { get; set; }
  
    
   
    public Event()
    {
        IsActive = false;
        ActivatesAfterFinish = new List<Event>();
        Description = GetDescription();
    }

   
    // Method to start the event

    private bool Activate()
    {
        if (QuizEvent.questActivate == true)
        {
            IsActive = true;
            Console.WriteLine($"{Description}");
            Quest();
        }
    }


    private void CompleteEvent()
    {
        IsActive = false;
        foreach (var nextEvent in ActivatesAfterFinish)
        {
            nextEvent.Activate();
        }
        }
    }
}