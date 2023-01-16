using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LoadTester.Extensions;
public static class Extensions
{
    
    public static DbContextOptionsBuilder BuildContext(this DbContextOptionsBuilder options, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("TestContext");

        return options.UseNpgsql(
            connectionString,
            opts =>
            {
                opts.CommandTimeout(30);
                opts.EnableRetryOnFailure(2);
            });
    }
}
