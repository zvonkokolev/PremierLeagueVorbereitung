using System.Collections.Generic;
using System.Threading.Tasks;
using PremierLeague.Core.Entities;

namespace PremierLeague.Core.Contracts
{
	public interface IGameRepository
	{
		Task AddRangeAsync(IEnumerable<Game> games);
		Task<Game> GetGameByIdAsync(int id);
		Task<IEnumerable<Game>> GetAllWithTeamsAsync();
		Task AddGameAsync(Game tempGame);
	}
}