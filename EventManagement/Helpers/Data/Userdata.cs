using System.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using EventManagement.Models;


public class Userdata
{
    private readonly string connectionString;

    public Userdata(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public void importUserData()
    {
        var filePath = "C:/Users/NaveenaThangavel/Desktop/Sample/Event/EventManagement/Helpers/Data/UserData.csv";
        var csvBytes = File.ReadAllBytes(filePath);
        var formFile = new FormFile(new MemoryStream(csvBytes), 0, csvBytes.Length, "test", Path.GetFileName(filePath));
        using (var stream = formFile.OpenReadStream())
        using (var reader = new StreamReader(stream))
        {
            var lineNumber = 0;
            var records = new List<User>();
            // Iterate over the CSV records
            while (!reader.EndOfStream)
            {
                lineNumber++;
                // Parse the current line as a CSV record
                string? line = reader.ReadLine();
                var values = line.Split(',');
                // Map the CSV values to a EventAttendeeDto
                var userList = new User
                {
                    UserId = Guid.NewGuid(),
                    FirstName = (values[0])
                };
                records.Add(userList);
            }
           
            // Insert the records into the database
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                foreach (var record in records)
                {
                    using (var command = new SqlCommand("INSERT INTO Users (user_id, first_name) VALUES (@UserId,@FirstName)", connection))
                    {
                        command.Parameters.AddWithValue("@UserId", record.UserId);
                        command.Parameters.AddWithValue("@FirstName", record.FirstName);
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
           
            }




        }
    }
