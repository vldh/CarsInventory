namespace NhatH.MVC.CarInventory.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Brand = c.String(nullable: false, maxLength: 100),
                        Model = c.String(nullable: false, maxLength: 100),
                        Year = c.Int(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        New = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        IsActived = c.Boolean(nullable: false),
                        IsNotDeletable = c.Boolean(),
                        ModifyDate = c.DateTime(),
                        ModifyBy = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RoleFunctions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        FunctionKey = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserGuid = c.Guid(nullable: false),
                        UserId = c.Int(),
                        UserName = c.String(),
                        Token = c.String(),
                        Name = c.String(),
                        Email = c.String(maxLength: 255, unicode: false),
                        Telephone = c.String(maxLength: 50, unicode: false),
                        Mobile = c.String(maxLength: 50, unicode: false),
                        Avatar = c.String(),
                        IsActived = c.Boolean(nullable: false),
                        IsNotDeletable = c.Boolean(),
                        ModifyDate = c.DateTime(),
                        ModifyBy = c.String(),
                        CreateDate = c.DateTime(),
                        LastChangePassword = c.DateTime(),
                        DecimalSymbol = c.String(),
                        ThousandSymbol = c.String(),
                        DateFormat = c.String(),
                        RoleChangedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.UserProfile", "UserId", "dbo.User");
            DropForeignKey("dbo.RoleFunctions", "RoleId", "dbo.Role");
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserProfile", new[] { "UserId" });
            DropIndex("dbo.RoleFunctions", new[] { "RoleId" });
            DropTable("dbo.UserRole");
            DropTable("dbo.UserProfile");
            DropTable("dbo.User");
            DropTable("dbo.RoleFunctions");
            DropTable("dbo.Role");
            DropTable("dbo.Cars");
        }
    }
}
