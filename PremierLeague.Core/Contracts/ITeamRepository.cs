using PremierLeague.Core.DataTransferObjects;
using PremierLeague.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PremierLeague.Core.Contracts
{
	public interface ITeamRepository
	{
		Task AddRangeAsync(IEnumerable<Team> teams);
		Task<Team> GetTeamByIdAsync(int teamId);

		Task<List<TeamTableRowDto>> GetTeamTableDtoAsync();
		Task<IList<Team>> GetAllAsync();
	}
}