using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PPWoven.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductionOperations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductionOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductionOrderId = table.Column<int>(type: "integer", nullable: false),
                    OperationSequence = table.Column<int>(type: "integer", nullable: false),
                    OperationName = table.Column<string>(type: "text", nullable: false),
                    MachineId = table.Column<int>(type: "integer", nullable: true),
                    EmployeeId = table.Column<int>(type: "integer", nullable: true),
                    ShiftId = table.Column<int>(type: "integer", nullable: true),
                    PlannedStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlannedEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PlannedQuantity = table.Column<decimal>(type: "numeric", nullable: false),
                    GoodQuantity = table.Column<decimal>(type: "numeric", nullable: false),
                    RejectedQuantity = table.Column<decimal>(type: "numeric", nullable: false),
                    WasteQuantity = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionOperations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionOperations");
        }
    }
}
