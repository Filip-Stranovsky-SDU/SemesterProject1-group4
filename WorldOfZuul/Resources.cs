namespace WorldOfZuul;

public class Resources{

    public int Money {get; set;} = 0;
    public int Environment {get; set;} = 0;
    public int Social {get; set;} = 0;

    public Resources(){}

    public Resources(int money, int environment, int social){
        Money = money;
        Environment = environment;
        Social = social;
    }
    public Resources(Dictionary<string, int> resources){
        Money = resources["Money"];
        Environment = resources["Environment"];
        Social = resources["Social"];
    }


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

    public Dictionary<string, int> GetDictionary(){
        Dictionary<string, int> output = new();

        foreach(var property in this.GetType().GetProperties()){
            output[property.Name] = (int)property.GetValue(this);
            
        }

        return output;
    }

}