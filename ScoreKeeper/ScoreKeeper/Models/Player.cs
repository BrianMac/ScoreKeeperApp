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

        //public string Name { get; set; }
        //public int AvatarID { get; set; }
        //public int CurrentScore { get; set; }

    }

    public class CustomDice
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int LowEnd { get; set; }
        public int HighEnd { get; set; }
    }


}