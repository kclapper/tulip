using Gamification.UI.Data;

namespace Gamification.UI.Models
{
	public class TaskResponseVM
	{
		public int Id { get; set; }
		public int TasksId { get; set; }
		public virtual Tasks Tasks { get; set; }
		public string RespondantName { get; set; }
		public int Score { get; set; }
		public bool IsComplete { get; set; }
	}
}
