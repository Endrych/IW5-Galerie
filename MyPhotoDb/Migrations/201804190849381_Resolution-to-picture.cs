namespace MyPhotoDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Resolutiontopicture : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PictureEntities", "Resolution_Id", "dbo.ResolutionEntities");
            DropIndex("dbo.PictureEntities", new[] { "Resolution_Id" });
            AddColumn("dbo.PictureEntities", "ResolutionHeight", c => c.Int(nullable: false));
            AddColumn("dbo.PictureEntities", "ResolutionWidth", c => c.Int(nullable: false));
            DropColumn("dbo.PictureEntities", "Resolution_Id");
            DropTable("dbo.ResolutionEntities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ResolutionEntities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Height = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PictureEntities", "Resolution_Id", c => c.Guid());
            DropColumn("dbo.PictureEntities", "ResolutionWidth");
            DropColumn("dbo.PictureEntities", "ResolutionHeight");
            CreateIndex("dbo.PictureEntities", "Resolution_Id");
            AddForeignKey("dbo.PictureEntities", "Resolution_Id", "dbo.ResolutionEntities", "Id");
        }
    }
}
