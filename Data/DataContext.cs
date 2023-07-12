using Microsoft.EntityFrameworkCore;

namespace EasySearchApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DocumentWord>()
              .HasOne(b => b.Document)
              .WithMany(c => c.Words)
              .HasForeignKey(b => b.DocumentId);

            modelBuilder.Entity<DocumentWord>()
                .HasOne(b => b.Word)
                .WithMany(w => w.Documents)
                .HasForeignKey(w => w.WordId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<DocumentWord> DocumentWord { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
    }
}
