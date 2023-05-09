using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Levi9.POS.Domain.Migrations
{
    public partial class Documentmodelupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumetType",
                table: "Documents",
                newName: "DocumentType");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("c6029928-076f-44a4-a01f-37ad0e4b0e09"), "133280975643412338" });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("606ea448-f69b-413f-9471-7e4a7912491a"), "133280975643412363" });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("41c3ebae-ca20-43ea-b048-cf9489163f21"), "133280975643412379" });

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDay", "GlobalId", "LastUpdate" },
                values: new object[] { "133280975643412437", new Guid("87e4a1fc-469d-422e-9d7d-efbcdc7340e5"), "133280975643412450" });

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationDay", "GlobalId", "LastUpdate" },
                values: new object[] { "133280975643412467", new Guid("9be581d0-26ac-4d37-85ef-30c75ac2734c"), "133280975643412479" });

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationDay", "GlobalId", "LastUpdate" },
                values: new object[] { "133280975643412494", new Guid("0a60bdba-d6e5-43ae-9a03-8dd6982516d8"), "133280975643412507" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("ff8f3305-a43b-43a8-9798-ae62a772d54b"), "133280975643411949" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("c4679aca-669b-4264-b086-4384560e3550"), "133280975643412049" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("d7a3bde3-16fc-4470-b9aa-5ae81f590fb3"), "133280975643412066" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "Documents",
                newName: "DocumetType");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("2a602f8e-e6fe-4229-8914-a676e1c752c2"), "133277539861042731" });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("17742107-890a-492d-9ce3-dd4eb0b51f34"), "133277539861042758" });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("0166e73d-befa-47ab-a81b-29571c2778c6"), "133277539861042795" });

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDay", "GlobalId", "LastUpdate" },
                values: new object[] { "133277539861042842", new Guid("ce8d01a7-ea56-49df-bd07-9d6977307aa8"), "133277539861042858" });

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationDay", "GlobalId", "LastUpdate" },
                values: new object[] { "133277539861042887", new Guid("2dc086ad-581b-4eb5-91dc-097d0911c74d"), "133277539861042899" });

            migrationBuilder.UpdateData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationDay", "GlobalId", "LastUpdate" },
                values: new object[] { "133277539861042915", new Guid("c163bd84-b871-4ff2-a79b-746c405e6a25"), "133277539861042928" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("a2eb5681-3d85-4bd5-9435-1cb1038b3912"), "133277539861042364" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("5890b251-f854-44e6-b172-e190b2f13032"), "133277539861042474" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "GlobalId", "LastUpdate" },
                values: new object[] { new Guid("3846a52f-f5f9-44ab-bd24-3c6797889405"), "133277539861042492" });
        }
    }
}
