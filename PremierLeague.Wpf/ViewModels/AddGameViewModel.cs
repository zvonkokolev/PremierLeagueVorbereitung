using PremierLeague.Core.Contracts;
using PremierLeague.Core.Entities;
using PremierLeague.Persistence;
using PremierLeague.Wpf.Common;
using PremierLeague.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PremierLeague.Wpf.ViewModels
{
	public class AddGameViewModel : BaseViewModel
	{
		private Game _game = new Game();
		private RelayCommand _cmdSaveGame;
		private string _homeTeam;
		private string _guestTeam;
		private int _homeGoals;
		private int _guestGoals;
		private int _round;
		private Team _selectedHomeTeam;
		private Team _selectedGuestTeam;

		public ObservableCollection<Game> Games { get; private set; }
		public ObservableCollection<Team> Teams { get; private set; }
		public Team SelectedTeam { get; private set; }

		public Game Game
		{
			get => _game;
			set
			{
				_game = value;
				OnPropertyChanged(nameof(Game));
			}
		}
		public string HomeTeam
		{
			get => _homeTeam;
			set
			{
				_homeTeam = value;
				OnPropertyChanged(nameof(HomeTeam));
			}
		}
		public string GuestTeam
		{
			get => _guestTeam;
			set
			{
				_guestTeam = value;
				OnPropertyChanged(nameof(GuestTeam));
			}
		}
		public int HomeGoals
		{
			get => _homeGoals;
			set
			{
				_homeGoals = value;
				OnPropertyChanged(nameof(HomeGoals));
			}
		}
		public int GuestGoals
		{
			get => _guestGoals;
			set
			{
				_guestGoals = value;
				OnPropertyChanged(nameof(GuestGoals));
			}
		}
		public int Round
		{
			get => _round;
			set
			{
				_round = value;
				OnPropertyChanged(nameof(Round));
			}
		}
		public Team SelectedHomeTeam
		{
			get => _selectedHomeTeam;
			set
			{
				_selectedHomeTeam = value;
				OnPropertyChanged(nameof(SelectedHomeTeam));
			}
		}
		public Team SelectedGuestTeam
		{
			get => _selectedGuestTeam;
			set
			{
				_selectedGuestTeam = value;
				OnPropertyChanged(nameof(SelectedGuestTeam));
			}
		}
		public RelayCommand CmdSaveGame
		{
			get => _cmdSaveGame;
			set
			{
				_cmdSaveGame = value;
				OnPropertyChanged(nameof(CmdSaveGame));
			}
		}

		public AddGameViewModel() : base(null)
		{
			LoadCommands();
		}
		public AddGameViewModel(IWindowController windowController) : base(windowController)
		{
			LoadCommands();
		}

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			throw new NotImplementedException();
		}
		private async Task LoadDataAsync()
		{
			using IUnitOfWork unitOfWork = new UnitOfWork();
			var games = await unitOfWork.Games.GetAllWithTeamsAsync();
			var teams = await unitOfWork.Teams.GetAllAsync();

			Games = new ObservableCollection<Game>(games);
			Teams = new ObservableCollection<Team>(teams);
			SelectedHomeTeam = Teams.FirstOrDefault();
			SelectedGuestTeam = Teams.FirstOrDefault();
		}
		public static async Task<BaseViewModel> CreateAsync(IWindowController controller)
		{
			var viewModel = new AddGameViewModel(controller);
			await viewModel.LoadDataAsync();
			return viewModel;
		}
		private void LoadCommands()
		{
			CmdSaveGame = new RelayCommand(
				execute: async _ =>
				{
					using IUnitOfWork unitOfWork = new UnitOfWork();
					Game.Round = Round;
					Game.HomeTeam = SelectedHomeTeam;
					Game.GuestTeam = SelectedGuestTeam;
					Game.HomeGoals = HomeGoals;
					Game.GuestGoals = GuestGoals;

					await unitOfWork.Games.AddGameAsync(Game);
					try
					{
						await unitOfWork.SaveChangesAsync();
					}
					catch (Exception e)
					{
						throw new ValidationException(e.Message);
					}
				},
				canExecute: _ => Game != null)
				;
		}
	}
}
