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
            database.CreateTableAsync<CustomDice>().Wait();
        }

        public Task<List<Player>> GetAllPlayersAsync()
        {
            //Get all players.
            return database.Table<Player>().ToListAsync();
        }

        public Task<List<Player>> GetCurrentPlayersAsync()
        {
            //Get all players that are currently playing.
            return database.Table<Player>()
                            .Where(p => p.IsPlaying == true)
                            .ToListAsync();
        }

        public Task<Player> GetPlayerAsync(int id)
        {
            // Get a specific player.
            return database.Table<Player>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SavePlayerAsync(Player player)
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

        public Task<List<CustomDice>> GetDiceAsync()
        {
            return database.Table<CustomDice>().ToListAsync();
        }

        public Task<CustomDice> GetDieAsync(int id)
        {
            // Get custom dice value.
            return database.Table<CustomDice>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveDiceAsync(CustomDice customDice)
        {
                if (customDice.ID != 0)
                {
                    // Update custom dice value.
                    return database.UpdateAsync(customDice);
                }
                else
                {
                // Save initial custom dice value.
                return database.InsertAsync(customDice);
                }
        }


    }
}