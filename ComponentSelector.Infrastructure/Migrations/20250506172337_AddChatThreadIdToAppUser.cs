using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentSelector.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChatThreadIdToAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AddColumn<int>(
            //     name: "Reviews",
            //     table: "Components",
            //     type: "int",
            //     nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatThreadId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            // migrationBuilder.CreateTable(
            //     name: "Characteristics",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         AttributeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         AttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         ComponentId = table.Column<int>(type: "int", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Characteristics", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Characteristics_Components_ComponentId",
            //             column: x => x.ComponentId,
            //             principalTable: "Components",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            // migrationBuilder.CreateIndex(
            //     name: "IX_Characteristics_ComponentId",
            //     table: "Characteristics",
            //     column: "ComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //     name: "Characteristics");

            // migrationBuilder.DropColumn(
            //     name: "Reviews",
            //     table: "Components");

            migrationBuilder.DropColumn(
                name: "ChatThreadId",
                table: "AspNetUsers");
        }
    }
}
