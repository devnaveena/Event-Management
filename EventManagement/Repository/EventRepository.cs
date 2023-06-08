using System.Data.SqlClient;
using EventManagement.Models;
using EventManagement.Contracts;
using System.Data;

namespace EventManagement.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly string _connectionString;
        private SqlConnection _connection = null!;

        public readonly ILogger<EventRepository> _logger;
        public EventRepository(IConfiguration configuration, ILogger<EventRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("MyConnectionString");
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all events from the database
        /// </summary>
        /// <param name="tableName">Name of the To fecth from Db</param>
        /// <returns>A list of events</returns>
        public List<Event> GetEvents(string tableName)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                _connection.Open();

                List<Event> eventsList = new List<Event>();

                string sqlQuery = $"SELECT * FROM {tableName}";
                using (SqlCommand command = new SqlCommand(sqlQuery, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Event events = new Event();
                            events.EventId = new Guid(reader.GetString("Event_Id"));
                            events.EventName = reader.GetString("Event_Name");
                            events.EventStartDateTime = reader.GetDateTime("Event_Start_DateTime");
                            events.EventEndDateTime = reader.GetDateTime("Event_End_DateTime");
                            eventsList.Add(events);
                        }
                    }

                }
                _connection.Close();
                _logger.LogInformation($"Retrieves a list of all events from the database");
                return eventsList;
            }
        }

        /// <summary>
        /// Retrieves a list of all Event Attendees based on userId from the database
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tableName">Name of the To fecth from Db</param>
        /// <returns>List of EventAttendee objects</returns>

        public List<EventAttendee> GetEventAttendeesList(string tableName, string userId)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                List<EventAttendee> eventsList = new List<EventAttendee>();


                string sqlQuery = $"SELECT * FROM {tableName} where User_Id='{userId}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EventAttendee events = new EventAttendee();
                            events.EventId = new Guid(reader.GetString("Event_Id"));
                            events.UserId = new Guid(reader.GetString("User_Id"));
                            events.Id = new Guid(reader.GetString("Id"));
                            eventsList.Add(events);
                        }
                    }

                }
                _connection.Close();

                _logger.LogInformation($"Retrieved list of {eventsList.Count} Event Attendees");
                return eventsList;
            }
        }

        /// <summary>
        /// Gets a list of all users in the database.
        /// </summary>
        /// <returns>The list of users.</returns>
        /// <param name="tableName">Name of the To fecth from Db</param>


        public List<User> GetUserList(string tableName)
        {

            using (_connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                List<User> userList = new List<User>();
                string sqlQuery = $"SELECT * FROM {tableName}";
                using (SqlCommand command = new SqlCommand(sqlQuery, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User();
                            user.UserId = new Guid(reader.GetString("user_id"));
                            user.FirstName = reader.GetString("first_name");
                            userList.Add(user);
                        }
                    }

                }
                _connection.Close();

                return userList;
            }


        }
        /// <summary>
        /// Inserts a new event into the database.
        /// </summary>
        /// <param name="events">The event object to be added to the database.</param>
        /// <param name="tableName">Name of the To fecth from Db</param>

        public void AddEvent(Event events, string tableName)
        {

            using (_connection = new SqlConnection(_connectionString))
            {
                _connection.Open();

                Guid eventId = Guid.NewGuid();
                string query = $"INSERT INTO {tableName} (Event_Id ,Event_Name ,Event_Start_DateTime ,Event_End_DateTime ) VALUES (@Event_Id,@Event_Name,@Event_Start_DateTime,@Event_End_DateTime)";
                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Event_Id", eventId);
                    command.Parameters.AddWithValue("@Event_Name", events.EventName);
                    command.Parameters.AddWithValue("@Event_Start_DateTime", events.EventStartDateTime);
                    command.Parameters.AddWithValue("@Event_End_DateTime", events.EventEndDateTime);
                    command.ExecuteNonQuery();
                    _logger.LogDebug($"Event with this name {events.EventName} Created");

                }
            }
            _connection.Close();



        }
        /// <summary>
        /// Adds a list of event attendees to the database.
        /// </summary>
        /// <param name="attendeesList">The list of event attendees to add.</param>
        /// <param name="tableName">Name of the To fecth from Db</param>

        public void AddEventAttendee(List<EventAttendee> attendeesList, string tableName)
        {
            // Insert the records into the database
            using (_connection = new SqlConnection(_connectionString))
            {
                _connection.Open();
                foreach (var record in attendeesList)
                {
                    using (var command = new SqlCommand($"INSERT INTO {tableName} (Id,User_Id,Event_Id) VALUES (@Id, @UserId, @EventId)", _connection))
                    {
                        command.Parameters.AddWithValue("@Id", record.Id);
                        command.Parameters.AddWithValue("@UserId", record.UserId);
                        command.Parameters.AddWithValue("@EventId", record.EventId);
                        command.ExecuteNonQuery();
                    }
                }
                _connection.Close();
            }

            _logger.LogInformation($"Added {attendeesList.Count} event attendees to the database.");
        }



    }
}