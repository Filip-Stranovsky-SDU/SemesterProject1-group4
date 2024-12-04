namespace WorldOfZuul;

public class EventHelper{
    public Dictionary<string, string>? DescriptionChanges {get; set;}
    public Dictionary<string, bool>? ChangeInteractables {get; set;}
    public List<string>? DeactivatesAfterFinish {get; set;}
    public List<string>? ConnectRooms {get; set;}
    
    
    public EventHelper(){}
    public void Execute(Game gameRef){
        if(DescriptionChanges != null){
            foreach((string key, string value) in DescriptionChanges){
                gameRef.Rooms[key].LongDescription=value;
            }
        }
        if(ChangeInteractables != null){
            foreach((string key, bool value) in ChangeInteractables){
                gameRef.Interactables[key].IsAvailable=value;
            }
        }
        if(DeactivatesAfterFinish != null){
            foreach(string key in DeactivatesAfterFinish){
                gameRef.Events[key].IsActive=false;
            }
        }
        if(ConnectRooms != null){
            //TODO
        }
    } 
}