using Microsoft.EntityFrameworkCore;
using PremierLeague.Core.Contracts;
using PremierLeague.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeague.Persistence
{
	public class GameRepository : IGameRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public GameRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task AddRangeAsync(IEnumerable<Game> games)
			  => await _dbContext
			  .AddRangeAsync(games)
			;
		public async Task<Game> GetGameByIdAsync(int id)
			=> await _dbContext
			.Games.Where(g => g.Id == id)
			.FirstOrDefaultAsync()
			;
		public async Task<IEnumerable<Game>> GetAllWithTeamsAsync()
			=> await _dbContext.Games
			.Include(g => g.HomeTeam)
			.Include(g => g.GuestTeam)
			.ToListAsync()
			;
		public async Task AddGameAsync(Game tempGame)
			=> await _dbContext.Games
			.AddAsync(tempGame)
			;

		public async Task<IEnumerable<Game>> GetGameByRoundAsync(int round)
			=> await _dbContext.Games
			.Include(g => g.HomeTeam)
			.Include(g => g.GuestTeam)
			.Where(g => g.Round == round)
			.ToListAsync()
			;
	}
}