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
            Score = 0;
        }
        public void addScore(int score)
        {
            Score += score;
        } 
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Score: {Score}";
        }

    }
}
