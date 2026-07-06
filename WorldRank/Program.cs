using System;
using System.Numerics;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using WorldRank;
List<Player> players = new List<Player>();
int nextId = 1;
while (true)
{
    Console.WriteLine("Enter Option\n1. Add Player\n2. See All Players\n3. Find by Name\n4. Exit");
    int option = int.Parse(Console.ReadLine());
    switch (option)
    {
        case 1:
            AddPlayer();
            break;
        case 2:
            SeeAllPlayers();
            break;
        case 3:
            FindByName();
            break;
        case 4:
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}
void AddPlayer()
{
    Console.WriteLine("Give Player Name:");
    string name = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Name cannot be empty. Please try again.");
        return;
    }
    Console.WriteLine("Give Player Score:");
    string score = Console.ReadLine();
    if (!int.TryParse(score, out int parsedScore))
    {
        Console.WriteLine("Invalid score. Please enter a valid integer.");
        return;
    }
    var player = new Player(name);
    player.addScore(parsedScore);
    players.Add(player);
    Console.WriteLine("Player added successfully.");
}
void SeeAllPlayers()
{
    if (players.Count == 0)
    {
        Console.WriteLine("No players found.");
        return;
    }
    foreach (var player in players)
    {
        Console.WriteLine(player);
    }
}
void FindByName()
{
    Console.WriteLine("Enter Player Name to search:");
    string name = Console.ReadLine();
    var foundPlayers = players.Where(p=>p.Name.Equals(name,StringComparison.OrdinalIgnoreCase)).ToList();
    if (foundPlayers.Count == 0)
    {
        Console.WriteLine("No players found with that name.");
        return;
    }
    foreach (var player in foundPlayers)
    {
        Console.WriteLine(player);
    }
}