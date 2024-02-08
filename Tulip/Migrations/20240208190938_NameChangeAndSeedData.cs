using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tulip.Migrations
{
    /// <inheritdoc />
    public partial class NameChangeAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ApplicationServer", "ClientId", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "trek.ucc.uwm.edu", 101, "d9b15b09-bb21-493c-b67a-0af79ee4d759", "AQAAAAIAAYagAAAAEOTAxua1e2Q70FRGMqRXBvRiWFA6tZDfWFk8VMjeaepuPeIgmZMbRWtQke8C14jPzg==", "0bade5e0-2d57-4e81-9c82-c6602099a99c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ApplicationServer", "ClientId", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "trek.ucc.uwm.edu", 101, "fa08800c-1cdb-4c2e-aaeb-ffe94072c4eb", "AQAAAAIAAYagAAAAEHKbXT0s/f0QLYjPbSWl3A1ULJGkdsC02PP3aGgO4jdss0samA07/IsPBYC7SvYVtw==", "ca2d833b-7e3f-4a49-bb26-cce69538d086" });
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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ApplicationServer", "ClientId", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e45z.4.ucc.md/sap", 111, "776271d5-eab9-4283-a5c3-5e516fce02fb", "AQAAAAEAACcQAAAAEH09C2/lRxNhBw8hKrP8A8o3yzRUXY55+ZFZb3FTayvzbuOP4naWm9lA1gQW47MxUQ==", "70a3c9ec-46eb-4ad2-9832-d98864db8a12" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9e224968-33e4-4652-b7b7-8574d048cdb9",
                columns: new[] { "ApplicationServer", "ClientId", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e45z.4.ucc.md/sap", 111, "6ddf598f-57fa-4288-a9f7-9fd0f2f01334", "AQAAAAEAACcQAAAAECGZC0tB741pd5ISdFhSN2I4SwZfRPZzzaT9pEs8bT0cINboVUTBDAq+7aelAGuqtQ==", "7500f7f6-3504-40f1-b220-133f886b4315" });
        }
    }
}
