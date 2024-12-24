namespace WorldOfZuul;

public class EventHelper{
    public Dictionary<string, string>? DescriptionChanges {get; set;}
    public Dictionary<string, bool>? ChangeInteractables {get; set;}
    public List<string>? DeactivatesAfterFinish {get; set;}
    public Dictionary<string, string> ConnectRooms { get; set; }
    public List<string> DisconnectRooms {get; set;}
    
    
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
        foreach((string direction, string destination) in ConnectRooms)
            {
            if (!gameRef.CurrentRoom.Exits.ContainsKey(direction))
                {
                gameRef.CurrentRoom.Exits.Add(direction, destination);
                string reverseDirection = GetOppositeDirection(direction); // so that you could go backwards
                gameRef.Rooms[destination].Exits[reverseDirection] = gameRef.CurrentRoom.RoomId;
                }
            }
        }

        if(DisconnectRooms != null) {
            foreach(string direction in DisconnectRooms) {
                if (gameRef.CurrentRoom.Exits.ContainsKey(direction)) {
                    gameRef.CurrentRoom.Exits.Remove(direction);
                    // so that you couldn't go back from another end
                } 
            }
        }
    }


private string GetOppositeDirection(string direction)
{
    return direction switch
    {
        "north" => "south",
        "south" => "north",
        "east" => "west",
        "west" => "east",
        _ => throw new ArgumentException("Invalid direction")
    };
}

}