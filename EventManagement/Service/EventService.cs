using System.Data.SqlClient;
using EventManagement.Models;
using EventManagement.Contracts;
using EventManagement.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data;

namespace EventManagement.Service
{
    public class EventService : IEventServices
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<EventService> _logger;

        /// <summary>
        /// Creates a new instance of the EventService class.
        /// </summary>
        /// <param name="eventRepository">The event repository to access the data</param>
        /// <param name="logger">The logger to use for logging messages.</param>
        public EventService(IEventRepository eventRepository, ILogger<EventService> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }

        /// <summary>
        /// Create a new event and add to the database
        /// </summary>
        /// <param name="events">The event to create</param>
        /// <returns>Throw Conflict exception if the event name already exists</returns>

        public void CreateEvent(List<Event> events)
        {
            List<Event> eventLists = _eventRepository.GetEvents("event");
            List<string> conflictEvents = new List<string>();
            foreach (var list in events)
            {
                if (eventLists.Any(a => a.EventName == list.EventName))
                {
                    _logger.LogDebug($"Event with this name {list.EventName} already registered");
                    conflictEvents.Add(list.EventName);
                }
                else
                {
                    _eventRepository.AddEvent(list, "Event");
                }
            }
            if (conflictEvents.Count() != 0)
            {
                throw new ConflictException($"Event with this names already registered");
            }
        }
        /// <summary>
        /// Creates a list of event attendees by parsing data from a CSV file.
        /// </summary>
        /// <param name="formFile">The CSV file containing the attendees data.</param>
        /// <returns>
        /// A list of strings representing the names of users who have a clash in their schedule.
        /// If no clashes are found, returns null.
        /// </returns>
        public List<string> CreateEventAttendeeList(IFormFile formFile)
        {
            // Read the contents in the CSV file
            using (var stream = formFile.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
                var lineNumber = 0;
                var records = new List<EventAttendeeDto>();
                // Iterate over the CSV records
                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    // Parse the current line as a CSV record
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    // Map the CSV values to a EventAttendeeDto
                    var record = new EventAttendeeDto
                    {
                        Id = Guid.NewGuid(),
                        UserId = Guid.Parse(values[0]),
                        EventName = (values[1])
                    };
                    records.Add(record);
                }
                // Get data from the database
                List<Event> eventsList = _eventRepository.GetEvents("Event");
                List<User> userList = _eventRepository.GetUserList("Users");
                List<string> clashList = new();
                List<EventAttendee> attendeesList = new();
                foreach (var eventAttendee in records)
                {   // Get the list of events the user is already attending
                    List<EventAttendee> eventAttendees = _eventRepository.GetEventAttendeesList(eventAttendee.UserId.ToString(), "EventAttendee");
                    if (eventAttendees.Count() != 0)
                    {     // Check for schedule clash
                        var eventStartDateTime = eventsList.Where(a => a.EventName == eventAttendee.EventName).Select(a => a.EventStartDateTime).FirstOrDefault();
                        var eventEndDateTime = eventsList.Where(a => a.EventName == eventAttendee.EventName).Select(a => a.EventEndDateTime).FirstOrDefault();
                        List<Event> eventIdsList = new();
                        foreach (var item in eventAttendees)
                        {
                            if (item != null)
                            {
                                //Adds Events in eventIdsList that already registered in EventAttendee Table
                                eventIdsList?.Add(eventsList.Where(a => a.EventId == item.EventId).Select(a => new Event { EventStartDateTime = a.EventStartDateTime, EventEndDateTime = a.EventEndDateTime }).FirstOrDefault());
                            }
                        }
                        if (eventIdsList.Any(a => a.EventStartDateTime <= eventStartDateTime && a.EventEndDateTime >= eventEndDateTime))
                        {
                            // Add the user's name to the clash list
                            clashList.Add(userList.Where(a => a.UserId == eventAttendee.UserId).FirstOrDefault().FirstName);
                        }

                        else
                        {
                            // Add the new attendee to the list
                            var attendees = new EventAttendee();
                            attendees.Id = eventAttendee.Id;
                            attendees.EventId = eventsList.Where(a => a.EventName == eventAttendee.EventName).Select(a => a.EventId).FirstOrDefault();
                            attendees.UserId = eventAttendee.UserId;
                            attendeesList.Add(attendees);
                        }
                    }
                    else
                    {   // Add the new attendee to the list
                        var attendees = new EventAttendee();
                        attendees.Id = eventAttendee.Id;
                        attendees.EventId = eventsList.Where(a => a.EventName == eventAttendee.EventName).Select(a => a.EventId).FirstOrDefault();
                        attendees.UserId = eventAttendee.UserId;
                        attendeesList.Add(attendees);
                    }
                }
                _eventRepository.AddEventAttendee(attendeesList, "EventAttendee");

                _logger.LogInformation("Return the list of users who have a clash in their schedule");
                return clashList;


            }
        }
        /// <summary>
        /// Retrieves a list of events from the database.
        /// </summary>
        /// <param name="eventType">A string indicating which events to retrieve ("AllEvents", "PastEvents", or "UpcomingEvents").</param>
        /// <returns>A list of event objects that match the specified criteria.</returns>
        public List<Event> GetAllEvents(string eventType)
        {
            _logger.LogInformation($"Retrieving {eventType}  from the database");

            DateTime currentDateTime = DateTime.Now;
            List<Event> eventList = _eventRepository.GetEvents("Event");
            if (eventType == "PastEvents")
            {
                _logger.LogInformation($"Filtering events to show past events ");
                eventList = eventList.Where(e => e.EventEndDateTime < currentDateTime).ToList();
            }
            else if (eventType == "UpcomingEvents")
            {
                _logger.LogInformation($"Filtering events to show upcoming events only");
                eventList = eventList.Where(e => e.EventStartDateTime > currentDateTime).ToList();
            }

            _logger.LogInformation($"Retrieved {eventList.Count} events from the database.");
            return eventList;
        }

    }
}