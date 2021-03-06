using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace Diray.Data
{
    class SQLCommands
    {
        //
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public SQLCommands(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider ?? throw new ArgumentNullException(nameof(dbConnectionProvider));
        }

        public void InitSchema()
        {
            IDbConnection connection = _dbConnectionProvider.Connection;

            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Day (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Note (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    IdDay INTEGER NOT NULL,
                    Title TEXT NOT NULL,
                    Content TEXT NOT NULL,
                    FOREIGN KEY (IdDay) REFERENCES Day(Id)
                );"
            );
        }

    }
}
