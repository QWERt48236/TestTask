using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConferenceBooking.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeBands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeBands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Start = table.Column<TimeOnly>(type: "time", nullable: false),
                    End = table.Column<TimeOnly>(type: "time", nullable: false),
                    Modifier = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeBands", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TimeBands",
                columns: new[] { "Id", "End", "Modifier", "Name", "Priority", "Start" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), new TimeOnly(18, 0, 0), 0.00m, "Standard", 4, new TimeOnly(9, 0, 0) },
                    { new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"), new TimeOnly(14, 0, 0), 0.15m, "Peak", 1, new TimeOnly(12, 0, 0) },
                    { new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), new TimeOnly(23, 0, 0), -0.20m, "Evening", 2, new TimeOnly(18, 0, 0) },
                    { new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"), new TimeOnly(9, 0, 0), -0.10m, "Morning", 3, new TimeOnly(6, 0, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeBands_Name",
                table: "TimeBands",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeBands_Priority",
                table: "TimeBands",
                column: "Priority",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeBands");
        }
    }
}
