using Microsoft.Data.Sqlite;

using var connection = new SqliteConnection("Data Source=Test.db;");
connection.Open();
using var transaction = connection.BeginTransaction();

try
{
string query = @"
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Age INTEGER
        );
    ";
using var command = new SqliteCommand(query, connection);
command.Transaction = transaction;
command.ExecuteNonQuery();

string query2 = @"
        INSERT INTO Users (Name, Age)
        VALUES ('Lusine', 28);
    ";
using var command2 = new SqliteCommand(query2, connection);
command2.Transaction = transaction;
command2.ExecuteNonQuery();

transaction.Commit();
Console.WriteLine("Таблица создана");
}
catch (Exception ex)
{
transaction.Rollback();
Console.WriteLine($"Ошибка: {ex.Message}");
}
