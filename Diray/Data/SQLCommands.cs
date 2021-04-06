using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace Diray.Data
{
    class SQLCommands
    {
        //
        private readonly IDbConnectionProvider _dbConnectionProvider;
        private IDbConnection connection;
        public List<ReaderDay> ListDays;
        public List<ReaderNote> ListNotes;
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

        public List<ReaderDay> GetDayOfMonth(string NumMonth)
        {

            ListDays = new List<ReaderDay>();

            var reader = connection.ExecuteReader($@"
                SELECT id, Date
                    FROM Day
                        WHERE strftime('%m', Date) = '{NumMonth}' ");

            while(reader.Read()){
                ListDays.Add(new ReaderDay { 
                    Id = Int32.Parse(reader[0].ToString()), 
                    Date = reader[1].ToString()
                } );
            }

            return ListDays;
        }

        public List<ReaderNote> GetNoteOfDay(int DayId)
        {
            ListNotes = new List<ReaderNote>();

            var reader = connection.ExecuteReader($@"
                SELECT IdDay, Title, Content, N.Id
                    FROM Day AS D
                        JOIN Note AS N
                            WHERE  N.IdDay = D.Id AND D.Id = {DayId} ");

            while (reader.Read())
            {
                ListNotes.Add(new ReaderNote {
                    Title = reader[1].ToString(),
                    Content = reader[2].ToString(),
                    Id = Int32.Parse(reader[3].ToString()),
                });
            }

            return ListNotes;
        }

        public void AddNote(string date, string title, string content)
        {
           var IdDay = connection.ExecuteScalar($@"
                SELECT Id
	                FROM Day
		                WHERE Date = '{date}' ");

            if (IdDay == null)
            {
                AddDay(date);
                IdDay = connection.ExecuteScalar($@"SELECT MAX(id) FROM Day").ToString();
            }
              
            connection.Execute($@"
                INSERT INTO Note (idDay, Title, Content)
                    VALUES ('{IdDay}','{title}','{content}')
            ");
        }

        public void UpdateNote(int idNote, string title, string content)
        {
            connection.Execute($@"
                UPDATE Note SET Title = '{title}', Content = '{content}' WHERE Id = '{idNote}'
            ");
        }

        public void DeletNote(int idDay, int idNote,int countNotes)
        {
            if (countNotes != 0)
            {
                connection.Execute($@"
                    DELETE FROM Note WHERE Id = '{idNote}'
                ");
            }
  
            if(countNotes <= 1)
                DeleteDay(idDay);
            
        }

        private void DeleteDay(int idDay)
        {
            connection.Execute($@" DELETE FROM Day WHERE Id = '{idDay}' ");
        }

        private void AddDay(string date)
        {
             connection.Execute($@" INSERT INTO Day (Date) VALUES ('{date}') ");
        }
    }
}
