using System;
using SQLite;

namespace ScoreKeeper.Models
{
    public class Player
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string AvatarFileName { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public bool IsPlaying { get; set; }
        public int AvatarID { get; set; }
        public string ItemBackground { get; set; }
        public string AvatarBackground { get; set; }
        public int CurrentScore { get; set; }
        public int NumOfWins { get; set; }
        public int TurnOrder { get; set; }
    }

    public class CustomDice
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int LowEnd { get; set; }
        public int HighEnd { get; set; }
    }


}