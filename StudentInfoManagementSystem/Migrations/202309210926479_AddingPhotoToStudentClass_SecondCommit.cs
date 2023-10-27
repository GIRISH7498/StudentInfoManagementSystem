namespace StudentInfoManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingPhotoToStudentClass_SecondCommit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "ProfileImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "ProfileImage");
        }
    }
}
