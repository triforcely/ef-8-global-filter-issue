using EF8GlobalFilters.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EF8GlobalFilters;

public class UnitTest1
{
    private readonly ApplicationDbContext _context;

    public UnitTest1()
    {
        _context = CreateContext();
    }
    
    // OK
    [Fact]
    public async Task GlobalQueryFilter_ShouldFilterSoftDeletedEntities()
    {
        _context.Users.Add(new User
        {
            DeletedAt = DateTimeOffset.Now
        });

        _context.Users.Add(new User
        {
            DeletedAt = null
        });
        
        await _context.SaveChangesAsync();

        var userCount = _context.Users.Count();
        Assert.Equal(1, userCount);
    }
    
    // NOT OK
    [Fact]
    public async Task GlobalQueryFilter_ShouldFilterSoftDeletedEntities_WhenSameFilterAppliedAdHoc()
    {
        _context.Users.Add(new User
        {
            DeletedAt = DateTimeOffset.Now
        });

        _context.Users.Add(new User
        {
            DeletedAt = null
        });
        
        await _context.SaveChangesAsync();

        var query = _context.Users.Where(x => x.DeletedAt == null);
        var querySql = query.ToQueryString(); // Why did it result in "WHERE 0" condition?
        var userCount = query.Count();
        Assert.Equal(1, userCount);
    }

    private ApplicationDbContext CreateContext()
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
        var connectionString = connectionStringBuilder.ToString();
        var connection = new SqliteConnection(connectionString);
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;
        
        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}