using WorldRank.PlayerRepository;
using WorldRank.WalletRepository;
using WorldRank.Wallets;
using WorldRank.Currencies;
using WorldRank.Players;
using Microsoft.VisualBasic.FileIO;
var players = new List<Player>();
var nextId = 1;
IWalletRepository walletRepository = new InMemoryWalletRepository(players);
IPlayerRepository playerRepository = new InMemoryPlayerRepository(players);
while (true)
{
    Console.WriteLine("\n=== WorldRank Player Registry ===");
    Console.WriteLine("1. Add player");
    Console.WriteLine("2. List all players");
    Console.WriteLine("3. Find player by name");
    Console.WriteLine("4. Add Wallet to player");
    Console.WriteLine("5. Get Player Wallets");
    Console.WriteLine("6. Delete Player");
    Console.WriteLine("7. Find Player by ID");
    Console.WriteLine("8. Update Player Score");
    Console.WriteLine("0. Exit");
    Console.Write("> ");

    Action? action = Console.ReadLine() switch
    {
        "1" => AddPlayer,
        "2" => SeeAllPlayers,
        "3" => FindByName,
        "4" => AddWalletToPlayer,
        "5" => GetWalletOfPlayer,
        "6" => DeletePlayer,
        "7" => FindPlayerById,
        "8" => UpdatePlayerScore,
        "0" => null,
        _ => () => Console.WriteLine("Unknown option.")
    };

    if (action is null)
        return; // "0" selected — exit

    action();
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

    var player = new Player(name,nextId++);
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
void AddWalletToPlayer()
{
    Console.Write("Give player id: ");
    var id = Console.ReadLine();
    Console.Write("Give Currency: 1 - EUR | 2 - USD\n");
    var currency = Console.ReadLine();

    Currency cur = Currency.EUR;

    switch (currency)
    {
        case "1":
        default:
            cur = Currency.EUR;
            break;
        case "2":
            cur =
            Currency.USD;
            break;
    }

    int.TryParse(id, out var playerId);
    {
        walletRepository.AddWallet(new Wallet(10, cur, false), playerId);
    }
}
void GetWalletOfPlayer()
{
    Console.Write("Give player id: ");
    var id = Console.ReadLine();

    if (int.TryParse(id, out var playerId))
    {
        var wallets = walletRepository.GetPlayer(playerId);

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
void DeletePlayer()
{
    Console.Write("Give player id: ");
    var id = Console.ReadLine();
    if (int.TryParse(id, out var playerId))
    {
        playerRepository.DeletePlayer(playerId);
        Console.WriteLine($"Player with ID {playerId} deleted successfully.");
    }
    else
    {
        Console.WriteLine("Invalid ID. Please enter a valid number.");
    }
}
void FindPlayerById()
{
    Console.Write("Give player id: ");
    var id = Console.ReadLine();
    if (int.TryParse(id, out var playerId))
    {
        var player = playerRepository.FindPlayer(playerId);
        if (player != null)
        {
            Console.WriteLine(player);
        }
        else
        {
            Console.WriteLine($"No player found with ID {playerId}.");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID. Please enter a valid number.");
    }
}
void UpdatePlayerScore()
{
    Console.Write("Give player id: ");
    var id = Console.ReadLine();
    if (int.TryParse(id, out var playerId))
    {
        var player = playerRepository.FindPlayer(playerId);
        if (player != null)
        {
            Console.Write("Enter new score: ");
            var scoreInput = Console.ReadLine();
            if (int.TryParse(scoreInput, out var newScore))
            {
                player.UpdateScore(newScore);
                Console.WriteLine($"Player {player.Name}'s score updated to {newScore}.");
            }
            else
            {
                Console.WriteLine("Invalid score. Please enter a valid number.");
            }
        }
        else
        {
            Console.WriteLine($"No player found with ID {playerId}.");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID. Please enter a valid number.");
    }
}