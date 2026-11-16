using Microsoft.Data.Sqlite;

namespace EmployeeApp
{
    using SQLitePCL;
    using System;

    namespace EmployeeApp
    {
        public class Employee
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public required string Position { get; set; }
            public decimal Salary { get; set; }

            public override string ToString()
            {
                return $"[{Id}] {Name} - {Position}, Зарплата: {Salary}";
            }
        }
        public class EmployeeRepository
        {
            private readonly string _connectionString;

            public EmployeeRepository(string connectionString)
            {
                _connectionString = connectionString;
                InitializeDatabase();
            }

            private void InitializeDatabase()
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                string query = @"
                CREATE TABLE IF NOT EXISTS Employees (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Position TEXT NOT NULL,
                Salary REAL NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Logs(
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Action TEXT NOT NULL 
                );";

                using var command = new SqliteCommand(query, connection);
                command.ExecuteNonQuery();
            }
            //CREATE
            public void Add(Employee employee, string log)
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                using var transaction = connection.BeginTransaction();

                try
                {
                    string queryEmployeeInsert = @"
                                                    INSERT INTO Employee (Name, Position, Salary)
                                                    VALUES (@Name, @Position, @Salary);
                                                ";
                    using var commandEmployee = new SqliteCommand(queryEmployeeInsert, connection, transaction);
                    commandEmployee.Parameters.AddWithValue("@Name", employee.Name);
                    commandEmployee.Parameters.AddWithValue("@Position", employee.Position);
                    commandEmployee.Parameters.AddWithValue("@Salary", employee.Salary);
                    //command2.Transaction = transaction;
                    commandEmployee.ExecuteNonQuery();

                    string queryLogInsert = @"
                                            INSERT INTO Log (Action)
                                            VALUES (@Action);
                                        ";
                    using var commandLog = new SqliteCommand(queryLogInsert, connection, transaction);
                    commandLog.Parameters.AddWithValue("@Action", log.Action);
                    commandLog.ExecuteNonQuery();

                    transaction.Commit();
                    Console.WriteLine("Таблица создана");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
            //READ
            public List<Employee> GetAll()
            {
                var employees = new List<Employee>();

                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                string query = "SELECT Id, Name, Position, Salary FROM Employees;";
                using var command = new SqliteCommand(query, connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Position = reader.GetString(2),
                        Salary = reader.GetDecimal(3)
                    });
                }
                return employees;
            }
            public void Update(Employee employee)
            {

            }
            //DELETE
            public void Delete(int id)
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();
                string query = @"DELETE FROM Employees WHERE Id = @Id;";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id ", id);
                command.ExecuteNonQuery();
            }
        }
    }
}
