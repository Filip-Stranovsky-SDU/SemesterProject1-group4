namespace WorldOfZuul
{

    public class Event
    {

    public bool IsActive { get; set; }
    public Interactible ParentInteractible { get; set; }
    public List<Event> ActivatesAfterFinish { get; set; }
    public string Description { get; private set; }
  
    
   
    public Event(string description)
    {
        IsActive = false;
        ActivatesAfterFinish = new List<Event>();
        Description = description;

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



    private void CompleteEvent()
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
        
    }
}