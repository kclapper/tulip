using Microsoft.EntityFrameworkCore.Migrations;

namespace Gamification.UI.Migrations
{
    public partial class AddedLeaderBoard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaderBoaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Point = table.Column<int>(type: "int", nullable: false),
                    CaseStudy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderBoaders", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
                column: "ConcurrencyStamp",
                value: "18db18c8-ccf9-4742-8d36-0b1dabafd970");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
                column: "ConcurrencyStamp",
                value: "9bada91b-b41e-49d3-ba11-f5f44daca066");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "44187eea-3cd6-4342-b8a2-9ee52567beb7", "AQAAAAEAACcQAAAAEFFaPLcnRuMtDXVXZLlOsYYSTlHBqMm4M2Tj/PCOFH32AJ7+XH5e470ukL7xXbJasg==", "0e2ac0e3-d5c8-4e9c-b55a-4c2adcff57ce" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b0783538-24ff-462c-aec2-be1286231644", "AQAAAAEAACcQAAAAEM5McagOe+v22v2/awBh5elrqUjqZ8kyhBP5HKWUTHzRNSfQH3mISIHbj2OuZGHKcA==", "ab43f5ec-ea35-4fac-923b-01bdd0edc895" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeaderBoaders");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
                column: "ConcurrencyStamp",
                value: "e71fcb7a-1885-4c3e-8296-e5eeae58e335");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
                column: "ConcurrencyStamp",
                value: "4c1e116c-b85d-4102-88a7-3d1cbbd34082");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2d0c5b1e-aaf9-418c-86a2-906699cd77b8", "AQAAAAEAACcQAAAAELu639vQFI9D8oR+0bJI8wO/4FAol1oNet0s12YfDxRtTtWvyrB6rtGlph0FwrPn2w==", "400e6f05-79a4-4edb-b3c8-f9719fec2121" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6fd403ba-9348-497f-a2cd-f9cca62db983", "AQAAAAEAACcQAAAAEOuk3/OzdIb/1KwPNEF+zuSRZR4Fl+QSgFcUUcBb/i0gsAhdOFj1yX7/GiK1Ohs9tw==", "779eb3ea-f2f8-41af-ba64-3ddc7c9ceaca" });
        }
    }
}
