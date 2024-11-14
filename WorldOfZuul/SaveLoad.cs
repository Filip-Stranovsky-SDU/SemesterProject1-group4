using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Ensure this is included for FirstOrDefault
using System.Text.Json;

namespace WorldOfZuul
{
    public static class SaveLoad
    {
        private const string SaveFilePath = @".\SaveFiles\save.json";

        public static bool LoadGame(Game game)
        {
            if (!File.Exists(SaveFilePath))
            {
                Console.WriteLine("Save file does not exist.");
                return false;
            }

            try
            {
                // Read JSON from file
                string jsonString = File.ReadAllText(SaveFilePath);

                // Deserialize JSON to SaveData
                SaveData? data = JsonSerializer.Deserialize<SaveData>(jsonString);

                if (data != null)
                {
                    Game.TypewriterEffect("Loading game...");

                    // Restore player's resources
                    game.Player.Resources = new Dictionary<string, int>(data.PlayerResources);

                    // Restore current room using RoomId for uniqueness
                    var room = game.Rooms?.FirstOrDefault(r => r.Value.RoomId == data.CurrentRoomId).Value;
                    if (room != null)
                    {
                        game.currentRoom = room;
                    }
                    else
                    {
                        Console.WriteLine("Error: Saved room does not exist.");
                        return false;
                    }

                    // Restore event states
                    if (game.Events != null)
                    {
                        foreach (var evtState in data.EventStates)
                        {
                            if (game.Events.ContainsKey(evtState.Key))
                            {
                                game.Events[evtState.Key].IsActive = evtState.Value;
                            }
                        }
                    }

                    Game.TypewriterEffect("Game loaded successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to load game state.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game: {ex.Message}");
                return false;
            }
        }

        public static void SaveGame(Game game)
        {
            try
            {
                // Ensure the directory exists
                string directory = Path.GetDirectoryName(SaveFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    Console.WriteLine($"Created directory: {directory}");
                }

                // Initialize SaveData with current game state
                SaveData data = new SaveData
                {
                    CurrentRoomId = game.currentRoom?.RoomId ?? "village-of-ix", // Use default room ID
                    PlayerResources = new Dictionary<string, int>(game.Player.Resources),
                    EventStates = game.Events?.ToDictionary(e => e.Key, e => e.Value.IsActive) ?? new Dictionary<string, bool>()
                };

                // Configure JSON serializer options
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                // Serialize SaveData to JSON and write to file
                string jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(SaveFilePath, jsonString);
                Game.TypewriterEffect("Game saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game: {ex.Message}");
            }
        }

        // Nested SaveData class to represent the saved state
        private class SaveData
        {
            public string CurrentRoomId { get; set; } = "";
            public Dictionary<string, int> PlayerResources { get; set; } = new();
            public Dictionary<string, bool> EventStates { get; set; } = new();
        }
    }
}