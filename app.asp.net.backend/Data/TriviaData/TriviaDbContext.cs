using Microsoft.EntityFrameworkCore;
using app.asp.net.backend.TriviaModels;

namespace app.asp.net.backend.TriviaData;

public class TriviaDBContext : DbContext
{
    public TriviaDBContext(DbContextOptions<TriviaDBContext> options) : base(options) { }

    public DbSet<TCategory> TCategories => Set<TCategory>();
    public DbSet<TQuestion> TQuestions => Set<TQuestion>();
    public DbSet<TScore> TScores => Set<TScore>();
}