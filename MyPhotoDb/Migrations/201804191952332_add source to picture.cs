namespace MyPhotoDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsourcetopicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PictureEntities", "Source", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PictureEntities", "Source");
        }
    }
}
