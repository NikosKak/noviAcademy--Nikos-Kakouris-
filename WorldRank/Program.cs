using WorldRank;
var players = new List<Player>();
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
    Console.Write("Name: ");
    var name = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Name cannot be empty.");
        return;
    }

    Console.Write("Score: ");
    var scoreInput = Console.ReadLine();
    if (!int.TryParse(scoreInput, out var score))
    {
        Console.WriteLine("Score must be a whole number.");
        return;
    }

    var player = new Player(name);
    player.UpdateScore(score);

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
    var name = Console.ReadLine() ?? string.Empty;
    var foundPlayers = players.Where(p=>p.Name.Equals(name,StringComparison.OrdinalIgnoreCase)).ToList();
    var player = players
            .FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    if (player is null)
    {
        Console.WriteLine("No player found.");
        return;
    }

    Console.WriteLine(player);
}