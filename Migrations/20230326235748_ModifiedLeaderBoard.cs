using Microsoft.EntityFrameworkCore.Migrations;

namespace Gamification.UI.Migrations
{
    public partial class ModifiedLeaderBoard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Point",
                table: "LeaderBoaders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "776271d5-eab9-4283-a5c3-5e516fce02fb", "AQAAAAEAACcQAAAAEH09C2/lRxNhBw8hKrP8A8o3yzRUXY55+ZFZb3FTayvzbuOP4naWm9lA1gQW47MxUQ==", "70a3c9ec-46eb-4ad2-9832-d98864db8a12" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6ddf598f-57fa-4288-a9f7-9fd0f2f01334", "AQAAAAEAACcQAAAAECGZC0tB741pd5ISdFhSN2I4SwZfRPZzzaT9pEs8bT0cINboVUTBDAq+7aelAGuqtQ==", "7500f7f6-3504-40f1-b220-133f886b4315" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Point",
                table: "LeaderBoaders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
