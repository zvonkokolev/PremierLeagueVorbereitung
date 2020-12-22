using PremierLeague.Core.Contracts;
using PremierLeague.Core.DataTransferObjects;
using PremierLeague.Persistence;
using PremierLeague.Wpf.Common;
using PremierLeague.Wpf.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PremierLeague.Wpf.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		private TeamTableRowDto _selectedTeam;
		private RelayCommand _cmdAddTeam;

		public ObservableCollection<TeamTableRowDto> Teams { get; set; }
		public TeamTableRowDto SelectedTeam
		{
			get => _selectedTeam;
			set
			{
				_selectedTeam = value;
				OnPropertyChanged(nameof(SelectedTeam));
			}
		}

		public RelayCommand CmdAddTeam
		{
			get => _cmdAddTeam;
			set
			{
				_cmdAddTeam = value;
				OnPropertyChanged(nameof(CmdAddTeam));
			}
		}

		public MainViewModel() : base(null)
		{
			LoadCommands();
		}
		public MainViewModel(IWindowController windowController) : base(windowController)
		{
			LoadCommands();
		}

		/// <summary>
		/// Erstellt die notwendigen Commands.
		/// </summary>
		private void LoadCommands()
		{
			CmdAddTeam = new RelayCommand(
				execute: async _ =>
				{
					using IUnitOfWork unitOfWork = new UnitOfWork();
					WindowController controller = new WindowController();
					controller.ShowWindow(await AddGameViewModel.CreateAsync(controller));
				},
				canExecute: _ => SelectedTeam != null
				)
				;
		}

		/// <summary>
		/// Asynchrones Laden von Daten für das ViewModel.
		/// Wird in CreateAsync(..) aufgerufen.
		/// </summary>
		/// <returns></returns>
		private async Task LoadDataAsync()
		{
			using IUnitOfWork unitOfWork = new UnitOfWork();
			List<TeamTableRowDto> teamTableRowDtos = await unitOfWork.Teams.GetTeamTableDtoAsync();
			Teams = new ObservableCollection<TeamTableRowDto>(teamTableRowDtos);
			SelectedTeam = Teams.FirstOrDefault();
		}

		public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			throw new NotImplementedException();
		}

		public static async Task<MainViewModel> CreateAsync(IWindowController windowController)
		{
			var viewModel = new MainViewModel(windowController);
			await viewModel.LoadDataAsync();
			return viewModel;
		}
	}
}
