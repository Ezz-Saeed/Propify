using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class OwnerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2af3960d-69d1-4917-9fd2-577e4f581811", "857356e5-501d-410c-b992-96076d5e39cb", "Owner", "OWNER" });

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Villa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2af3960d-69d1-4917-9fd2-577e4f581811");

            migrationBuilder.UpdateData(
                table: "PropertyTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "{Villa}");
        }
    }
}
