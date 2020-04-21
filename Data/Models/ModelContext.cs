using Microsoft.EntityFrameworkCore;

namespace Data.Models
{
    public class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookTable> Books { get; set; }
        public virtual DbSet<AuthorTable> Authors { get; set; }
        public virtual DbQuery<BookNextValQuery> BookNextVals { get; set; }
        public virtual DbQuery<AuthorNextValQuery> AuthorNextVals { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
            //    .HasAnnotation("Relational:DefaultSchema", "HR");

            modelBuilder.Entity<BookTable>(entity =>
            {
                entity.HasKey(e => e.Bookid)
                    .HasName("BOOKS_CONST1_PK");

                entity.ToTable("BOOKS");

                entity.HasIndex(e => e.BookTitle)
                    .HasName("BOOKS_CONST2_UK")
                    .IsUnique();

                entity.HasIndex(e => e.Bookid)
                    .HasName("BOOKS_CONST1_PK")
                    .IsUnique();

                entity.Property(e => e.Bookid)
                    .HasColumnName("BOOKID")
                    .HasColumnType("NUMBER(10)");

                entity.Property(e => e.BookTitle)
                    .IsRequired()
                    .HasColumnName("BOOK_TITLE")
                    .HasColumnType("VARCHAR2(100)");

                entity.Property(e => e.Pages).HasColumnName("PAGES");

                entity.Property(e => e.PublishDate)
                    .HasColumnName("PUBLISH_DATE")
                    .HasColumnType("DATE");
            });

            modelBuilder.Entity<AuthorTable>(entity =>
             {

                 entity.HasKey(e => e.AuthorId);

                 entity.Property(e => e.AuthorId)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(10)");

                 entity.HasOne(e => e.Book)
                 .WithMany(e => e.AuthorTables)
                 .HasForeignKey(f => f.BookTableId);

                 entity.Property(e => e.AuthorName).IsRequired();

             }
            );


            base.OnModelCreating(modelBuilder);
        }
    }
}
