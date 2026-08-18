using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CORE.Migrations
{
    /// <inheritdoc />
    public partial class FixShadowKeyFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HR");

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "HR",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MotherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    BirthPlace = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsBornAbroad = table.Column<bool>(type: "bit", nullable: false),
                    NationalityCode = table.Column<int>(type: "int", nullable: true),
                    HasDoubleNationality = table.Column<bool>(type: "bit", nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SocialCategory = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                    table.CheckConstraint("CK_Gender", "Gender IN ('F', 'M')");
                    table.CheckConstraint("CK_MaritalStatus", "MaritalStatus IN ('S', 'M', 'D', 'W')");
                });

            migrationBuilder.CreateTable(
                name: "FinancialAccounts",
                schema: "HR",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccounts", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAccounts",
                schema: "HR",
                columns: table => new
                {
                    AccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    LockedUntil = table.Column<DateTime>(type: "datetime", nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    EmployeeID1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAccounts", x => x.AccountID);
                    table.ForeignKey(
                        name: "FK_EmployeeAccounts_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeAccounts_Employees_EmployeeID1",
                        column: x => x.EmployeeID1,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAddresses",
                schema: "HR",
                columns: table => new
                {
                    AddressID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    AddressType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CityCode = table.Column<int>(type: "int", nullable: true),
                    CountryCode = table.Column<int>(type: "int", nullable: true),
                    AddressText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NearestPoliceStation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAddresses", x => x.AddressID);
                    table.CheckConstraint("CK_AddressType", "AddressType IN ('C', 'P')");
                    table.ForeignKey(
                        name: "FK_EmployeeAddresses_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeBankAccounts",
                schema: "HR",
                columns: table => new
                {
                    BankAccountID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    BankCode = table.Column<int>(type: "int", nullable: true),
                    BranchCode = table.Column<int>(type: "int", nullable: true),
                    CityCode = table.Column<int>(type: "int", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IBAN = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankAddress1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankAddress2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryCode = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ValidFrom = table.Column<DateOnly>(type: "date", nullable: false),
                    ValidTo = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBankAccounts", x => x.BankAccountID);
                    table.ForeignKey(
                        name: "FK_EmployeeBankAccounts_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAccounts_Email",
                schema: "HR",
                table: "EmployeeAccounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAccounts_EmployeeID",
                schema: "HR",
                table: "EmployeeAccounts",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAccounts_EmployeeID1",
                schema: "HR",
                table: "EmployeeAccounts",
                column: "EmployeeID1",
                unique: true,
                filter: "[EmployeeID1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAccounts_Username",
                schema: "HR",
                table: "EmployeeAccounts",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAddresses_EmployeeID",
                schema: "HR",
                table: "EmployeeAddresses",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBankAccounts_EmployeeID",
                schema: "HR",
                table: "EmployeeBankAccounts",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeNumber",
                schema: "HR",
                table: "Employees",
                column: "EmployeeNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeAccounts",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "EmployeeAddresses",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "EmployeeBankAccounts",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "FinancialAccounts",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "HR");
        }
    }
}
