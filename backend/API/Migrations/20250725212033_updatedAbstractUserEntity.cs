using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class updatedAbstractUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "specialization",
                table: "AbstractUser",
                newName: "Specialization");

            migrationBuilder.AddColumn<string>(
                name: "PassWordSalt",
                table: "AbstractUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "AbstractUser",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassWordSalt",
                table: "AbstractUser");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "AbstractUser");

            migrationBuilder.RenameColumn(
                name: "Specialization",
                table: "AbstractUser",
                newName: "specialization");
        }
    }
}
