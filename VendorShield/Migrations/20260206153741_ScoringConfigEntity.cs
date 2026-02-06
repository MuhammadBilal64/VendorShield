using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorShield.Migrations
{
    /// <inheritdoc />
    public partial class ScoringConfigEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoringConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OnTimeWeight = table.Column<double>(type: "float", nullable: false),
                    QualityWeight = table.Column<double>(type: "float", nullable: false),
                    IncidentWeight = table.Column<double>(type: "float", nullable: false),
                    ThresholdLowRisk = table.Column<int>(type: "int", nullable: false),
                    ThresholdMediumRisk = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringConfigs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoringConfigs");
        }
    }
}
