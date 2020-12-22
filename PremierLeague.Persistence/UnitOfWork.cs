using Microsoft.EntityFrameworkCore;
using PremierLeague.Core.Contracts;
using PremierLeague.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeague.Persistence
{
	public class UnitOfWork : IUnitOfWork
	{
		readonly ApplicationDbContext _dbContext;

		/// <summary>
		/// ConnectionString kommt aus den appsettings.json
		/// </summary>
		public UnitOfWork()
		{
			_dbContext = new ApplicationDbContext();
			Teams = new TeamRepository(_dbContext);
			Games = new GameRepository(_dbContext);
		}

		public ITeamRepository Teams { get; }
		public IGameRepository Games { get; }

		public void Dispose()
		{
			_dbContext.Dispose();
		}

		public async Task MigrateDatabaseAsync() => await _dbContext.Database.MigrateAsync();
		public async Task DeleteDatabaseAsync() => await _dbContext.Database.EnsureDeletedAsync();

		public async Task SaveChangesAsync()
		{
			var entities = _dbContext.ChangeTracker.Entries()
				 .Where(entity => entity.State == EntityState.Added
										|| entity.State == EntityState.Modified)
				 .Select(e => e.Entity);
			foreach (var entity in entities)
			{
				await ValidateEntityAsync(entity);
			}
			await _dbContext.SaveChangesAsync();
		}

		/// <summary>
		/// Hat ein Team in dieser Runde bereits gespielt?
		/// Liegt die Rundenzahl, abhängig von der Teamanzahl im gültigen Bereich?
		/// </summary>
		/// <param name="entity"></param>
		private async Task ValidateEntityAsync(object entity)
		{
			if (entity is Game game)
			{
				if (await _dbContext.Games
					.Include(ht => ht.HomeTeam)
					.Include(gt => gt.GuestTeam)
					.AnyAsync(g => (g.HomeTeam.Id == game.HomeTeam.Id && g.Round == game.Round)
					|| (g.GuestTeam.Id == game.GuestTeam.Id && g.Round == game.Round)))
				{
					throw new ValidationException($"Der Team hat bereits in dieser Runde gespielt");
				}
				//int res = _dbContext.Games.Count();
				//int teamsAnzahl = _dbContext.Games.Count();
				//if (res < 0 || res > ((teamsAnzahl - 1) * teamsAnzahl) - 1)
				//{
				//	throw new ValidationException($"Rundenanzahl liegt nicht in gültigen Bereich");
				//}
			}
			//if (entity is Team team)
			//{
			//	throw new NotImplementedException("DB-Validierungen für Team implementieren!");
			//}
		}
	}
}
