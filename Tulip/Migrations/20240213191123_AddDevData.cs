using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tulip.Migrations
{
    /// <inheritdoc />
    public partial class AddDevData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cbc43a8e-f7bb-4445-baaf-1add431ffbbf", "8e445865-a24d-4543-a6c6-9443d048cdb9" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "cac43a6e-f7bb-4448-baaf-1add431ccbbf", "9e224968-33e4-4652-b7b7-8574d048cdb9" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
                column: "ConcurrencyStamp",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(21)",
                oldMaxLength: 21);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
                column: "ConcurrencyStamp",
                value: "a17d64c5-d3b5-40cd-af2f-04386a783a1a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
                column: "ConcurrencyStamp",
                value: "596c46d2-9546-438b-9d82-03ad2002fe7d");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ApplicationServer", "ClientId", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserId", "UserName" },
                values: new object[,]
                {
                    { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "e45z.4.ucc.md/sap", 111, "776271d5-eab9-4283-a5c3-5e516fce02fb", "ApplicationUser", "admin@localhost.com", false, "System", "Admin", false, null, null, "ADMIN@LOCALHOST.COM", "ADMIN", "AQAAAAEAACcQAAAAEH09C2/lRxNhBw8hKrP8A8o3yzRUXY55+ZFZb3FTayvzbuOP4naWm9lA1gQW47MxUQ==", null, false, "70a3c9ec-46eb-4ad2-9832-d98864db8a12", false, "Learn-031", "admin" },
                    { "9e224968-33e4-4652-b7b7-8574d048cdb9", 0, "e45z.4.ucc.md/sap", 111, "6ddf598f-57fa-4288-a9f7-9fd0f2f01334", "ApplicationUser", "user@localhost.com", false, "System", "User", false, null, null, "USER@LOCALHOST.COM", "USER", "AQAAAAEAACcQAAAAECGZC0tB741pd5ISdFhSN2I4SwZfRPZzzaT9pEs8bT0cINboVUTBDAq+7aelAGuqtQ==", null, false, "7500f7f6-3504-40f1-b220-133f886b4315", false, "Learn-031", "user" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "cbc43a8e-f7bb-4445-baaf-1add431ffbbf", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { "cac43a6e-f7bb-4448-baaf-1add431ccbbf", "9e224968-33e4-4652-b7b7-8574d048cdb9" }
                });
        }
    }
}
