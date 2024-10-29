namespace WorldOfZuul;

public class Player{

    public Dictionary<string, int> Resources {get; set;} = new();
    
    private Game gameRef;


    public Player(Game game){
        gameRef = game;
        Resources.Add("Environment", 40);
        Resources.Add("Social", 40);
        Resources.Add("Money", 40);
    }

    
    public Player(Game game, int[] resources){
        gameRef = game;
        Resources.Add("Environment", resources[0]);
        Resources.Add("Social", resources[1]);
        Resources.Add("Money", resources[2]);
    }


    public void ChangeResources(Dictionary<string, int> changes){
        foreach(KeyValuePair<string, int> change in changes){
            Resources[change.Key] += change.Value;
        }
        CheckResources();
    }

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

}