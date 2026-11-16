using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TechDesk.Data;

#nullable disable
namespace TechDesk.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("202611010001_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Devices",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                AssetTag = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                PurchaseYear = table.Column<int>(type: "int", nullable: false),
                IsOnLoan = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Devices", x => x.Id));

        migrationBuilder.CreateIndex(name: "IX_Devices_AssetTag", table: "Devices", column: "AssetTag", unique: true);
        migrationBuilder.InsertData("Devices", new[] { "Id", "AssetTag", "Category", "IsOnLoan", "Name", "PurchaseYear" }, new object[,] {
            { 1, "ECC-0042", "Laptop", true, "Dell Latitude 5440", 2023 },
            { 2, "ECC-0007", "Laptop", false, "HP ProBook 450", 2019 },
            { 3, "ECC-0115", "Tablet", true, "iPad 9th generation", 2022 },
            { 4, "ECC-0090", "Tablet", false, "Samsung Galaxy Tab A8", 2021 },
            { 5, "ECC-0023", "Laptop", false, "Lenovo ThinkPad E14", 2017 }
        });
    }


    /// <summary>
    /// Supplies the model snapshot used by EF Core when generating SQL for
    /// this migration's data operations (including the seeded Devices rows).
    /// </summary>
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.31")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        modelBuilder.Entity("TechDesk.Models.Device", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int")
                .HasAnnotation("SqlServer:Identity", "1, 1");

            b.Property<string>("AssetTag")
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnType("nvarchar(10)");

            b.Property<string>("Category")
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            b.Property<bool>("IsOnLoan").HasColumnType("bit");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.Property<int>("PurchaseYear").HasColumnType("int");

            b.HasKey("Id");
            b.HasIndex("AssetTag").IsUnique();

            b.HasData(
                new { Id = 1, AssetTag = "ECC-0042", Category = "Laptop", IsOnLoan = true, Name = "Dell Latitude 5440", PurchaseYear = 2023 },
                new { Id = 2, AssetTag = "ECC-0007", Category = "Laptop", IsOnLoan = false, Name = "HP ProBook 450", PurchaseYear = 2019 },
                new { Id = 3, AssetTag = "ECC-0115", Category = "Tablet", IsOnLoan = true, Name = "iPad 9th generation", PurchaseYear = 2022 },
                new { Id = 4, AssetTag = "ECC-0090", Category = "Tablet", IsOnLoan = false, Name = "Samsung Galaxy Tab A8", PurchaseYear = 2021 },
                new { Id = 5, AssetTag = "ECC-0023", Category = "Laptop", IsOnLoan = false, Name = "Lenovo ThinkPad E14", PurchaseYear = 2017 });

            b.ToTable("Devices");
        });
#pragma warning restore 612, 618
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(name: "Devices");
}
