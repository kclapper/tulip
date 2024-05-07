﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tulip.Models
{
	public class LeaderBoader
	{
		[Key]
		public int Id { get; set; }
		public string? Username { get; set; }
		public int? Point { get; set; }
		public string? CaseStudy { get; set; }
		// [NotMapped]
		public string AvatarUrl { get; set; }
	}
}
