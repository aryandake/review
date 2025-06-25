namespace Fiction2Fact.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeatureToggleSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TBL_COMPANY",
                c => new
                    {
                        CMP_ID = c.Int(nullable: false, identity: true),
                        CMP_NAME = c.String(nullable: false, maxLength: 255),
                        CMP_SHORT_NAME = c.String(nullable: false, maxLength: 255),
                        CMP_ADDRESS = c.String(maxLength: 255),
                        CMP_LANDMARK = c.String(maxLength: 255),
                        CMP_CITY = c.String(maxLength: 255),
                        CMP_STATE = c.String(maxLength: 255),
                        CMP_PINCODE = c.String(maxLength: 10),
                        CMP_CONTACT_PERSON = c.String(maxLength: 255),
                        CMP_CONTACT_NO = c.String(maxLength: 255),
                        CMP_MOBILE = c.String(maxLength: 255),
                        CMP_FAX = c.String(maxLength: 255),
                        CMP_EMAIL = c.String(maxLength: 255),
                        CMP_LOGO = c.String(maxLength: 255),
                        CMP_LOGO_MIME = c.String(maxLength: 255),
                        CMP_ACTIVE = c.String(nullable: false, maxLength: 1),
                        CMP_CURRENT = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.CMP_ID);
            
            CreateTable(
                "dbo.tbl_company_products",
                c => new
                    {
                        cp_id = c.Int(nullable: false, identity: true),
                        cp_cmp_id = c.Int(nullable: false),
                        cp_pm_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.cp_id)
                .ForeignKey("dbo.TBL_COMPANY", t => t.cp_cmp_id, cascadeDelete: true)
                .ForeignKey("dbo.tbl_product_mas", t => t.cp_pm_id, cascadeDelete: true)
                .Index(t => t.cp_cmp_id)
                .Index(t => t.cp_pm_id);
            
            CreateTable(
                "dbo.tbl_cp_features",
                c => new
                    {
                        CPF_ID = c.Int(nullable: false, identity: true),
                        CPF_FM_ID = c.Int(nullable: false),
                        CPF_CP_ID = c.Int(nullable: false),
                        CPF_PGM_ID = c.Int(nullable: false),
                        CPF_ENABLED = c.String(nullable: false, maxLength: 1),
                        STATUS = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.CPF_ID)
                .ForeignKey("dbo.tbl_company_products", t => t.CPF_CP_ID, cascadeDelete: true)
                .ForeignKey("dbo.tbl_feature_mas", t => t.CPF_FM_ID, cascadeDelete: true)
                .ForeignKey("dbo.tbl_page_mas", t => t.CPF_PGM_ID, cascadeDelete: true)
                .Index(t => t.CPF_FM_ID)
                .Index(t => t.CPF_CP_ID)
                .Index(t => t.CPF_PGM_ID);
            
            CreateTable(
                "dbo.tbl_feature_mas",
                c => new
                    {
                        FM_ID = c.Int(nullable: false, identity: true),
                        FM_NAME = c.String(nullable: false, maxLength: 255),
                        FM_DESCRIPTION = c.String(maxLength: 255),
                        STATUS = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.FM_ID);
            
            CreateTable(
                "dbo.tbl_page_mas",
                c => new
                    {
                        PGM_ID = c.Int(nullable: false, identity: true),
                        PGM_PM_ID = c.Int(nullable: false),
                        PGM_NAME = c.String(nullable: false, maxLength: 255),
                        PGM_ABSOLUTE_PATH = c.String(nullable: false, maxLength: 255),
                        PGM_LINK = c.String(nullable: false, maxLength: 255),
                        Status = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.PGM_ID)
                .ForeignKey("dbo.tbl_product_mas", t => t.PGM_PM_ID, cascadeDelete: false)
                .Index(t => t.PGM_PM_ID);
            
            CreateTable(
                "dbo.tbl_product_mas",
                c => new
                    {
                        PM_ID = c.Int(nullable: false, identity: true),
                        PM_NAME = c.String(nullable: false, maxLength: 255),
                        Status = c.String(nullable: false, maxLength: 1),
                    })
                .PrimaryKey(t => t.PM_ID);
            
            CreateTable(
                "dbo.TBL_MENU_MAS",
                c => new
                    {
                        MM_ID = c.Int(nullable: false, identity: true),
                        MM_MENU_NAME = c.String(maxLength: 255),
                        MM_MENU_LEVEL = c.Int(nullable: false),
                        MM_PARENT_ID = c.Int(),
                        MM_PM_ID = c.Int(),
                        MM_SORT_ORDER = c.Int(),
                        MM_HTML = c.String(maxLength: 255),
                        MM_JAVASCRIPT = c.String(maxLength: 4000),
                        MM_HTML_OUTER = c.String(maxLength: 4000),
                        MM_JAVASCRIPT_OUTER = c.String(maxLength: 4000),
                        MM_JS_MENU_NAME = c.String(maxLength: 4000),
                        CREATE_BY = c.String(maxLength: 50),
                        CREATE_DT = c.DateTime(nullable: false),
                        UPDATE_BY = c.String(maxLength: 50),
                        UPDATE_DT = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MM_ID)
                .ForeignKey("dbo.TBL_MENU_MAS", t => t.MM_PARENT_ID)
                .ForeignKey("dbo.tbl_product_mas", t => t.MM_PM_ID)
                .Index(t => t.MM_PARENT_ID)
                .Index(t => t.MM_PM_ID);
            
            CreateTable(
                "dbo.tbl_product_menu_mapping",
                c => new
                    {
                        PMM_ID = c.Int(nullable: false, identity: true),
                        PMM_PM_ID = c.Int(nullable: false),
                        PMM_MM_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PMM_ID)
                .ForeignKey("dbo.TBL_MENU_MAS", t => t.PMM_MM_ID, cascadeDelete: true)
                .ForeignKey("dbo.tbl_product_mas", t => t.PMM_PM_ID, cascadeDelete: true)
                .Index(t => t.PMM_PM_ID)
                .Index(t => t.PMM_MM_ID);
            
            CreateTable(
                "dbo.TBL_F2F_LOG",
                c => new
                    {
                        FL_ID = c.Long(nullable: false, identity: true),
                        FL_TIMESTAMP = c.DateTime(nullable: false),
                        FL_FILENAME = c.String(maxLength: 255),
                        FL_FUNCTION = c.String(maxLength: 255),
                        FL_MESSAGE = c.String(),
                        FL_INNER_MESSAGE = c.String(),
                        FL_STACK_TRACE = c.String(),
                        FL_ADDITIONAL_INFO = c.String(),
                    })
                .PrimaryKey(t => t.FL_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbl_page_mas", "PGM_PM_ID", "dbo.tbl_product_mas");
            DropForeignKey("dbo.tbl_product_menu_mapping", "PMM_PM_ID", "dbo.tbl_product_mas");
            DropForeignKey("dbo.tbl_product_menu_mapping", "PMM_MM_ID", "dbo.TBL_MENU_MAS");
            DropForeignKey("dbo.TBL_MENU_MAS", "MM_PM_ID", "dbo.tbl_product_mas");
            DropForeignKey("dbo.TBL_MENU_MAS", "MM_PARENT_ID", "dbo.TBL_MENU_MAS");
            DropForeignKey("dbo.tbl_company_products", "cp_pm_id", "dbo.tbl_product_mas");
            DropForeignKey("dbo.tbl_cp_features", "CPF_PGM_ID", "dbo.tbl_page_mas");
            DropForeignKey("dbo.tbl_cp_features", "CPF_FM_ID", "dbo.tbl_feature_mas");
            DropForeignKey("dbo.tbl_cp_features", "CPF_CP_ID", "dbo.tbl_company_products");
            DropForeignKey("dbo.tbl_company_products", "cp_cmp_id", "dbo.TBL_COMPANY");
            DropIndex("dbo.tbl_product_menu_mapping", new[] { "PMM_MM_ID" });
            DropIndex("dbo.tbl_product_menu_mapping", new[] { "PMM_PM_ID" });
            DropIndex("dbo.TBL_MENU_MAS", new[] { "MM_PM_ID" });
            DropIndex("dbo.TBL_MENU_MAS", new[] { "MM_PARENT_ID" });
            DropIndex("dbo.tbl_page_mas", new[] { "PGM_PM_ID" });
            DropIndex("dbo.tbl_cp_features", new[] { "CPF_PGM_ID" });
            DropIndex("dbo.tbl_cp_features", new[] { "CPF_CP_ID" });
            DropIndex("dbo.tbl_cp_features", new[] { "CPF_FM_ID" });
            DropIndex("dbo.tbl_company_products", new[] { "cp_pm_id" });
            DropIndex("dbo.tbl_company_products", new[] { "cp_cmp_id" });
            DropTable("dbo.TBL_F2F_LOG");
            DropTable("dbo.tbl_product_menu_mapping");
            DropTable("dbo.TBL_MENU_MAS");
            DropTable("dbo.tbl_product_mas");
            DropTable("dbo.tbl_page_mas");
            DropTable("dbo.tbl_feature_mas");
            DropTable("dbo.tbl_cp_features");
            DropTable("dbo.tbl_company_products");
            DropTable("dbo.TBL_COMPANY");
        }
    }
}
