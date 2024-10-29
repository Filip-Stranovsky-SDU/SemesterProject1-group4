namespace WorldOfZuul
{
    public interface IInteractable{
        //Method that triggers when the player interacts with the object
        void Interact();
    }
    public class CollectibleItem : IInteractable
    {
        public string ItemName {get;private set;}
        public CollectibleItem(string itemName)
        {
            ItemName = itemName;
        }

        public void Interact()
        {
            Console.WriteLine($"You picked up {ItemName}!");
            //Logic to add the item to the player's inventory
            //GameManager.Instance.AddToInventory(this);
        }
    }
    public class NPC : IInteractable
    {
        public string Name {get;private set;}
        public NPC(string name)
        {
            Name = name;
        }

        public void Interact()
        {
            Console.WriteLine($"You talk to {Name}.");
            //Trigger a conversation or dialogue box
            //DialogueManager.Instance.StartDialogue(this);
        }
    }

    public class InteractionManager
    {
        public void AttemptInteraction(IInteractable interactable)
        {
            interactable.Interact();
        }
    }
}