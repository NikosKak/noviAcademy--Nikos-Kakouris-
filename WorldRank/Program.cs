using WorldRank;
var players = new List<Player>();
var nextId = 1;
IWalletRepository walletRepository = new InMemoryWalletRepository(players);
IPlayerRepository playerRepository = new InMemoryPlayerRepository(players);
while (true)
{
    Console.WriteLine("Enter Option\n1. Add Player\n2. See All Players\n3. Find by Name\n4.Get Wallet of Player\n5. Add Wallet to Player\n6. Exit");
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
            GetWalletOfPlayer();
            break;
        case 5:
            AddWalletToPlayer();
            break;
        case 6:
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

    var player = new Player(name, nextId++);
    player.UpdateScore(score);

    playerRepository.Addplayer(player);
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
void GetWalletOfPlayer()
{
    Console.Write("Give player id: ");
    var id = Console.ReadLine();

    if (int.TryParse(id, out var playerId))
    {
        var wallets = walletRepository.GetByPlayer(playerId);

        foreach (var wallet in wallets)
        {
            Console.WriteLine($"Wallet Number {wallets.IndexOf(wallet)} {wallet.ToString()}");
        }
    }
    else
    {
        Console.Write("Id not a number");
    }
}
void AddWalletToPlayer()
{
    Console.Write("Give player id: ");
    var id = Console.ReadLine();
    Console.Write("Give Currency: 0 - NONE |  1 - EUR | 2 - USD\n");
    var currency = Console.ReadLine();

    Currency cur = Currency.NONE;

    switch (currency)
    {
        case "0":
        default:
            cur = Currency.NONE;
            break;

        case "1":
            cur =
            Currency.EUR;
            break;
        case "2":
            cur =
            Currency.USD;
            break;
    }

    int.TryParse(id, out var playerId);
    {
        walletRepository.Add(new Wallet(10, cur, false), playerId);
    }
}
void FindPlayerById()
{
    Console.Write("Search by Id: ");
    var term = Console.ReadLine() ?? string.Empty;

    if (!int.TryParse(term, out var id))
    {
        Console.WriteLine("Player id is not a number");
    }

    var player = playerRepository.FindPlayer(id);

    if (player is null)
    {
        Console.WriteLine("No player found.");
        return;
    }

    Console.WriteLine(player);
}

void SearchPlayer()
{
    Console.Write("Give player id: ");
    var id = Console.ReadLine();
    int.TryParse(id, out var playerId);
    {
        walletRepository.Add(new Wallet(10, Currency.EUR, false), playerId);
    }
    Console.Write("Id not a number");
}