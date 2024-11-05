namespace WorldOfZuul;

public class Player{

    public Dictionary<string, int> Resources {get; set;} = new();
    private Game gameRef;

    // Add resource
    public Player(Game game){
        gameRef = game;
        Resources.Add("Environment", 50);
        Resources.Add("Social", 50);
        Resources.Add("Money", 50);
    }

    
    public Player(Game game, int[] resources){
        gameRef = game;
        Resources.Add("Environment", resources[0]);
        Resources.Add("Social", resources[1]);
        Resources.Add("Money", resources[2]);
    }

    // Resource addition or subtraction
    // I need a method for that
    // but then shouldn't this method take in the Resources dictionary
    public void ChangeResources(Dictionary<string, int> changes)
    // where do I have my changes dictionary or where should I have it?
    {
        foreach(KeyValuePair<string, int> change in changes){
            Resources[change.Key] += change.Value;
        }
    // ensure that resources can't be negative
        CheckResources();
    }

    // display the current state of resources

    private void CheckResources(){
        if(Resources["Environment"] > 90 && Resources["Social"] > 90){
            Console.Write("You won!");
            gameRef.ContinuePlaying = false;
            return;
        }
        if(Resources["Environment"] < 20 || Resources["Social"] < 20){
            Console.Write("You lost! XD");
            gameRef.ContinuePlaying = false;
            return;
        }
        
    }

    public void DisplayResources(){
        string result = "";
        foreach(var resource in Resources){
            result += @$"{resource.Key}: {resource.Value} | ";
            
        }

        result = result.Remove(result.Length - 2);
        Console.WriteLine(result);
    }

// stat bar display:
/*
string statBar = "Social: " + Resources[Social] + " | Environment: " + Resources[Environment] + "
| Money: $" Resources[Money] ?;
Console.SetCursorPosition(0,1);
Console.WriteLine(statBar);


probably in the Game class, method:

UpdateStatBar(string Player)
{
Player.
}
- and this method might need to be called to events because that's where after picking from
multiple choices, you might increase or reduce your resources


*/

}