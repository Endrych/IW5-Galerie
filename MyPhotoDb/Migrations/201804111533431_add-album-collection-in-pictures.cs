namespace MyPhotoDb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addalbumcollectioninpictures : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PictureEntities", "AlbumEntity_Id", "dbo.AlbumEntities");
            DropIndex("dbo.PictureEntities", new[] { "AlbumEntity_Id" });
            CreateTable(
                "dbo.PictureEntityAlbumEntities",
                c => new
                    {
                        PictureEntity_Id = c.Guid(nullable: false),
                        AlbumEntity_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.PictureEntity_Id, t.AlbumEntity_Id })
                .ForeignKey("dbo.PictureEntities", t => t.PictureEntity_Id, cascadeDelete: true)
                .ForeignKey("dbo.AlbumEntities", t => t.AlbumEntity_Id, cascadeDelete: true)
                .Index(t => t.PictureEntity_Id)
                .Index(t => t.AlbumEntity_Id);
            
            DropColumn("dbo.PictureEntities", "AlbumEntity_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PictureEntities", "AlbumEntity_Id", c => c.Guid());
            DropForeignKey("dbo.PictureEntityAlbumEntities", "AlbumEntity_Id", "dbo.AlbumEntities");
            DropForeignKey("dbo.PictureEntityAlbumEntities", "PictureEntity_Id", "dbo.PictureEntities");
            DropIndex("dbo.PictureEntityAlbumEntities", new[] { "AlbumEntity_Id" });
            DropIndex("dbo.PictureEntityAlbumEntities", new[] { "PictureEntity_Id" });
            DropTable("dbo.PictureEntityAlbumEntities");
            CreateIndex("dbo.PictureEntities", "AlbumEntity_Id");
            AddForeignKey("dbo.PictureEntities", "AlbumEntity_Id", "dbo.AlbumEntities", "Id");
        }
    }
}
