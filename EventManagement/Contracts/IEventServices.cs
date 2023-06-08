using EventManagement.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EventManagement.Helpers;
using System.Collections.Generic;
namespace EventManagement.Contracts
{

    public interface IEventServices
    {
        /// <summary>
        /// Create a new event and add to the database
        /// </summary>
        /// <param name="events">The event to create</param>


        void CreateEvent(List<Event> events);

        /// <summary>
        /// Retrieves a list of events from the database.
        /// </summary>
        /// <param name="eventType">A string indicating which events to retrieve ("AllEvents", "PastEvents", or "UpcomingEvents").</param>
        /// <returns>A list of event objects that match the specified criteria.</returns>
        public List<Event> GetAllEvents(string events);

        /// <summary>
        /// Creates a list of event attendees by parsing data from a CSV file.
        /// </summary>
        /// <param name="formFile">The CSV file containing the attendees data.</param>
        /// <returns>
        /// A list of strings representing the names of users who have a clash in their schedule.
        /// If no clashes are found, returns null.
        /// </returns>
        public List<string> CreateEventAttendeeList(IFormFile formFile);


    }
}