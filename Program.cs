
using System;
using System.Data.SqlClient;

namespace ADOCRUD
{
    internal class Program
    {
        static string cs = @"Server=localhost\SQLEXPRESS;Database=SQLBasicsDB;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n======================================");
                Console.WriteLine("       STUDENT MANAGEMENT SYSTEM");
                Console.WriteLine("======================================");
                Console.WriteLine("1. Insert Student");
                Console.WriteLine("2. View Students");
                Console.WriteLine("3. Update Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Exit");
                Console.WriteLine("======================================");

                Console.Write("Enter your choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input! Enter a number.");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            InsertStudent();
                            break;

                        case 2:
                            ViewStudents();
                            break;

                        case 3:
                            UpdateStudent();
                            break;

                        case 4:
                            DeleteStudent();
                            break;

                        case 5:
                            Console.WriteLine("Exiting Application...");
                            return;

                        default:
                            Console.WriteLine("Invalid Choice!");
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Database Error: " + ex.Message);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid data format!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        // INSERT STUDENT
        static void InsertStudent()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO dbo.Student
                (StudentId, StudentName, Email, Age, City, DepartmentId)
                VALUES (@id, @name, @email, @age, @city, @dept)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    Console.Write("Enter Student ID: ");
                    int id = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter Name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter Email: ");
                    string email = Console.ReadLine();

                    Console.Write("Enter Age: ");
                    int age = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter City: ");
                    string city = Console.ReadLine();

                    Console.Write("Enter Department ID (1, 2, 3 or blank): ");
                    string deptInput = Console.ReadLine();

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);

                    cmd.Parameters.AddWithValue("@email",
                        string.IsNullOrWhiteSpace(email)
                        ? (object)DBNull.Value : email);

                    cmd.Parameters.AddWithValue("@age", age);

                    cmd.Parameters.AddWithValue("@city",
                        string.IsNullOrWhiteSpace(city) ? "Agra" : city);

                    cmd.Parameters.AddWithValue("@dept",
                        string.IsNullOrWhiteSpace(deptInput)
                        ? (object)DBNull.Value
                        : Convert.ToInt32(deptInput));

                    con.Open();

                    int rows = cmd.ExecuteNonQuery();

                    Console.WriteLine(rows > 0
                        ? "Student Inserted Successfully!"
                        : "Student Not Inserted!");
                }
            }
        }

        // VIEW STUDENTS
        static void ViewStudents()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"SELECT StudentId, StudentName, Email,
                                 Age, City, DepartmentId
                                 FROM dbo.Student";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine();

                        Console.WriteLine(
                            "{0,-6} {1,-20} {2,-30} {3,-6} {4,-15} {5,-10}",
                            "ID", "Name", "Email", "Age", "City", "Dept ID");

                        Console.WriteLine(new string('-', 92));

                        while (reader.Read())
                        {
                            Console.WriteLine(
                                "{0,-6} {1,-20} {2,-30} {3,-6} {4,-15} {5,-10}",
                                reader["StudentId"],
                                reader["StudentName"],
                                reader["Email"] == DBNull.Value ? "NULL" : reader["Email"],
                                reader["Age"],
                                reader["City"],
                                reader["DepartmentId"] == DBNull.Value ? "NULL" : reader["DepartmentId"]
                            );
                        }
                    }
                }
            }
        }

        // UPDATE STUDENT
        static void UpdateStudent()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE dbo.Student
                                 SET StudentName = @name,
                                     City = @city
                                 WHERE StudentId = @id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    Console.Write("Enter Student ID: ");
                    int id = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter New Name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter New City: ");
                    string city = Console.ReadLine();

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@city", city);

                    con.Open();

                    int rows = cmd.ExecuteNonQuery();

                    Console.WriteLine(rows > 0
                        ? "Student Updated Successfully!"
                        : "Student ID Not Found!");
                }
            }
        }

        // DELETE STUDENT
        static void DeleteStudent()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM dbo.Student WHERE StudentId = @id";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    Console.Write("Enter Student ID to Delete: ");
                    int id = Convert.ToInt32(Console.ReadLine());

                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();

                    int rows = cmd.ExecuteNonQuery();

                    Console.WriteLine(rows > 0
                        ? "Student Deleted Successfully!"
                        : "Student ID Not Found!");
                }
            }
        }
    }
}
