using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gamification.UI.Data
{
	public class TasksResponse
	{
		[Key]
		public int Id { get; set; }
		[ForeignKey("TasksId")]
		public int TasksId { get; set; }
		public virtual Tasks Tasks { get; set; }
		public string RespondantName { get; set; }
		public int Score { get; set; }
		public bool IsComplete { get; set; }
		public IEnumerable<Tasks> TasksList { get; set; }
	}
}
