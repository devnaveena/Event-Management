using Moq;
using EventManagement.Controllers;
using EventManagement.Models;
using EventManagement.Contracts;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Net;
using EventManagement.Helpers;
using Microsoft.Extensions.Logging;
using EventManagement.Repository;
namespace XunitTest.Testing
{

    public class EventTest
    {
        private IEventServices _services;
        private EventManagementController _controller;
        private Mock<IEventRepository> _mockRepository;
        private ILogger<EventManagementController> _loggerController;
        private ILogger<EventService> _loggerService;
        private ILogger<EventRepository> _loggerRepository;



        public EventTest()
        {
            _mockRepository = new Mock<IEventRepository>();
            var loggerFactory = LoggerFactory.Create(builder =>
          {
              builder.AddConsole();
          });

            _loggerController = loggerFactory.CreateLogger<EventManagementController>();
            _loggerService = loggerFactory.CreateLogger<EventService>();
            _loggerRepository = loggerFactory.CreateLogger<EventRepository>();
            _services = new EventService(_mockRepository.Object, _loggerService);
            _controller = new EventManagementController(_services, _loggerController);
        }
        /// <summary>
        /// Tests  the CreateEvent method in the controller returns a StatusCodeResult with a status code of 201 (Created) when a new event is successfully created.
        /// </summary>
        [Fact]
        public void CreateEvent_Events_ReturnsOkObjectResult()
        {
            List<Event> eventList = new List<Event>();
            Event events = new Event();
            events.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB");
            events.EventName = "Onamfest";
            events.EventStartDateTime = DateTime.Parse("2023-04-08T08:30:00Z");
            events.EventEndDateTime = DateTime.Parse("2023-04-08T09:30:00Z");
            List<Event> eventLists = new List<Event>();
            _mockRepository.Setup(a => a.GetEvents("Event")).Returns(eventLists);

            //Act
            IActionResult result = _controller.CreateEvent(eventList);

            //Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);

        }
        /// <summary>
        /// Tests  the CreateEvent method in the controller returns a StatusCodeResult with a status code of 201 (Created) when a new event is successfully created.
        /// </summary>
        [Fact]
        public void CreateEvent_Events_ReturnsBadRequest()
        {
            List<Event> eventList = new List<Event>();
            Event events = new Event();
            events.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB");
            events.EventName = "Onamfest";
            events.EventStartDateTime = DateTime.Parse("2023-04-08T08:30:00Z");
            events.EventEndDateTime = DateTime.Parse("2023-04-08T09:30:00Z");
            List<Event> eventLists = new List<Event>();
            _mockRepository.Setup(a => a.GetEvents("Event")).Returns(eventLists);

            //Act
            IActionResult result = _controller.CreateEvent(eventList);

            //Assert
            var statusCodeResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, statusCodeResult.StatusCode);

        }
        /// <summary>
        /// Tests  the CreateEvent method in the controller returns a ConflictObjectResult when the repository already contains an event with the same Id.
        /// </summary>

        [Fact]
        public void CreateEvent_ConflictResult_ThrowsConflictException()
        {
            //User input dummy data for creating Event
            List<Event> eventList = new List<Event>();
            Event events = new Event();
            events.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB");
            events.EventName = "Onamfest";
            events.EventStartDateTime = DateTime.Parse("2023-04-08T08:30:00Z");
            events.EventEndDateTime = DateTime.Parse("2023-04-08T09:30:00Z");
            events.EventId = Guid.Parse("7D5FDEFD-8217-4411-BFAA-F7D856EBF9BB");
            events.EventName = "fest";
            events.EventStartDateTime = DateTime.Parse("2023-04-08T08:30:00Z");
            events.EventEndDateTime = DateTime.Parse("2023-04-08T09:30:00Z");
            //List of event in the DB to check Conflict
            List<Event> eventLists = new List<Event>();
            Event creatEvent = new Event();
            creatEvent.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB");
            creatEvent.EventName = "Onamfest";
            creatEvent.EventStartDateTime = DateTime.Parse("2023-04-08T08:30:00Z");
            creatEvent.EventEndDateTime = DateTime.Parse("2023-04-08T09:30:00Z");
            eventList.Add(creatEvent);
            _mockRepository.Setup(a => a.GetEvents("Event")).Returns(eventLists);

            //Act
            IActionResult result = _controller.CreateEvent(eventList);

            //Assert

            Assert.IsType<ConflictObjectResult>(result);
        }
        ///<summary>
        ///Tests the GetEvents() action of the EventsController to return all events with a 200 status code.
        ///</summary>
        [Fact]
        public void GetEvents_AllEvents_ReturnsListofEvents()
        {
            string getEvent = "";
            //List of Events in Db 
            List<Event> events = new List<Event>();
            Event creatEvent = new Event();
            creatEvent.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB"); ;
            creatEvent.EventName = "Onamfest";
            creatEvent.EventStartDateTime = DateTime.Parse("2023-04-08T08:30:00Z");
            creatEvent.EventEndDateTime = DateTime.Parse("2023-04-08T09:30:00Z");
            events.Add(creatEvent);
            creatEvent.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB"); ;
            creatEvent.EventName = "Onamfest";
            creatEvent.EventStartDateTime = DateTime.Parse("2023-04-08T08:30:00Z");
            creatEvent.EventEndDateTime = DateTime.Parse("2023-04-08T09:30:00Z");
            events.Add(creatEvent);

            _mockRepository.Setup(a => a.GetEvents("Event")).Returns(events);

            //Act
            IActionResult result = _controller.GetEvents(getEvent);

            //Assert
            var results = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(results);
        }
        ///<summary>
        ///Tests if the controller returns an OK object result with the list of past events
        ///</summary>

        [Fact]
        public void GetEvents_PastEvents_ReturnsListofPastevents()
        {
            string getEvent = "PastEvents";
            //List of Events in Db 
            List<Event> events = new List<Event>();
            Event creatEvent = new Event();
            creatEvent.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB"); ;
            creatEvent.EventName = "Onamfest";
            creatEvent.EventStartDateTime = DateTime.Parse("2023-04-08T08:30:00");
            creatEvent.EventEndDateTime = DateTime.Parse("2023-04-09T08:30:00");
            events.Add(creatEvent);
            creatEvent.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB"); ;
            creatEvent.EventName = "Meeting";
            creatEvent.EventStartDateTime = DateTime.Parse("2023-04-14T08:30:00Z");
            creatEvent.EventEndDateTime = DateTime.Parse("2023-04-14T09:30:00Z");
            events.Add(creatEvent);

            _mockRepository.Setup(a => a.GetEvents("Event")).Returns(events);

            //Act
            IActionResult result = _controller.GetEvents(getEvent);

            //Assert
            if (result is OkObjectResult)
            {
                var results = result as OkObjectResult;
                Assert.IsType<OkObjectResult>(results);
            }
            else
            {
                var results = result as ObjectResult;
                Assert.Equal(204, (results.Value as ErrorDto).StatusCode);
            }

        }
        /// <summary>
        /// Test to check if the GetEvents method returns a list of upcoming events.
        /// </summary>
        [Fact]
        public void GetEvents_UpcomingEvents_ReturnsListOfUpcomingEvents()
        {
            string getEvent = "UpcomingEvents";
            //List of Events in Db 
            List<Event> events = new List<Event>();
            Event creatEvent = new Event();
            creatEvent.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB");
            creatEvent.EventName = "Onamfest";
            creatEvent.EventStartDateTime = DateTime.Parse("2023-04-08T08:30:00Z");
            creatEvent.EventEndDateTime = DateTime.Parse("2023-04-08T09:30:00Z");
            events.Add(creatEvent);
            creatEvent.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB"); ;
            creatEvent.EventName = "Meeting";
            creatEvent.EventStartDateTime = DateTime.Parse("2023-04-14T08:30:00Z");
            creatEvent.EventEndDateTime = DateTime.Parse("2023-04-14T09:30:00Z");
            events.Add(creatEvent);

            _mockRepository.Setup(a => a.GetEvents("Event")).Returns(events);

            //Act
            IActionResult result = _controller.GetEvents(getEvent);

            //Assert
            if (result is OkObjectResult)
            {
                var results = result as OkObjectResult;
                Assert.IsType<OkObjectResult>(results);
            }
            else
            {
                var results = result as ObjectResult;
                Assert.Equal(204, (results.Value as ErrorDto).StatusCode);
            }

        }
        /// <summary>
        /// Tests that the GetEvents method in the controller returns a NoContentResult when the repository returns an empty list of events.
        /// </summary>
        [Fact]
        public void GetEvents_NoContent_ObjectResult()
        {

            string getEvent = "";
            List<Event> events = new List<Event>();

            _mockRepository.Setup(a => a.GetEvents("Event")).Returns(events);

            // Act
            var result = _controller.GetEvents(getEvent) as ObjectResult;

            // Assert
            Assert.Equal(204, (result.Value as ErrorDto).StatusCode);


        }
        /// <summary>
        /// This test method checks if the CreateEventAttendee action method of the EventAttendeeController returns an OkObjectResult when given valid input.
        /// </summary>
        [Fact]
        public void CreateEventAttendee_Ok_ObjectResult()
        {
            //Csv file for User input 
            var filePath = "C:/Users/NaveenaThangavel/Desktop/Sample/Event/EventManagement/Helpers/Data/DummyDatas.csv";
            var csvBytes = File.ReadAllBytes(filePath);
            string userId = Guid.Parse("1963a254-6d0f-47d0-8b8c-ee9246362207").ToString();
            var formFile = new FormFile(new MemoryStream(csvBytes), 0, csvBytes.Length, "test", Path.GetFileName(filePath));

            // Creating List users for generate Clash List
            List<User> usersList = new List<User>();
            User user = new User();
            user.UserId = Guid.Parse("1963a254-6d0f-47d0-8b8c-ee9246362207");
            user.FirstName = "Naveena";
            usersList.Add(user);

            //Creating List of event attendees to check wheather any conficts  while registering with another event
            List<EventAttendee> eventAttendees = new List<EventAttendee>();
            EventAttendee eventAttendee = new EventAttendee();
            eventAttendee.Id = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB");
            eventAttendee.EventId = Guid.Parse("afecfa21-67b9-4c72-a16b-525e3b817e4c");
            eventAttendee.UserId = Guid.Parse("1963a254-6d0f-47d0-8b8c-ee9246362207");
            eventAttendees.Add(eventAttendee);
            eventAttendee.Id = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB");
            eventAttendee.EventId = Guid.Parse("7D5FDEFD-8217-4411-BFAA-F7D856EBF9BB");
            eventAttendee.UserId = Guid.Parse("1963a254-6d0f-47d0-8b8c-ee9246362207");
            eventAttendees.Add(eventAttendee);

            //Dummy events for comparing with user Input data
            List<Event> eventsList = new List<Event>();
            Event creatEvent = new Event();
            creatEvent.EventId = Guid.Parse("afecfa21-67b9-4c72-a16b-525e3b817e4c");
            creatEvent.EventName = "Onamfest";
            creatEvent.EventStartDateTime = DateTime.Parse("2023-04-14T08:30:00Z");
            creatEvent.EventEndDateTime = DateTime.Parse("2023-04-16T09:30:00Z");
            eventsList.Add(creatEvent);
            creatEvent.EventId = Guid.Parse("7F5FDEFD-8217-4411-BFAA-F7D856EBF9BB"); ;
            creatEvent.EventName = "Holi";
            creatEvent.EventStartDateTime = DateTime.Parse("2023-04-14T08:30:00Z");
            creatEvent.EventEndDateTime = DateTime.Parse("2023-04-14T09:30:00Z");
            eventsList.Add(creatEvent);
            creatEvent.EventId = Guid.Parse("7D5FDEFD-8217-4411-BFAA-F7D856EBF9BB");
            creatEvent.EventName = "Meeting";
            creatEvent.EventStartDateTime = DateTime.Parse("2023-04-14T08:30:00Z");
            creatEvent.EventEndDateTime = DateTime.Parse("2023-05-14T09:30:00Z");
            eventsList.Add(creatEvent);


            List<String> clash = new List<string>();
            List<EventAttendee> attendeesList = new();
            List<Event> eventIdsList = new();

            _mockRepository.Setup(a => a.GetEventAttendeesList(userId, "EventAttendee")).Returns(eventAttendees);
            _mockRepository.Setup(a => a.GetEvents("Event")).Returns(eventsList);
            _mockRepository.Setup(a => a.GetUserList("Users")).Returns(usersList);

            //Act
            IActionResult result = _controller.CreateEventAttendees(formFile);

            //Assert
            var results = result as OkObjectResult;
            Assert.IsType<OkObjectResult>(results);

        }
        /// <summary>
        /// This test method checks if the CreateEventAttendee giving Empty data in csv.
        /// </summary>
        [Fact]
        public void CreateEventAttendee_BadRequest_ObjectResult()
        {
            var filePath = "C:/Users/NaveenaThangavel/Desktop/Sample/Event/EventManagement/Helpers/Data/EmptyList.csv";
            var csvBytes = File.ReadAllBytes(filePath);
            string userId = Guid.Parse("1963a254-6d0f-47d0-8b8c-ee9246362207").ToString();
            var formFile = new FormFile(new MemoryStream(csvBytes), 0, csvBytes.Length, "test", Path.GetFileName(filePath));
            IActionResult result = _controller.CreateEventAttendees(formFile);

            //Assert
            var results = result as BadRequestObjectResult;
            Assert.IsType<BadRequestObjectResult>(results);
        }
    }
}
