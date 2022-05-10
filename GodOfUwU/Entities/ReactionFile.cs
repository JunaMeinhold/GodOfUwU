namespace GodOfUwU.Entities;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class ReactionContext : DbContext
{
    public DbSet<Reaction> Reactions { get; set; }

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

    public ReactionContext()
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source=reactions.sqlite");
    }
}