namespace MyPhotoDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimetoPicture : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PictureEntities", "PhotoTakenDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PictureEntities", "PhotoTakenDate", c => c.Time(nullable: false, precision: 7));
        }
    }
}
