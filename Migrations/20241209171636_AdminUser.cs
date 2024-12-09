using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekt.Migrations
{
    /// <inheritdoc />
    public partial class AdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reviews");

            migrationBuilder.AlterColumn<Guid>(
                name: "AnnouncementId",
                table: "Reviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "CreatedAt", "Email", "LastName", "Location", "Name", "Password", "Phone", "PostCode", "Role" },
                values: new object[] { new Guid("3b0aab2b-d208-46a5-bf52-17356594809c"), null, new DateTime(2024, 12, 9, 17, 16, 35, 643, DateTimeKind.Utc).AddTicks(4622), "admin@example.com", "Admin", null, "Admin", "21232F297A57A5A743894A0E4A801FC3", null, null, "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3b0aab2b-d208-46a5-bf52-17356594809c"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AnnouncementId",
                table: "Reviews",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Reviews",
                type: "uuid",
                nullable: true);
        }
    }
}
