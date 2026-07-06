using System;
using System.Numerics;
using WorldRank;
while (true)
{
    Console.WriteLine("Enter Option\n1. Add Player\n2. See All Players\n3. Find by Name\n4. Exit");
    int option = int.Parse(Console.ReadLine());
    switch (option)
    {
        case 1:
            Player.AddPlayer();
            break;
        case 2:
            Player.SeeAllPlayers();
            break;
        case 3:
            Player.FindByName();
            break;
        case 4:
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}