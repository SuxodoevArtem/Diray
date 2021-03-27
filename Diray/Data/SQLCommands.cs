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
        private IDbConnection connection;
        public SQLCommands(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider ?? throw new ArgumentNullException(nameof(dbConnectionProvider));
        }

        public void InitSchema()
        {
            connection = _dbConnectionProvider.Connection;

            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Day (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL UNIQUE
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

        public IDataReader GetNote(int id)
        {
            return connection.ExecuteReader($@"
                SELECT IdDay, Title, Content
                FROM Day AS D
                    JOIN Note AS N
                        WHERE N.IdDay = D.Id AND D.Id = {id}");
        }

        public void GetDayOfMonth(string NumMonth)
        {
            var reader = connection.ExecuteReader($@"
                SELECT id, Date
                    FROM Day
                        WHERE strftime('%m', Date) = '{ NumMonth }' ");

            string x = " ";

            while(reader.Read()){
                x = reader[1].ToString();
            }

        }

    }
}
