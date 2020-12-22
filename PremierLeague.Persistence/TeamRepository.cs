using Microsoft.EntityFrameworkCore;
using PremierLeague.Core.Contracts;
using PremierLeague.Core.DataTransferObjects;
using PremierLeague.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeague.Persistence
{
  public class TeamRepository : ITeamRepository
  {
    private readonly ApplicationDbContext _dbContext;

    public TeamRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }

		public async Task AddRangeAsync(IEnumerable<Team> teams)
			=> await _dbContext
			.AddRangeAsync(teams)
			;
		public async Task<Team> GetTeamByIdAsync(int teamId)
			=> await _dbContext
			.Teams.Where(t => t.Id == teamId)
			.FirstOrDefaultAsync()
			;
		public async Task<List<TeamTableRowDto>> GetTeamTableDtoAsync()
		{
			var result = _dbContext.Teams
			.Select(t => new TeamTableRowDto
			{
				Id = t.Id,
				Name = t.Name,
				Matches = t.AwayGames.Count() + t.HomeGames.Count(),
				Won = t.HomeGames.Where(m => m.HomeGoals > m.GuestGoals).Count()
					+ t.AwayGames.Where(m => m.HomeGoals < m.GuestGoals).Count(),
				Lost = t.HomeGames.Where(m => m.HomeGoals < m.GuestGoals).Count()
					+ t.AwayGames.Where(m => m.HomeGoals > m.GuestGoals).Count(),
				GoalsPlus = t.HomeGames.Sum(g => g.HomeGoals) + t.AwayGames.Sum(g => g.GuestGoals),
				GoalsMinus = t.HomeGames.Sum(g => g.GuestGoals) + t.AwayGames.Sum(g => g.HomeGoals)
			})
			.AsEnumerable()
			.OrderByDescending(t => t.Points)
			.ThenByDescending(t => t.GoalsPlusMinus)
			.ToList()
			;
			int counter = 1;
			foreach (var team in result)
			{
				team.Rank = counter++;
			}
			return result;
		}
		public async Task<IList<Team>> GetAllAsync()
			=> await _dbContext.Teams
			.OrderBy(t => t.Name)
			.ToListAsync()
			;
	}
}