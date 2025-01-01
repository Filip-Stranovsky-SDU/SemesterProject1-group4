namespace WorldOfZuul;



public class Minigame{
    
    private int Ticks;
    private Character petunia = null!;
    private Character player = null!;
    private Missile? missile;
    private bool isRunning;
    private string mapArt = null!;
    private string missileArt = null!;
    public string returnEventName {get; private set;} = null!;


    public Minigame(){

        Console.WriteLine("Petunia starts running at you at incredible speed. It looks as if she was preparing for this moment for years. Luckily you have bunch of darts lying next to you. Maybe those will stop her.");
        Console.WriteLine("Move left and right by using arrows, throw a dart by pressing arrow UP. When ready press ENTER");
        Console.ReadLine();
        Console.WriteLine("");
        for(int i = 0; i<Console.WindowHeight; i++){
            Console.WriteLine("");
        }
        Console.Clear();
        (int n, int m) = Console.GetCursorPosition();

        try {

            isRunning = true;
            
            Setup();

            while(isRunning){
                GameLoop(n, m);
            }
        }
        catch{
            returnEventName = "PetuniaWon";
        }


    }
    private void Setup() {
        List<string> characterArt = new();

        string line;    
        StreamReader sr = new StreamReader(@$".\JsonFiles\art.txt");
        for(int i = 0; i < 20; i++){
            line = sr.ReadLine()!;
            mapArt = mapArt + line + "\n";
        }
        for(int i = 0; i < 3; i++){
            line = sr.ReadLine()!;
            characterArt.Add(line);
        }
        line = sr.ReadLine()!;
        missileArt = line;

        player = new(characterArt, 5, 0, 0);
        petunia = new(characterArt, 10, 1, 5);

        player.Position.X = 10;
        player.Position.Y = 17;
        
        petunia.Position.X = 10;
        petunia.Position.Y = 3;



    }

    private void GameLoop(int n, int m){
        Console.SetCursorPosition(n, m);
        Console.Write(mapArt);
        player.UpdateCharacter(n, m);
        petunia.UpdateCharacter(n, m);
        if(missile != null){
            if(missile.UpdateMissile(petunia, n, m) == false){
                missile = null;
            };
        }
        Console.SetCursorPosition(0, 0);
        if(petunia.Position.X > 35 || petunia.Position.X < 5){
            petunia.direction = -petunia.direction;
            petunia.Position.X = petunia.Position.X + petunia.direction;
            petunia.Position.Y = petunia.Position.Y + 2;
        }
        if(petunia.Position.Y > 15) {
            GameLost();
        }
        if(petunia.Lives == 0){
            GameWon();
        }

        while (Console.KeyAvailable){
            switch (Console.ReadKey(true).Key){
                case ConsoleKey.UpArrow:
                    if(missile == null){
                        missile = new(missileArt, player.Position.X, 16); // TODO CREATE MISSILE PROPERLY
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if(player.Position.X > 5){
                        player.direction = -1;
                    }

                    break;
                
                case ConsoleKey.RightArrow:
                    if(player.Position.X < 35){
                        player.direction = 1;
                    }

                    break;
            }


        }
        Thread.Sleep(TimeSpan.FromMilliseconds(20));


        Ticks = Ticks + 1;
        return;
    }

    private void GameLost(){
        isRunning = false;
        returnEventName = "PetuniaLost";
    }
    private void GameWon(){
        isRunning = false;
        returnEventName = "PetuniaWon";
        
    }


private class Character {
	public (int X, int Y) Position;
	private int UpdateFrame = 0;
	private int FramesToUpdate;
    public int Lives {get; set;}
    private List<string> art;
    public int direction {get; set;}

    public Character(List<string> nart, int framesToUpdate, int ndirection, int lives){
        FramesToUpdate = framesToUpdate;
        art = nart;
        direction = ndirection;
        Lives = lives;

    }

    public void UpdateCharacter(int n, int m) {	

        for(int i = 0; i<3; i++){
            Console.SetCursorPosition(Position.X-2 + n, Position.Y-1+i + m);
            Console.Write("     ");
        }
        if (UpdateFrame >= FramesToUpdate) {
			UpdateFrame = -1;
            Position.X += direction;
		}
        if(Lives == 0){
            direction = 0;
        }
        


        for(int i = 0; i<3; i++){
            Console.SetCursorPosition(Position.X-2 + n, Position.Y-1+i + m);
            Console.Write(art[i]);
        }

        UpdateFrame++;
    
	}
}

private class Missile {
	private (int X, int Y) Position;
	private int UpdateFrame = 0;
	private int FramesToUpdate = 5;
    private string art;


    public Missile(string nart, int x, int y) {
        art = nart;
        Position.X = x;
        Position.Y = y;
    }
    public bool UpdateMissile(Character petunia, int n, int m) {
        
        Console.SetCursorPosition(Position.X+n, Position.Y+m);
        Console.Write(' ');

        if (UpdateFrame >= FramesToUpdate) {
            UpdateFrame = -1;
            Position.Y = Position.Y - 1;
        }


        if(Position.Y == 1) {
            return false;
        }

        int px = petunia.Position.X;
        int py = petunia.Position.Y;
        int x = Position.X;
        int y = Position.Y;

        if(x<px+2 && x>px-2){
            if(y<py+1 && y> py-1){
                
                Console.SetCursorPosition(Position.X+n, Position.Y+m);
                Console.Write(' ');
                petunia.Lives--;
                return false;
            }
        }


        Console.SetCursorPosition(Position.X + n, Position.Y + m);
        Console.Write(art);
        UpdateFrame++;
        return true;
    }

}


}






