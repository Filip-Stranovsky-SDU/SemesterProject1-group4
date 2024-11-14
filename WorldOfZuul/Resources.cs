namespace WorldOfZuul;

public class Resources{

    public int Money {get; set;} = 0;
    public int Environment {get; set;} = 0;
    public int Social {get; set;} = 0;


    public void Change(Resources changeInResources){

        Money += changeInResources.Money;
        Environment += changeInResources.Environment;
        Social += changeInResources.Social;
    }

    public string Display(){
        string result = "";
        

        foreach(var property in this.GetType().GetProperties()){
            result += @$"{property.Name}: {property.GetValue(this)} | ";
            
        }

        
        result = result.Remove(result.Length - 2);
        return result;

    }

}