using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoList.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "status_id",
                table: "task_items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.CreateTable(
                name: "task_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_statuses", x => x.id);
                    table.CheckConstraint("ck_task_statuses_color_hex", "color ~ '^#[0-9A-Fa-f]{6}$'");
                });

            migrationBuilder.InsertData(
                table: "task_statuses",
                columns: new[] { "id", "color", "is_system", "name", "normalized_name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), "#3B82F6", true, "Открыта", "ОТКРЫТА" });

            migrationBuilder.Sql(
                """
                CREATE FUNCTION protect_default_task_status()
                RETURNS trigger
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    RAISE EXCEPTION 'The default task status cannot be changed or deleted.';
                    RETURN OLD;
                END;
                $$;

                CREATE TRIGGER trg_protect_default_task_status
                BEFORE UPDATE OR DELETE ON task_statuses
                FOR EACH ROW
                WHEN (OLD.id = '00000000-0000-0000-0000-000000000001'::uuid)
                EXECUTE FUNCTION protect_default_task_status();
                """);

            migrationBuilder.CreateIndex(
                name: "ix_task_items_status_id",
                table: "task_items",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ux_task_statuses_normalized_name",
                table: "task_statuses",
                column: "normalized_name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_task_items_task_statuses_status_id",
                table: "task_items",
                column: "status_id",
                principalTable: "task_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_task_items_task_statuses_status_id",
                table: "task_items");

            migrationBuilder.Sql(
                """
                DROP TRIGGER IF EXISTS trg_protect_default_task_status
                    ON task_statuses;
                DROP FUNCTION IF EXISTS protect_default_task_status();
                """);

            migrationBuilder.DropTable(
                name: "task_statuses");

            migrationBuilder.DropIndex(
                name: "ix_task_items_status_id",
                table: "task_items");

            migrationBuilder.DropColumn(
                name: "status_id",
                table: "task_items");
        }
    }
}
