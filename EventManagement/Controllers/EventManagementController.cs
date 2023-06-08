using Microsoft.AspNetCore.Mvc;
using EventManagement.Models;
using EventManagement.Contracts;
using EventManagement.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;

namespace EventManagement.Controllers
{
    /// <summary>
    /// Represents a controller for Eventmangement.
    /// </summary>
    [ApiController]
    [Route("api/event-management")]
    public class EventManagementController : ControllerBase
    {
        private readonly IEventServices _eventService;
        private readonly ILogger<EventManagementController> _logger;

        /// <summary>
        /// Creates a new instance of the EventManagementController class.
        /// </summary>
        /// <param name="services">The event service  to use for event repository</param>
        /// <param name="logger">The logger to use for logging messages.</param>
        public EventManagementController(IEventServices eventServices, ILogger<EventManagementController> logger)
        {
            _eventService = eventServices;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new event in the database
        /// </summary>
        /// <param name="events">Contains the Event details to create</param>
        /// <returns>
        /// A response gives the success or failure of the creation process:
        /// - 201 Created: The event was successfully created in the database.
        /// - 400 Bad Request: The user input is not valid.
        /// - 409 Conflict: The user input conflicts with existing data in the database.
        /// </returns>

        [HttpPost]
        public IActionResult CreateEvent([FromBody] List<Event> events)
        {

            if (events == null && !ModelState.IsValid)
            {
                _logger.LogError("Invalid data provided");
                return BadRequest(new ErrorDto { ErrorMessage = "BadRequest", StatusCode = (int)HttpStatusCode.BadRequest, Description = "End date must be after start date." });
            }
            try
            {
                _eventService.CreateEvent(events);
                {
                    _logger.LogInformation($"Events created successfully");
                    return StatusCode(StatusCodes.Status201Created);
                }
            }
            catch (ConflictException e)
            {
                _logger.LogError($"An event with this name already exists");
                return Conflict(new ErrorDto { ErrorMessage = "Conflict", StatusCode = (int)HttpStatusCode.Conflict, Description = e.Message });
            }

        }


        /// <summary>
        /// Retrieves a list of events from the  database.
        /// </summary>
        /// <param name="eventType">The type of events to retrieve such as past or upcoming</param>
        /// <returns>
        /// A response gives the success or failure of the retrieval process:
        /// - 200 OK: The events were successfully retrieved from the database.
        /// - 204 No Content: There were no events matching the specified criteria.
        /// </returns>

        [HttpGet]
        public IActionResult GetEvents([FromQuery] string eventType)
        {
            List<Event> eventList = _eventService.GetAllEvents(eventType);

            if (eventList.Count() == 0)
            {
                _logger.LogDebug($"No {eventType} registered");
                return StatusCode(StatusCodes.Status204NoContent, new ErrorDto { ErrorMessage = "No Content", StatusCode = (int)HttpStatusCode.NoContent, Description = $"No {eventType} registered" });
            }

            _logger.LogInformation($"List of {eventType}");
            return Ok(eventList);

        }

        /// <summary>
        /// Creates a list of event attendees from a CSV file.
        /// </summary>
        /// <param name="attendeeList">The CSV file containing the list of attendees.</param>
        /// <returns>A list of strings representing any clashes in the attendee schedule.</returns>

        [HttpPost("event-attendee")]
        public IActionResult CreateEventAttendees(IFormFile attendeeList)
        {
            // Check if a file was uploaded
            _logger.LogInformation("Creating event attendeeList");

            if (attendeeList == null || attendeeList.Length == 0)
            {
                return BadRequest("No CSV file was uploaded.");
            }

            List<String> conflictList = _eventService.CreateEventAttendeeList(attendeeList);
            if (conflictList.Count() == 0)
            {
                _logger.LogInformation("Created Event Attendees list without any clashes in the attendee schedule.");
                return Ok();
            }
            return Ok(conflictList);
        }
    }
}
