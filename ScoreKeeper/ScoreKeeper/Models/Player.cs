using System;
using SQLite;

namespace ScoreKeeper.Models
{
    public class Player
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}