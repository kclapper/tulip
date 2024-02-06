using Microsoft.EntityFrameworkCore.Migrations;

namespace Gamification.UI.Migrations
{
    public partial class addedOtherfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationServer",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

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
                columns: new[] { "ApplicationServer", "ClientId", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "e45z.4.ucc.md/sap", 111, "2d0c5b1e-aaf9-418c-86a2-906699cd77b8", "AQAAAAEAACcQAAAAELu639vQFI9D8oR+0bJI8wO/4FAol1oNet0s12YfDxRtTtWvyrB6rtGlph0FwrPn2w==", "400e6f05-79a4-4edb-b3c8-f9719fec2121", "Learn-031" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ApplicationServer", "ClientId", "ConcurrencyStamp", "PasswordHash", "SecurityStamp", "UserId" },
                values: new object[] { "e45z.4.ucc.md/sap", 111, "6fd403ba-9348-497f-a2cd-f9cca62db983", "AQAAAAEAACcQAAAAEOuk3/OzdIb/1KwPNEF+zuSRZR4Fl+QSgFcUUcBb/i0gsAhdOFj1yX7/GiK1Ohs9tw==", "779eb3ea-f2f8-41af-ba64-3ddc7c9ceaca", "Learn-031" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationServer",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cac43a6e-f7bb-4448-baaf-1add431ccbbf",
                column: "ConcurrencyStamp",
                value: "8c7b6de9-701d-41e5-bdf8-5598fa4e0ff7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
                column: "ConcurrencyStamp",
                value: "e53c21d5-8122-4f7e-be63-a7470131edc6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d49b75ba-78b5-4d72-905f-f28974a581e2", "AQAAAAEAACcQAAAAEFOLzE95ku5ZZIZ6bkEWOzo8ImZbwIDLQivA73D3Bq5KVQFDENym4W3yZstEREQ1pQ==", "bba81f47-327f-41af-9e4a-66088dc6f621" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "305d0040-ddc2-41d3-8cf0-8673f3b27cb9", "AQAAAAEAACcQAAAAEPUKZecsacfzMDYzF34i/opOn+2sQ9JG68u2NnCk9H6NpMZP+JGp8kmPMM+Nd09T7g==", "82074c9b-def8-48c0-896c-49b3452e1794" });
        }
    }
}
