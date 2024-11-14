namespace WorldOfZuul;

public class Player{

    public Resources WorldStats {get; set;} = new();
    private Game gameRef;

    // Add resource
    public Player(Game game){
        gameRef = game;
        WorldStats.Environment = 50;
        WorldStats.Money = 50;
        WorldStats.Social = 50;
    }


    // Resource addition or subtraction
    // I need a method for that
    // but then shouldn't this method take in the Resources dictionary
    public void ChangeResources(Resources changes)
    // where do I have my changes dictionary or where should I have it?
    {
        WorldStats.Change(changes)
    // ensure that resources can't be negative
        CheckResources();
    }

    // display the current state of resources

    private void CheckResources(){
        if(WorldStats.Enviroment > 90 && WorldStats.Social > 90){
            Console.Write("You won!");
            gameRef.ContinuePlaying = false;
            return;
        }
        if(WorldStats.Enviroment < 20 || WorldStats.Social < 20){
            Console.Write("You lost! XD");
            gameRef.ContinuePlaying = false;
            return;
        }
        
    }

    public string DisplayResources(){
        Console.WriteLine(WorldStats.Display());
    }

// stat bar display:
/*
string statBar = "Social: " + Resources[Social] + " | Environment: " + Resources[Environment] + "
| Money: $" Resources[Money] ?;
Console.SetCursorPosition(0,0);
Console.WriteLine(statBar);


probably in the Game class, method:

UpdateStatBar(string something)
{
Player.something
}
- and this method might need to be called to player because that's where after picking from
multiple choices, you might increase or reduce your resources


*/

}