using EventManagement.Models;
using System.Collections.Generic;
namespace EventManagement.Contracts
{
    public interface IEventRepository
    {
        /// <summary>
        /// Retrieves a list of all events from the database
        /// </summary>
        /// <param name="tableName">Name of the To fecth from Db</param>
        /// <returns>A list of events</returns>
        public List<Event> GetEvents(string tableName);

        /// <summary>
        /// Retrieves a list of all Event Attendees from the database
        /// </summary>
        /// <returns>List of EventAttendee objects</returns>
        public List<EventAttendee> GetEventAttendeesList(string userId, string tableName);

        /// <summary>
        /// Gets a list of all users in the database.
        /// </summary>
        /// <param name="tableName">Name of the To fecth from Db</param>
        /// <returns>The list of users.</returns>
        public List<User> GetUserList(string tableName);

        /// <summary>
        /// Inserts a new event into the database.
        /// </summary>
        /// <param name="tableName">Name of the To fecth from Db</param>
        /// <param name="events">The event object to be added to the database.</param>
        public void AddEvent(Event events, string tableName);

        /// <summary>
        /// Adds a list of event attendees to the database.
        /// </summary>
        /// <param name="tableName">Name of the To fecth from Db</param>
        /// <param name="attendeesList">The list of event attendees to add.</param>
        public void AddEventAttendee(List<EventAttendee> attendeesList, string tableName);
    }
}