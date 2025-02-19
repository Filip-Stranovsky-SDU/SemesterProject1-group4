using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq; // Ensure this is included for FirstOrDefault
using System.Text.Json;

namespace WorldOfZuul;
using LoadData = (Dictionary<string, Room>, Dictionary<string, Interactable>, Dictionary<string, Event>, string);
public class SaveLoad
{
    private const string SaveFilePath = @".\SaveFiles\";
    
    private const string DefaultPath = @".\JsonFiles\";

    public LoadData? LoadGame(Game game, string? saveName)
    {   
        if(saveName == null){
            var events = CreateEvents(game, DefaultPath);
            var rooms = CreateRooms(DefaultPath);
            var interactables = CreateInteractibles(game, DefaultPath);

            return (rooms, interactables, events, "village-of-ix");
        }

        // Guard clause for checking if the save file exists
        if (!Directory.Exists(SaveFilePath+saveName))
        {
            Console.WriteLine("Save file does not exist.");
            return null;
        }

        try
        {
            
            Game.TypewriterEffect("Loading game...");

            var events = CreateEvents(game, $@"{SaveFilePath}{saveName}\");
            var rooms = CreateRooms($@"{SaveFilePath}{saveName}\");
            var interactables = CreateInteractibles(game, $@"{SaveFilePath}{saveName}\");


            string? jsonString = File.ReadAllText($@"{SaveFilePath}{saveName}\stats.json"); // forward '/' to not use @
            
            Resources worldStats;
            string roomID;
            KeyValuePair<string, Resources> temp;
            temp = JsonSerializer.Deserialize<KeyValuePair<string, Resources>>(jsonString);
            roomID = temp.Key;
            worldStats = temp.Value;

            game.Player.WorldStats = worldStats;

            return (rooms, interactables, events, roomID);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading game: {ex.Message}");
            return null;
        }
    }

    public void SaveGame(Game game, string? saveName)
    {
        if(saveName == null){
            Console.WriteLine("Save name must be valid");
        }
        try
        {
            // Ensure the directory exists
            
            Directory.CreateDirectory(@$"{SaveFilePath}\{saveName}");

            string sRooms = JsonSerializer.Serialize(game.Rooms);
            File.WriteAllText(@$"{SaveFilePath}\{saveName}\rooms.json", sRooms);

            string sInteractables = JsonSerializer.Serialize(game.Interactables);
            File.WriteAllText(@$"{SaveFilePath}\{saveName}\npcs.json", sInteractables);

            Dictionary<string, Event> events = new();
            Dictionary<string, TextEvent> tevents = new();
            Dictionary<string, QuizEvent> qevents = new();
            Dictionary<string, MultipleChoiceEvent> mcevents = new();
            foreach(var (id, e) in game.Events){
                switch (e) {
                    case TextEvent ev:
                        tevents.Add(id, ev);
                        break;

                    case QuizEvent ev:
                        qevents.Add(id, ev);
                        break;

                    case MultipleChoiceEvent ev:
                        mcevents.Add(id, ev);
                        break;
                    case Event ev:
                        events.Add(id, ev);
                        break;
                }
            }
            
            string sEvents = JsonSerializer.Serialize(events, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(@$"{SaveFilePath}\{saveName}\events.json", sEvents);
            
            string sTEvents = JsonSerializer.Serialize(tevents, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(@$"{SaveFilePath}\{saveName}\textEvents.json", sTEvents);
            
            string sQEvents = JsonSerializer.Serialize(qevents, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(@$"{SaveFilePath}\{saveName}\quizEvents.json", sQEvents);
            
            string sMCEvents = JsonSerializer.Serialize(mcevents, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(@$"{SaveFilePath}\{saveName}\multipleChoice.json", sMCEvents);



            KeyValuePair<string, Resources> temp = new(game.CurrentRoom.RoomId, game.Player.WorldStats);
            string sWorldStats = JsonSerializer.Serialize(temp, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(@$"{SaveFilePath}\{saveName}\stats.json", sWorldStats);
            
            //File.WriteAllText(SaveFilePath, jsonString);
            Game.TypewriterEffect("Game saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving game: {ex.Message}");
        }
    }

    private Dictionary<string, Room> CreateRooms(string filePath)
    {
        string? jsonString = File.ReadAllText(filePath+"rooms.json"); 
        return JsonSerializer.Deserialize<Dictionary<string, Room>>(jsonString)??throw new("Failed to load rooms"); // if this is null, it returns this value
    }
    
    private Dictionary<string, Interactable> CreateInteractibles(Game gameRef, string filePath)
    {
        string jsonString = File.ReadAllText(filePath+"npcs.json");
        

        var options = new JsonSerializerOptions
        {
            IncludeFields = true
        };

        var Interactables = JsonSerializer.Deserialize<Dictionary<string, Interactable>>(jsonString, options)??throw new("Failed to load interactables");
        
        foreach(var entry in Interactables){
            entry.Value.setupGameRef(gameRef);
        }

        return Interactables;
    }

    private Dictionary<string, Event> CreateEvents(Game gameRef, string filePath)
    {   
        string? jsonString = File.ReadAllText(filePath+"events.json");
        var options = new JsonSerializerOptions{ IncludeFields = true };

        var Events = JsonSerializer.Deserialize<Dictionary<string, Event>>(jsonString, options)??throw new("Failed to load events");

        jsonString = File.ReadAllText(filePath+"textEvents.json");
        var textEvents = JsonSerializer.Deserialize<Dictionary<string, TextEvent>>(jsonString, options)??throw new("Failed to load text events");
        
        jsonString = File.ReadAllText(filePath+"quizEvents.json"); 
        var quizEvents = JsonSerializer.Deserialize<Dictionary<string, QuizEvent>>(jsonString, options)??throw new("Failed to load quiz events");

        jsonString = File.ReadAllText(filePath+"multipleChoice.json"); 
        var multipleChoice = JsonSerializer.Deserialize<Dictionary<string, MultipleChoiceEvent>>(jsonString, options)??throw new("Failed to load mutliple choice events");

        foreach(var entry in Events){
            entry.Value.setupGameRef(gameRef);
        }

        foreach(var entry in textEvents){
            Events.Add(entry.Key, entry.Value);
            entry.Value.setupGameRef(gameRef);
        }
        
        foreach(var entry in quizEvents){
            Events.Add(entry.Key, entry.Value);
            entry.Value.setupGameRef(gameRef);
        }

        foreach(var entry in multipleChoice){
            Events.Add(entry.Key, entry.Value);
            entry.Value.setupGameRef(gameRef);
        }
        return Events;
        
    }
}
