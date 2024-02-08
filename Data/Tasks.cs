using System.ComponentModel.DataAnnotations;

namespace Tulip.Data
{
	public class Tasks
	{
		[Key]
		public int Id { get; set; }
		public string Description { get; set; }
		public string StepNumber { get; set; }
	}
}
