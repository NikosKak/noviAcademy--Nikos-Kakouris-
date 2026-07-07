using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WorldRank
{
    public class Player
    {
        public Guid Id { get; }
        public string Name { get;set; }
        public int Score { get; private set; }
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
