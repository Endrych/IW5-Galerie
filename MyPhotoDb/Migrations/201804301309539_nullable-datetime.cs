namespace MyPhotoDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullabledatetime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PictureEntities", "PhotoTakenDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PictureEntities", "PhotoTakenDate", c => c.DateTime(nullable: false));
        }
    }
}
