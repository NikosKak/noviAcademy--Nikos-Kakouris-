using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WorldRank
{
    public class Player :IPlayer
    {
        //orizoyme metavlites toy Player
        public Guid Id { get; }
        public string Name { get;set; }
        public int Score { get => Score; set => Score = value; }
        //orizoume to wallet tou player
        IDictionary<Currency, Wallet> IPlayer.Wallets => throw new NotImplementedException();
        //arxikopoioume ton player
        public Player(string name) {
            Id = Guid.NewGuid();
            Name = name;
        }
        public void UpdateScore(int newscore)
        {
            Score = newscore;
        }
        public override string ToString() =>
            $"[{Id}] {Name} - Score: {Score}";

    }
}
