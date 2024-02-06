using System.ComponentModel.DataAnnotations;

namespace Gamification.UI.Data
{
	public class Tasks
	{
		[Key]
		public int Id { get; set; }
		public string Description { get; set; }
		public string StepNumber { get; set; }
	}
}
