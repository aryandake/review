namespace Fiction2Fact.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedTestUser : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO AspNetUsers(Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName) values ('" + Guid.NewGuid() + "', 'testuser@fiction2fact.com', 1, null, null, null, 1, 0, null, 0, 0, 'testuser')");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM AspNetUsers WHERE UserName = 'testuser'");
        }
    }
}
