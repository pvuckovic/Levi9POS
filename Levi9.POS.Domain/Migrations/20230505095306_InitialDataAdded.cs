using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Levi9.POS.Domain.Migrations
{
    public partial class InitialDataAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "Email", "GlobalId", "LastUpdate", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "1.maja, Derventa", "marko@gmail.com", new Guid("2a602f8e-e6fe-4229-8914-a676e1c752c2"), "133277539861042731", "Marko", "+387 65 132 527" },
                    { 2, "Koste Racina 24, Novi Sad", "aleksa@gmail.com", new Guid("17742107-890a-492d-9ce3-dd4eb0b51f34"), "133277539861042758", "Aleksa", "+387 64 862 476" },
                    { 3, "Strumicka 13, Novi Sad", "milos@gmail.com", new Guid("0166e73d-befa-47ab-a81b-29571c2778c6"), "133277539861042795", "Milos", "+387 65 912 127" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AvailableQuantity", "GlobalId", "LastUpdate", "Name", "Price", "ProductImageUrl" },
                values: new object[,]
                {
                    { 1, 30, new Guid("a2eb5681-3d85-4bd5-9435-1cb1038b3912"), "133277539861042364", "Levi 9 T-Shirt", 10f, "baseURL//nekiurl1.png" },
                    { 2, 10, new Guid("5890b251-f854-44e6-b172-e190b2f13032"), "133277539861042474", "Novis T-Shirt", 15f, "baseURL//nekiurl2.png" },
                    { 3, 20, new Guid("3846a52f-f5f9-44ab-bd24-3c6797889405"), "133277539861042492", "Vega IT T-Shirt", 20f, "baseURL//nekiurl3.png" }
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
                values: new object[] { 1, 1, "133277539861042842", "INVOICE", new Guid("ce8d01a7-ea56-49df-bd07-9d6977307aa8"), "133277539861042858" });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
                values: new object[] { 2, 2, "133277539861042887", "INVOICE", new Guid("2dc086ad-581b-4eb5-91dc-097d0911c74d"), "133277539861042899" });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
                values: new object[] { 3, 3, "133277539861042915", "INVOICE", new Guid("c163bd84-b871-4ff2-a79b-746c405e6a25"), "133277539861042928" });

            migrationBuilder.InsertData(
                table: "ProductDocuments",
                columns: new[] { "DocumentId", "ProductId", "Currency", "Price", "Quantity" },
                values: new object[] { 1, 1, "RSD", 1200f, 20 });

            migrationBuilder.InsertData(
                table: "ProductDocuments",
                columns: new[] { "DocumentId", "ProductId", "Currency", "Price", "Quantity" },
                values: new object[] { 2, 2, "EUR", 10f, 10 });

            migrationBuilder.InsertData(
                table: "ProductDocuments",
                columns: new[] { "DocumentId", "ProductId", "Currency", "Price", "Quantity" },
                values: new object[] { 3, 3, "USD", 15f, 15 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductDocuments",
                keyColumns: new[] { "DocumentId", "ProductId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ProductDocuments",
                keyColumns: new[] { "DocumentId", "ProductId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "ProductDocuments",
                keyColumns: new[] { "DocumentId", "ProductId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
