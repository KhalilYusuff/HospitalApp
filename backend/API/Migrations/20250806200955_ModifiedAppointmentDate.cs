using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedAppointmentDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DOsage",
                table: "Perscriptions",
                newName: "Dosage");

            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                table: "Appointments",
                newName: "StartDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "Dosage",
                table: "Perscriptions",
                newName: "DOsage");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Appointments",
                newName: "AppointmentDate");
        }
    }
}
