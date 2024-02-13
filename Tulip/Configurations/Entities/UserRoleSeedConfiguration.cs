﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/* 
This seed is used for a fresh database that needs admin and a user credential 
pre added. This should not be used after initial launch. See ApplicationDbContext.cs
for configuration.
*/

namespace Tulip.Configurations.Entities
{
	public class UserRoleSeedConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
		{
			builder.HasData(
				new IdentityUserRole<string>
				{
					RoleId = "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
					UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
				},
				new IdentityUserRole<string>
				{
					RoleId = "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
					UserId = "9e224968-33e4-4652-b7b7-8574d048cdb9"
				}
			);
		}
	}

}
