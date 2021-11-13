#if DEBUG
using System.Data;
/*
 * Copyright (c) 2019 aatmmr
 *
 * This software may be modified and distributed under the terms
 * of the MIT license. See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core.mysql
 * 
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Piranha.AspNetCore.Identity.MySQL
{
    /// <summary>
    /// Factory for creating a db context. Only used in dev mode
    /// when creating migrations.
    /// </summary>
    [NoCoverage]
    public class DbFactory : IDesignTimeDbContextFactory<IdentityMySqlDb>
    {
        /// <summary>
        /// Creates a new db context.
        /// </summary>
        /// <param name="args">The arguments</param>
        /// <returns>The db context</returns>
        public IdentityMySqlDb CreateDbContext(string[] args) 
        {
            var connectionString = "server=localhost;port=3306;database=piranha;uid=root;password=password";

            var builder = new DbContextOptionsBuilder<IdentityMySqlDb>();
            builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            return new IdentityMySqlDb(builder.Options);
        }
    }
}
#endif