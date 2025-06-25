namespace Fiction2Fact.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedAdminRole : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO AspNetRoles (Id, Name) values ('" + Guid.NewGuid() + "', 'Admin')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM AspNetRoles WHERE Name = 'Admin'");
        }
    }
}
