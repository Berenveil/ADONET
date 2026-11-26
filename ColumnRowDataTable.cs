// DataTable (DataRow, DataColumn)
// DataSet
// DataAdapter

// Disconnected
// 1)подключились
// 2)получили данные
// 3)отключились
// 4)локально работаете
// 5)подключились
// 6)загрузки
// 7)отключились

// using System.Data;

// var table = new DataTable();
// table.Column.Add("id", typeof (int));
// table.Column.Add("name", typeof (string));

// table.Rows.Add(1, "Имя");

// foreach (DataRow row in table.Rows)
// {
//     Console.Writeline($"{row["id"]} - {row[name]}");
// }

using System;
using System.Data;

var bookTable = new DataTable();
var colId = new DataColumn("id", typeof (int))
{
    AutoIncrement = true,
    AutoIncrementSeed = 1,
    AutoIncrementStep = 1
};

bookTable.Columns.Add(colId);

var colName = new DataColumn("name", typeof(string))
{
  Unique = true,
  AllowDBNull = false  
};

bookTable.Columns.Add(colName);
bookTable.Columns.Add("year_manufacture", typeof(DateTime));
bookTable.Columns.Add("genre", typeof(string));

bookTable.Rows.Add(null, "1984", new DateTime(1949, 6, 8), "Антиутопия");
bookTable.Rows.Add(null, "Мастер и Маргарита", new DateTime(1966, 1, 1), "Мистический роман");
bookTable.Rows.Add(null, "Дюна", new DateTime(1965, 8, 1), "Научная фантастика");

foreach (DataRow row in bookTable.Rows)
{
    DateTime year_manufactureDate = (DateTime)row["year_manufacture"];
    Console.WriteLine($"{row["id"]} - {row["name"]} - {year_manufactureDate:dd.MM.yyyy} - {row["genre"]}");
}
