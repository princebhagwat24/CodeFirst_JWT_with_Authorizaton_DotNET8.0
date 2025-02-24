
using CodeFirstJWT1.Models;
using Microsoft.EntityFrameworkCore;

public class CodeFirstJWTDbContext : DbContext
{
    public CodeFirstJWTDbContext(DbContextOptions<CodeFirstJWTDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}