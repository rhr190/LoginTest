using LoginTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Reservation> Get() => CreateDummyReservations();
        public List<Reservation> CreateDummyReservations()
        {
            List<Reservation> returnedList = new List<Reservation>()
            {
                new Reservation{Id = 1, Name = "Unga Bunga", StartLocation = "Beijing", EndLocation="Honolulu"},
                new Reservation{Id = 1, Name = "Chungus Mungus", StartLocation = "Wukong", EndLocation="Atlantis"},
                new Reservation{Id = 1, Name = "Aurist", StartLocation = "Wyouming", EndLocation="Banshee"},
            };
            return returnedList;
        }
    }
}
