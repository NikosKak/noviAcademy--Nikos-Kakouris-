using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WorldRank
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        private static List<Player> players = new List<Player>();
        private static int nextId = 1;
        private static Player player;

        public static void AddPlayer()
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
            var player = new Player
            {
                Id = nextId++,
                Name = name,
                Score = parsedScore
            };
            players.Add(player);
            Console.WriteLine("Player added successfully.");
        }
        public static void SeeAllPlayers()
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
        public static void FindByName()
        {
            Console.WriteLine("Enter Player Name to search:");
            string name = Console.ReadLine();
            var foundPlayers = players.FindAll(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
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

        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Score: {Score}";
        }

    }
}
