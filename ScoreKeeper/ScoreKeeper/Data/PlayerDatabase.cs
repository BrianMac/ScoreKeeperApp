using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using ScoreKeeper.Models;

namespace ScoreKeeper.Data
{
    public class PlayerDatabase
    {
        readonly SQLiteAsyncConnection database;

        public PlayerDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Player>().Wait();
        }

        public Task<List<Player>> GetNotesAsync()
        {
            //Get all players.
            return database.Table<Player>().ToListAsync();
        }

        public Task<Player> GetPlayerAsync(int id)
        {
            // Get a specific player.
            return database.Table<Player>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveNoteAsync(Player player)
        {
            if (player.ID != 0)
            {
                // Update an existing player.
                return database.UpdateAsync(player);
            }
            else
            {
                // Save a new player.
                return database.InsertAsync(player);
            }
        }

        public Task<int> DeletePlayerAsync(Player player)
        {
            // Delete a player.
            return database.DeleteAsync(player);
        }
    }
}