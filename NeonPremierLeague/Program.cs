using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using NeonPremierLeague.Models;

 public class Program
{
    static void Main(string[] args)
    {
        // Initialise variables
        string json;
        double budget = 100.0;
        int teamSize = 15;
        int maxPlayersPerClub = 3;
        List<Player> selectedTeam = new();
        Dictionary<string, int> clubCount = new();
        List<Player> players = new();
        string filePath = "players.json";


        // Read players from JSON file, serialise if found
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Players data file not found.");
            return;
        }
        try
        {
             json = File.ReadAllText("players.json");
        }
        catch (IOException ex)
        {
            throw new IOException($"Error reading the file '{filePath}': {ex.Message}");
        }

        players = JsonSerializer.Deserialize<List<Player>>(json);

        if(players == null || players.Count == 0)
        {
            Console.WriteLine("No players found in the data file.");
            return;
        }   

        // Player Selection 
        Console.WriteLine("Welcome to Neon Premier League Fantasy Football");
        Console.WriteLine($"Your budget is {budget} million. Select your team of {teamSize} players.");

        while (selectedTeam.Count < teamSize)
        {
            // Display available players
            Console.WriteLine("\nAvailable Players:");
            foreach (var player in players)
            {
                Console.WriteLine($"{player.id}. {player.name} ({player.club}, {player.position}) - Price: {player.price}");
            }

            // Get player selection from user
            Console.Write($"\nEnter the ID of the player you want to select, enter 0 to finish selection early: ");
            if (!int.TryParse(Console.ReadLine(), out int playerId))
            {
                Console.WriteLine("Invalid input. Please enter a valid player ID.");
                continue;
            }

            if (playerId == 0)
            {
                break;
            }

            Player selectedPlayer = players.FirstOrDefault(p => p.id == playerId);

            if (selectedPlayer == null)
            {
                Console.WriteLine("Invalid Player ID, please enter a valid ID");
                continue;
            }

            // Validation rules
            double currentTeamValue = selectedTeam.Sum(p => p.price);
            if (currentTeamValue + selectedPlayer.price > budget)
            {
                Console.WriteLine("Insufficient budget to select this player");
                continue;
            }

            if (!clubCount.ContainsKey(selectedPlayer.club))
            {
                clubCount[selectedPlayer.club] = 0;
            }

            if (clubCount[selectedPlayer.club] >= maxPlayersPerClub)
            {
                Console.WriteLine($"You have reached the maximum number of players from {selectedPlayer.club}.");
                continue;
            }

            // Add player to the team
            selectedTeam.Add(selectedPlayer);
            clubCount[selectedPlayer.club]++;
            players.Remove(selectedPlayer); // Remove selected player from available players
            Console.WriteLine($"{selectedPlayer.name} added to your team.");
            Console.WriteLine($"Remaining budget: {budget - selectedTeam.Sum(p => p.price)} million");
            Console.WriteLine($"Players selected: {selectedTeam.Count}/{teamSize}");
        }

        // Display selected team
        Console.WriteLine("\nYour Selected Team:");
        double finalTeamValue = 0;
        foreach (var player in selectedTeam)
        {
            Console.WriteLine($"{player.name} ({player.club}, {player.position}) - Price: {player.price}");
            finalTeamValue += player.price;
        }

        Console.WriteLine($"\nTotal team value: {finalTeamValue} million");
        Console.WriteLine("Thank you for playing");
    }
}
