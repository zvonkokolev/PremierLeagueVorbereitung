namespace PremierLeague.Core.DataTransferObjects
{
	public class TeamTableRowDto
	{
		public int Id { get; set; }
		public int Rank { get; set; }
		public string Name { get; set; }
		public int Matches { get; set; }
		public int Points => Won * 3 + Drawn;
		public int Won { get; set; }
		public int Lost { get; set; }
		public int Drawn => Matches - Won - Lost;
		public int GoalsPlus { get; set; }
		public int GoalsMinus { get; set; }
		public int GoalsPlusMinus => GoalsPlus - GoalsMinus;
	}
}
