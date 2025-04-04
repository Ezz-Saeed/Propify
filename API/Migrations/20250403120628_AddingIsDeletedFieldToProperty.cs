using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddingIsDeletedFieldToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2af3960d-69d1-4917-9fd2-577e4f581811");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Property",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "33533155-31ea-41ee-b9f4-9302483e371a", "be7b42a0-ff43-4e68-b63c-7235db65c357", "Owner", "OWNER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33533155-31ea-41ee-b9f4-9302483e371a");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Property");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2af3960d-69d1-4917-9fd2-577e4f581811", "857356e5-501d-410c-b992-96076d5e39cb", "Owner", "OWNER" });
        }
    }
}
