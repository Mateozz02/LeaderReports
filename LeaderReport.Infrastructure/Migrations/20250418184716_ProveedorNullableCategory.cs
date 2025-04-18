using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaderReport.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProveedorNullableCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proveedores_CategoriaProveedores_CategoriaProveedorId",
                table: "Proveedores");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaProveedorId",
                table: "Proveedores",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Cod_Proveedor",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedores_CategoriaProveedores_CategoriaProveedorId",
                table: "Proveedores",
                column: "CategoriaProveedorId",
                principalTable: "CategoriaProveedores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proveedores_CategoriaProveedores_CategoriaProveedorId",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "Cod_Proveedor",
                table: "Productos");

            migrationBuilder.AlterColumn<int>(
                name: "CategoriaProveedorId",
                table: "Proveedores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedores_CategoriaProveedores_CategoriaProveedorId",
                table: "Proveedores",
                column: "CategoriaProveedorId",
                principalTable: "CategoriaProveedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
