using Microsoft.EntityFrameworkCore;

namespace GradeBook.EntityData
{
    public class EntityDB : DbContext
    {
        public EntityDB(DbContextOptions<EntityDB> options) : base(options) { }
        public DbSet<Student> Students { get; set; }
      
    }
}

