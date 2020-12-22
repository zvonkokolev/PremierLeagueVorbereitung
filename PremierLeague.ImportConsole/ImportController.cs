using PremierLeague.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace PremierLeague.ImportConsole
{
	public static class ImportController
	{
		public async static Task<IEnumerable<Game>> ReadFromCsvAsync()
		{
			string[][] matrix = await MyFile.ReadStringMatrixFromCsvAsync("PremierLeague.csv", false);
			List<Team> teams = matrix
				.Select(s => s[1])
				.Distinct()
				.Select(t => new Team { Name = t })
				.ToList()
				;
			return matrix.Select(g => new Game
			{
				Round = int.Parse(g[0]),
				HomeTeam = teams.Single(t => t.Name.Equals(g[1])),
				GuestTeam = teams.Single(t => t.Name.Equals(g[2])),
				HomeGoals = int.Parse(g[3]),
				GuestGoals = int.Parse(g[4])
			})
			.ToList()
			;
		}
	}
}
