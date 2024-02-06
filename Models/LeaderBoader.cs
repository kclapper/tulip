using System.ComponentModel.DataAnnotations;

namespace Gamification.UI.Models
{
	public class LeaderBoader
	{
		[Key]
		public int Id { get; set; }
		public string? Username { get; set; }
		public int? Point { get; set; }
		public string? CaseStudy { get; set; }
	}
}
