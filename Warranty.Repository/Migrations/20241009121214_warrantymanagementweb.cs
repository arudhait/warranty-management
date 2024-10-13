using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warranty.Repository.Migrations
{
    /// <inheritdoc />
    public partial class warrantymanagementweb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LoginFailure",
                columns: table => new
                {
                    LoginFailureId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    IP = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginFailure", x => x.LoginFailureId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LoginHistory",
                columns: table => new
                {
                    LoginHistoryId = table.Column<int>(type: "int", nullable: false),
                    UserMasterId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LoggedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    LoggedOut = table.Column<DateTime>(type: "datetime", nullable: true),
                    LoggedOutBy = table.Column<int>(type: "int", nullable: true),
                    IP = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginHistory", x => x.LoginHistoryId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    MenuId = table.Column<short>(type: "smallint", nullable: false),
                    MenuName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuNameId = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuUrl = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DisplayOrder = table.Column<short>(type: "smallint", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    HaveChild = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.MenuId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USER_TYPE_MAST",
                columns: table => new
                {
                    USER_TYPE_ID = table.Column<short>(type: "smallint", nullable: false),
                    USER_TYPE_NAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_TYPE_MAST", x => x.USER_TYPE_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MenuRole",
                columns: table => new
                {
                    MenuRoleId = table.Column<short>(type: "smallint", nullable: false),
                    MenuId = table.Column<short>(type: "smallint", nullable: false),
                    USER_TYPE_ID = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRole", x => x.MenuRoleId);
                    table.ForeignKey(
                        name: "FK_MenuRole_Menu",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "MenuId");
                    table.ForeignKey(
                        name: "FK_MenuRole_USER_TYPE_MAST",
                        column: x => x.USER_TYPE_ID,
                        principalTable: "USER_TYPE_MAST",
                        principalColumn: "USER_TYPE_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ACTION_MAST",
                columns: table => new
                {
                    ACTION_ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ACTION_NAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "((1))"),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACTION_MAST", x => x.ACTION_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BREAKDOWN_DET",
                columns: table => new
                {
                    BREAKDOWN_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CUST_ID = table.Column<long>(type: "bigint", nullable: false),
                    CALL_REG_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    TYPE_ID = table.Column<short>(type: "smallint", nullable: false),
                    ENGG_ID = table.Column<int>(type: "int", nullable: false),
                    ENGG_FIRST_VISIT_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    CRM_NO = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PROBLEMS = table.Column<string>(type: "longtext", unicode: false, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    REQ_ACTION = table.Column<short>(type: "smallint", nullable: false),
                    ACTION_TAKEN = table.Column<short>(type: "smallint", nullable: false),
                    CONCLUSION = table.Column<short>(type: "smallint", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "((1))"),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BREAKDOWN_DET", x => x.BREAKDOWN_ID);
                    table.ForeignKey(
                        name: "FK_BREAKDOWN_DET_ACTION_MAST",
                        column: x => x.REQ_ACTION,
                        principalTable: "ACTION_MAST",
                        principalColumn: "ACTION_ID");
                    table.ForeignKey(
                        name: "FK_BREAKDOWN_DET_ACTION_MAST1",
                        column: x => x.ACTION_TAKEN,
                        principalTable: "ACTION_MAST",
                        principalColumn: "ACTION_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BREAKDOWN_STATUS_MAST",
                columns: table => new
                {
                    BREAKDOWN_STATUS_ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BREAKDOWN_STATUS_NAME = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "((1))"),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BREAKDOWN_STATUS_MAST", x => x.BREAKDOWN_STATUS_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CONTRACT_DET",
                columns: table => new
                {
                    CONTRACT_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CONTRACT_TYPE_ID = table.Column<short>(type: "smallint", nullable: false),
                    WARRANTY_ID = table.Column<long>(type: "bigint", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CHEQUE_DET = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    INVOICE_NO = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    START_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    AMT_EXCLUD_TAX = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NO_OF_SERVICE = table.Column<short>(type: "smallint", nullable: true),
                    INTERVAL = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TAKEN_BY = table.Column<int>(type: "int", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CREATED_BY = table.Column<int>(type: "int", nullable: true),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRACT_DET", x => x.CONTRACT_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CONTRACT_TYPE_MAST",
                columns: table => new
                {
                    CONTRACT_TYPE_ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CONTRACT_TYPE_NAME = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRACT_TYPE_MAST", x => x.CONTRACT_TYPE_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CUST_MAST",
                columns: table => new
                {
                    CUST_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DOCTOR_NAME = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HOSPITAL_NAME = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    POSTAL_ADDRESS = table.Column<string>(type: "varchar(800)", unicode: false, maxLength: 800, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DESIGNATION = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MOBILE_NO = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PHONE_NO = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EMAIL = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PINCODE = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STATE_ID = table.Column<short>(type: "smallint", nullable: false),
                    DISTRICT_ID = table.Column<int>(type: "int", nullable: false),
                    CITY = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PNDT_CERTI_NO = table.Column<string>(type: "varchar(27)", unicode: false, maxLength: 27, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    REG_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "((1))"),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CUST_MAST", x => x.CUST_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Declaration",
                columns: table => new
                {
                    DeclarationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CRMNo = table.Column<int>(type: "int", nullable: false),
                    SparePartNo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PartSN = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MachineSN = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SignedBy = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Declaration", x => x.DeclarationId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DISTRICT_MAST",
                columns: table => new
                {
                    DISTRICT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DISTRICT_NAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    STATE_ID = table.Column<short>(type: "smallint", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "((1))"),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DISTRICT_MAST", x => x.DISTRICT_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ENGG_MAST",
                columns: table => new
                {
                    ENGG_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ENGG_NAME = table.Column<string>(type: "varchar(80)", unicode: false, maxLength: 80, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ENGG_MAST", x => x.ENGG_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USER_MAST",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ENGG_ID = table.Column<int>(type: "int", nullable: true),
                    USER_NAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    USER_PASSWORD = table.Column<string>(type: "longtext", unicode: false, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    USER_TYPE_ID = table.Column<short>(type: "smallint", nullable: false),
                    EMAIL = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_MAST", x => x.USER_ID);
                    table.ForeignKey(
                        name: "FK_USER_MAST_ENGG_MAST",
                        column: x => x.ENGG_ID,
                        principalTable: "ENGG_MAST",
                        principalColumn: "ENGG_ID");
                    table.ForeignKey(
                        name: "FK_USER_MAST_USER_TYPE_MAST",
                        column: x => x.USER_TYPE_ID,
                        principalTable: "USER_TYPE_MAST",
                        principalColumn: "USER_TYPE_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MODEL_MAST",
                columns: table => new
                {
                    MODEL_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MODEL_NO = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MODEL_MAST", x => x.MODEL_ID);
                    table.ForeignKey(
                        name: "FK_MODEL_MAST_USER_MAST",
                        column: x => x.CREATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                    table.ForeignKey(
                        name: "FK_MODEL_MAST_USER_MAST1",
                        column: x => x.UPDATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductMaster",
                columns: table => new
                {
                    ProductMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Qty = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Sku = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BatchNo = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Warranty = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CRAETED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMaster", x => x.ProductMasterId);
                    table.ForeignKey(
                        name: "FK_ProductMaster_ProductMaster",
                        column: x => x.CREATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                    table.ForeignKey(
                        name: "FK_ProductMaster_USER_MAST",
                        column: x => x.UPDATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "STATE_MAST",
                columns: table => new
                {
                    STATE_ID = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    STATE_NAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_STATE_MAST", x => x.STATE_ID);
                    table.ForeignKey(
                        name: "FK_STATE_MAST_USER_MAST",
                        column: x => x.CREATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                    table.ForeignKey(
                        name: "FK_STATE_MAST_USER_MAST1",
                        column: x => x.UPDATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WARRANTY_DET",
                columns: table => new
                {
                    WARRANTY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CUST_ID = table.Column<long>(type: "bigint", nullable: false),
                    SELLING_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    START_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    END_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    INSTALLED_BY = table.Column<int>(type: "int", nullable: true),
                    CRM_NO = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NO_OF_SERVICES = table.Column<short>(type: "smallint", nullable: true),
                    INTERVAL = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WARRANTY_DET", x => x.WARRANTY_ID);
                    table.ForeignKey(
                        name: "FK_WARRANTY_DET_CUST_MAST",
                        column: x => x.CUST_ID,
                        principalTable: "CUST_MAST",
                        principalColumn: "CUST_ID");
                    table.ForeignKey(
                        name: "FK_WARRANTY_DET_ENGG_MAST",
                        column: x => x.INSTALLED_BY,
                        principalTable: "ENGG_MAST",
                        principalColumn: "ENGG_ID");
                    table.ForeignKey(
                        name: "FK_WARRANTY_DET_USER_MAST",
                        column: x => x.CREATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                    table.ForeignKey(
                        name: "FK_WARRANTY_DET_USER_MAST1",
                        column: x => x.UPDATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Warranty_List",
                columns: table => new
                {
                    WARRANTY_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CUST_ID = table.Column<long>(type: "bigint", nullable: false),
                    SELLING_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    START_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    END_DATE = table.Column<DateTime>(type: "date", nullable: true),
                    INSTALLED_BY = table.Column<int>(type: "int", nullable: true),
                    CRM_NO = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NO_OF_SERVICES = table.Column<short>(type: "smallint", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    INTERVAL = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    CRAETED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warranty", x => x.WARRANTY_ID);
                    table.ForeignKey(
                        name: "FK_Warranty_List_CUST_MAST",
                        column: x => x.CUST_ID,
                        principalTable: "CUST_MAST",
                        principalColumn: "CUST_ID");
                    table.ForeignKey(
                        name: "FK_Warranty_List_ENGG_MAST",
                        column: x => x.INSTALLED_BY,
                        principalTable: "ENGG_MAST",
                        principalColumn: "ENGG_ID");
                    table.ForeignKey(
                        name: "FK_Warranty_USER_MAST",
                        column: x => x.CREATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                    table.ForeignKey(
                        name: "FK_Warranty_USER_MAST1",
                        column: x => x.UPDATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SupplierMaster",
                columns: table => new
                {
                    SupplierMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SupplierName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContactNo = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StateId = table.Column<short>(type: "smallint", nullable: false),
                    ZipCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductMatserId = table.Column<int>(type: "int", nullable: false),
                    SupplierSku = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductPrice = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierMaster", x => x.SupplierMasterId);
                    table.ForeignKey(
                        name: "FK_SupplierMaster_ProductMaster",
                        column: x => x.ProductMatserId,
                        principalTable: "ProductMaster",
                        principalColumn: "ProductMasterId");
                    table.ForeignKey(
                        name: "FK_SupplierMaster_STATE_MAST",
                        column: x => x.StateId,
                        principalTable: "STATE_MAST",
                        principalColumn: "STATE_ID");
                    table.ForeignKey(
                        name: "FK_SupplierMaster_USER_MAST",
                        column: x => x.CreatedBy,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                    table.ForeignKey(
                        name: "FK_SupplierMaster_USER_MAST1",
                        column: x => x.UpdatedBy,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TerritoryAllocation",
                columns: table => new
                {
                    AlloctionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ENGG_ID = table.Column<int>(type: "int", nullable: false),
                    DISTRICT_ID = table.Column<int>(type: "int", nullable: false),
                    STATE_ID = table.Column<short>(type: "smallint", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime", nullable: false),
                    CREATED_BY = table.Column<int>(type: "int", nullable: false),
                    UPDATED_BY = table.Column<int>(type: "int", nullable: true),
                    UPDATED_DATE = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerritoryAllocation", x => x.AlloctionId);
                    table.ForeignKey(
                        name: "FK_TerritoryAllocation_DISTRICT_MAST",
                        column: x => x.DISTRICT_ID,
                        principalTable: "DISTRICT_MAST",
                        principalColumn: "DISTRICT_ID");
                    table.ForeignKey(
                        name: "FK_TerritoryAllocation_ENGG_MAST",
                        column: x => x.ENGG_ID,
                        principalTable: "ENGG_MAST",
                        principalColumn: "ENGG_ID");
                    table.ForeignKey(
                        name: "FK_TerritoryAllocation_STATE_MAST",
                        column: x => x.STATE_ID,
                        principalTable: "STATE_MAST",
                        principalColumn: "STATE_ID");
                    table.ForeignKey(
                        name: "FK_TerritoryAllocation_USER_MAST",
                        column: x => x.CREATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                    table.ForeignKey(
                        name: "FK_TerritoryAllocation_USER_MAST1",
                        column: x => x.UPDATED_BY,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MODEL_DET",
                columns: table => new
                {
                    MODEL_DET_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WARRANTY_ID = table.Column<long>(type: "bigint", nullable: false),
                    MODEL_ID = table.Column<int>(type: "int", nullable: false),
                    MODEL_SERIAL_NO = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MODEL_DET", x => x.MODEL_DET_ID);
                    table.ForeignKey(
                        name: "FK_MODEL_DET_MODEL_MAST",
                        column: x => x.MODEL_ID,
                        principalTable: "MODEL_MAST",
                        principalColumn: "MODEL_ID");
                    table.ForeignKey(
                        name: "FK_MODEL_DET_WARRANTY_DET",
                        column: x => x.WARRANTY_ID,
                        principalTable: "WARRANTY_DET",
                        principalColumn: "WARRANTY_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PROB_DET",
                columns: table => new
                {
                    PROB_ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WARRANTY_ID = table.Column<long>(type: "bigint", nullable: false),
                    PROB_NAME = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PROB_SERIAL_NO = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PROB_DET", x => x.PROB_ID);
                    table.ForeignKey(
                        name: "FK_PROB_DET_WARRANTY_DET",
                        column: x => x.WARRANTY_ID,
                        principalTable: "WARRANTY_DET",
                        principalColumn: "WARRANTY_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InwardOutward",
                columns: table => new
                {
                    InwardOutwardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CUST_ID = table.Column<long>(type: "bigint", nullable: true),
                    SupplierMasterId = table.Column<int>(type: "int", nullable: true),
                    IsType = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    Note = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IP = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InwardOutward", x => x.InwardOutwardId);
                    table.ForeignKey(
                        name: "FK_InwardOutward_CUST_MAST",
                        column: x => x.CUST_ID,
                        principalTable: "CUST_MAST",
                        principalColumn: "CUST_ID");
                    table.ForeignKey(
                        name: "FK_InwardOutward_SupplierMaster",
                        column: x => x.SupplierMasterId,
                        principalTable: "SupplierMaster",
                        principalColumn: "SupplierMasterId");
                    table.ForeignKey(
                        name: "FK_InwardOutward_USER_MAST",
                        column: x => x.CreatedBy,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                    table.ForeignKey(
                        name: "FK_InwardOutward_USER_MAST1",
                        column: x => x.UpdatedBy,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InwardOutwardItem",
                columns: table => new
                {
                    InwardOutwardItemId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InwardOutwardId = table.Column<int>(type: "int", nullable: false),
                    ProductMasterId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<short>(type: "smallint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InwardOutwardItem", x => x.InwardOutwardItemId);
                    table.ForeignKey(
                        name: "FK_InwardOutwardItem_InwardOutward",
                        column: x => x.InwardOutwardId,
                        principalTable: "InwardOutward",
                        principalColumn: "InwardOutwardId");
                    table.ForeignKey(
                        name: "FK_InwardOutwardItem_ProductMaster",
                        column: x => x.ProductMasterId,
                        principalTable: "ProductMaster",
                        principalColumn: "ProductMasterId");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ledger",
                columns: table => new
                {
                    LedgerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductMasterId = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<short>(type: "smallint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Type = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsCredit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Remarks = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InwardOutwardItemId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    IP = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledger", x => x.LedgerId);
                    table.ForeignKey(
                        name: "FK_Ledger_InwardOutwardItem",
                        column: x => x.InwardOutwardItemId,
                        principalTable: "InwardOutwardItem",
                        principalColumn: "InwardOutwardItemId");
                    table.ForeignKey(
                        name: "FK_Ledger_ProductMaster",
                        column: x => x.ProductMasterId,
                        principalTable: "ProductMaster",
                        principalColumn: "ProductMasterId");
                    table.ForeignKey(
                        name: "FK_Ledger_USER_MAST",
                        column: x => x.CreatedBy,
                        principalTable: "USER_MAST",
                        principalColumn: "USER_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ACTION_MAST_CREATED_BY",
                table: "ACTION_MAST",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_ACTION_MAST_UPDATED_BY",
                table: "ACTION_MAST",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "UC_ACTION_MAST",
                table: "ACTION_MAST",
                column: "ACTION_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BREAKDOWN_DET_ACTION_TAKEN",
                table: "BREAKDOWN_DET",
                column: "ACTION_TAKEN");

            migrationBuilder.CreateIndex(
                name: "IX_BREAKDOWN_DET_CREATED_BY",
                table: "BREAKDOWN_DET",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_BREAKDOWN_DET_CUST_ID",
                table: "BREAKDOWN_DET",
                column: "CUST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BREAKDOWN_DET_ENGG_ID",
                table: "BREAKDOWN_DET",
                column: "ENGG_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BREAKDOWN_DET_REQ_ACTION",
                table: "BREAKDOWN_DET",
                column: "REQ_ACTION");

            migrationBuilder.CreateIndex(
                name: "IX_BREAKDOWN_DET_TYPE_ID",
                table: "BREAKDOWN_DET",
                column: "TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_BREAKDOWN_DET_UPDATED_BY",
                table: "BREAKDOWN_DET",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_BREAKDOWN_STATUS_MAST_CREATED_BY",
                table: "BREAKDOWN_STATUS_MAST",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_BREAKDOWN_STATUS_MAST_UPDATED_BY",
                table: "BREAKDOWN_STATUS_MAST",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRACT_DET_CONTRACT_TYPE_ID",
                table: "CONTRACT_DET",
                column: "CONTRACT_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRACT_DET_CREATED_BY",
                table: "CONTRACT_DET",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRACT_DET_TAKEN_BY",
                table: "CONTRACT_DET",
                column: "TAKEN_BY");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRACT_DET_WARRANTY_ID",
                table: "CONTRACT_DET",
                column: "WARRANTY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRACT_TYPE_MAST_CREATED_BY",
                table: "CONTRACT_TYPE_MAST",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRACT_TYPE_MAST_UPDATED_BY",
                table: "CONTRACT_TYPE_MAST",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "UC_CONTRACT_TYPE_MAST",
                table: "CONTRACT_TYPE_MAST",
                column: "CONTRACT_TYPE_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CUST_MAST",
                table: "CUST_MAST",
                column: "DOCTOR_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CUST_MAST_CREATED_BY",
                table: "CUST_MAST",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_CUST_MAST_DISTRICT_ID",
                table: "CUST_MAST",
                column: "DISTRICT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CUST_MAST_STATE_ID",
                table: "CUST_MAST",
                column: "STATE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_CUST_MAST_UPDATED_BY",
                table: "CUST_MAST",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_Declaration_CreatedBy",
                table: "Declaration",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Declaration_UpdatedBy",
                table: "Declaration",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DISTRICT_MAST_CREATED_BY",
                table: "DISTRICT_MAST",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_DISTRICT_MAST_STATE_ID",
                table: "DISTRICT_MAST",
                column: "STATE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_DISTRICT_MAST_UPDATED_BY",
                table: "DISTRICT_MAST",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_ENGG_MAST_CREATED_BY",
                table: "ENGG_MAST",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_ENGG_MAST_UPDATED_BY",
                table: "ENGG_MAST",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_InwardOutward_CreatedBy",
                table: "InwardOutward",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InwardOutward_CUST_ID",
                table: "InwardOutward",
                column: "CUST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_InwardOutward_SupplierMasterId",
                table: "InwardOutward",
                column: "SupplierMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_InwardOutward_UpdatedBy",
                table: "InwardOutward",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InwardOutwardItem_InwardOutwardId",
                table: "InwardOutwardItem",
                column: "InwardOutwardId");

            migrationBuilder.CreateIndex(
                name: "IX_InwardOutwardItem_ProductMasterId",
                table: "InwardOutwardItem",
                column: "ProductMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledger_CreatedBy",
                table: "Ledger",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ledger_InwardOutwardItemId",
                table: "Ledger",
                column: "InwardOutwardItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledger_ProductMasterId",
                table: "Ledger",
                column: "ProductMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRole_MenuId",
                table: "MenuRole",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRole_USER_TYPE_ID",
                table: "MenuRole",
                column: "USER_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MODEL_DET_MODEL_ID",
                table: "MODEL_DET",
                column: "MODEL_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MODEL_DET_WARRANTY_ID",
                table: "MODEL_DET",
                column: "WARRANTY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MODEL_MAST_CREATED_BY",
                table: "MODEL_MAST",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_MODEL_MAST_UPDATED_BY",
                table: "MODEL_MAST",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "UC_MODEL_MAST",
                table: "MODEL_MAST",
                column: "MODEL_NO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PROB_DET_WARRANTY_ID",
                table: "PROB_DET",
                column: "WARRANTY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaster_CREATED_BY",
                table: "ProductMaster",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMaster_UPDATED_BY",
                table: "ProductMaster",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_STATE_MAST_CREATED_BY",
                table: "STATE_MAST",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_STATE_MAST_UPDATED_BY",
                table: "STATE_MAST",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "UC_STATE_MAST",
                table: "STATE_MAST",
                column: "STATE_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierMaster_CreatedBy",
                table: "SupplierMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierMaster_ProductMatserId",
                table: "SupplierMaster",
                column: "ProductMatserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierMaster_StateId",
                table: "SupplierMaster",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierMaster_UpdatedBy",
                table: "SupplierMaster",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryAllocation_CREATED_BY",
                table: "TerritoryAllocation",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryAllocation_DISTRICT_ID",
                table: "TerritoryAllocation",
                column: "DISTRICT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryAllocation_ENGG_ID",
                table: "TerritoryAllocation",
                column: "ENGG_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryAllocation_STATE_ID",
                table: "TerritoryAllocation",
                column: "STATE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_TerritoryAllocation_UPDATED_BY",
                table: "TerritoryAllocation",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_USER_MAST_ENGG_ID",
                table: "USER_MAST",
                column: "ENGG_ID");

            migrationBuilder.CreateIndex(
                name: "IX_USER_MAST_USER_TYPE_ID",
                table: "USER_MAST",
                column: "USER_TYPE_ID");

            migrationBuilder.CreateIndex(
                name: "UC_USER_MAST",
                table: "USER_MAST",
                column: "USER_NAME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WARRANTY_DET_CREATED_BY",
                table: "WARRANTY_DET",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_WARRANTY_DET_CUST_ID",
                table: "WARRANTY_DET",
                column: "CUST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_WARRANTY_DET_INSTALLED_BY",
                table: "WARRANTY_DET",
                column: "INSTALLED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_WARRANTY_DET_UPDATED_BY",
                table: "WARRANTY_DET",
                column: "UPDATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_Warranty_List_CREATED_BY",
                table: "Warranty_List",
                column: "CREATED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_Warranty_List_CUST_ID",
                table: "Warranty_List",
                column: "CUST_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Warranty_List_INSTALLED_BY",
                table: "Warranty_List",
                column: "INSTALLED_BY");

            migrationBuilder.CreateIndex(
                name: "IX_Warranty_List_UPDATED_BY",
                table: "Warranty_List",
                column: "UPDATED_BY");

            migrationBuilder.AddForeignKey(
                name: "FK_ACTION_MAST_USER_MAST",
                table: "ACTION_MAST",
                column: "CREATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ACTION_MAST_USER_MAST1",
                table: "ACTION_MAST",
                column: "UPDATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BREAKDOWN_DET_BREAK_DOWN_MAST_MAST",
                table: "BREAKDOWN_DET",
                column: "TYPE_ID",
                principalTable: "BREAKDOWN_STATUS_MAST",
                principalColumn: "BREAKDOWN_STATUS_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BREAKDOWN_DET_CUST_MAST",
                table: "BREAKDOWN_DET",
                column: "CUST_ID",
                principalTable: "CUST_MAST",
                principalColumn: "CUST_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BREAKDOWN_DET_ENGG_MAST",
                table: "BREAKDOWN_DET",
                column: "ENGG_ID",
                principalTable: "ENGG_MAST",
                principalColumn: "ENGG_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BREAKDOWN_DET_USER_MAST",
                table: "BREAKDOWN_DET",
                column: "CREATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BREAKDOWN_DET_USER_MAST1",
                table: "BREAKDOWN_DET",
                column: "UPDATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BREAKDOWN_STATUS_MAST_USER_MAST",
                table: "BREAKDOWN_STATUS_MAST",
                column: "CREATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BREAKDOWN_STATUS_MAST_USER_MAST1",
                table: "BREAKDOWN_STATUS_MAST",
                column: "UPDATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CONTRACT_DET_CONTRACT_TYPE_MAST",
                table: "CONTRACT_DET",
                column: "CONTRACT_TYPE_ID",
                principalTable: "CONTRACT_TYPE_MAST",
                principalColumn: "CONTRACT_TYPE_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CONTRACT_DET_ENGG_MAST",
                table: "CONTRACT_DET",
                column: "TAKEN_BY",
                principalTable: "ENGG_MAST",
                principalColumn: "ENGG_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CONTRACT_DET_USER_MAST",
                table: "CONTRACT_DET",
                column: "CREATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CONTRACT_DET_WARRANTY_DET",
                table: "CONTRACT_DET",
                column: "WARRANTY_ID",
                principalTable: "WARRANTY_DET",
                principalColumn: "WARRANTY_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CONTRACT_TYPE_MAST_USER_MAST",
                table: "CONTRACT_TYPE_MAST",
                column: "CREATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CONTRACT_TYPE_MAST_USER_MAST1",
                table: "CONTRACT_TYPE_MAST",
                column: "UPDATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CUST_MAST_DISTRICT_MAST",
                table: "CUST_MAST",
                column: "DISTRICT_ID",
                principalTable: "DISTRICT_MAST",
                principalColumn: "DISTRICT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CUST_MAST_STATE_MAST",
                table: "CUST_MAST",
                column: "STATE_ID",
                principalTable: "STATE_MAST",
                principalColumn: "STATE_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CUST_MAST_USER_MAST",
                table: "CUST_MAST",
                column: "CREATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CUST_MAST_USER_MAST1",
                table: "CUST_MAST",
                column: "UPDATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Declaration_USER_MAST",
                table: "Declaration",
                column: "CreatedBy",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Declaration_USER_MAST1",
                table: "Declaration",
                column: "UpdatedBy",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DISTRICT_MAST_STATE_MAST",
                table: "DISTRICT_MAST",
                column: "STATE_ID",
                principalTable: "STATE_MAST",
                principalColumn: "STATE_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DISTRICT_MAST_USER_MAST",
                table: "DISTRICT_MAST",
                column: "CREATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_DISTRICT_MAST_USER_MAST1",
                table: "DISTRICT_MAST",
                column: "UPDATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ENGG_MAST_USER_MAST",
                table: "ENGG_MAST",
                column: "CREATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ENGG_MAST_USER_MAST1",
                table: "ENGG_MAST",
                column: "UPDATED_BY",
                principalTable: "USER_MAST",
                principalColumn: "USER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ENGG_MAST_USER_MAST",
                table: "ENGG_MAST");

            migrationBuilder.DropForeignKey(
                name: "FK_ENGG_MAST_USER_MAST1",
                table: "ENGG_MAST");

            migrationBuilder.DropTable(
                name: "BREAKDOWN_DET");

            migrationBuilder.DropTable(
                name: "CONTRACT_DET");

            migrationBuilder.DropTable(
                name: "Declaration");

            migrationBuilder.DropTable(
                name: "Ledger");

            migrationBuilder.DropTable(
                name: "LoginFailure");

            migrationBuilder.DropTable(
                name: "LoginHistory");

            migrationBuilder.DropTable(
                name: "MenuRole");

            migrationBuilder.DropTable(
                name: "MODEL_DET");

            migrationBuilder.DropTable(
                name: "PROB_DET");

            migrationBuilder.DropTable(
                name: "TerritoryAllocation");

            migrationBuilder.DropTable(
                name: "Warranty_List");

            migrationBuilder.DropTable(
                name: "ACTION_MAST");

            migrationBuilder.DropTable(
                name: "BREAKDOWN_STATUS_MAST");

            migrationBuilder.DropTable(
                name: "CONTRACT_TYPE_MAST");

            migrationBuilder.DropTable(
                name: "InwardOutwardItem");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "MODEL_MAST");

            migrationBuilder.DropTable(
                name: "WARRANTY_DET");

            migrationBuilder.DropTable(
                name: "InwardOutward");

            migrationBuilder.DropTable(
                name: "CUST_MAST");

            migrationBuilder.DropTable(
                name: "SupplierMaster");

            migrationBuilder.DropTable(
                name: "DISTRICT_MAST");

            migrationBuilder.DropTable(
                name: "ProductMaster");

            migrationBuilder.DropTable(
                name: "STATE_MAST");

            migrationBuilder.DropTable(
                name: "USER_MAST");

            migrationBuilder.DropTable(
                name: "ENGG_MAST");

            migrationBuilder.DropTable(
                name: "USER_TYPE_MAST");
        }
    }
}
