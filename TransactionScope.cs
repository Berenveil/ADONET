using Microsoft.Data.Sqlite;
using System.Transactions;

TransactionScope;
//определяет границы транзакции

using (var transaction = new TransactionScope())
{
    using (var connection = new SqliteConnection("Data Source=cars.db"))
    {
        connection.Open();
        using var command1 = new SqliteCommand("UPDATE Car SET Price = @Price WHERE Id = @Id", connection);
        command1.Parameters.AddWithValue("@Price", newPrice);
        command1.Parameters.AddWithValue("@Id", newCarId);
        command1.ExecuteNonQuery();
        using var command2 = new SqliteCommand("", connection);
        command2.ExecuteNonQuery();
    }
    transaction.Complete();
}
