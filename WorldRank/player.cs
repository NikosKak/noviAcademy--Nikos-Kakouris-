using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WorldRank
{
    public class Player
    {
        public int Id { get;private set; }
        public string Name { get;private set; }
        public int Score { get; private set; }
        public Player(int id, string name, int score) {
            Id = id;
            Name = name;
            Score = score;
        }
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Score: {Score}";
        }

    }
}
