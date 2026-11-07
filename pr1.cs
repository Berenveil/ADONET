using Microsoft.Data.Sqlite;

string connectionPath = "Data Source=Test.db;";

using (var connection = new SqliteConnection(connectionPath))
{
    connection.Open();
    Console.WriteLine("Успешное соедиенение с бд!");
    string query = @"CREATE TABLE IF NOT EXISTS Students (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                GroupName TEXT NOT NULL
            );";
    using (var command = new SqliteCommand(query, connection))
    {
        command.ExecuteNonQuery();
        Console.WriteLine("таблица создана в бд!");
        command.CommandText = "INSERT INTO Students (Fullname, GroupName) VALUES ('Test1','Test2')";
        command.ExecuteNonQuery();
    }
    query = "SELECT * FROM Students";
    using (var command = new SqliteCommand(query, connection))
    {
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                string group = reader.GetString(2);
                Console.WriteLine($"[{id} {name} - {group}]");
            }
        }
        query = "SELECT COUNT(*) FROM Students";
        command.CommandText = query;
        long countStudents = (long)command.ExecuteScalar();
        Console.WriteLine($"Count: {countStudents}");
    }
    
}
