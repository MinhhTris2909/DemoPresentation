using DemoPresentation.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DemoPresentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private IRepository repository;

        private IWebHostEnvironment webHostEnvironment;

        public ReservationController (IRepository repo, IWebHostEnvironment environment)
        {
            repository = repo;
            webHostEnvironment = environment;
        }

        [HttpGet]
        public IEnumerable<Reservation> Get() => repository.Reservations;
        
        [HttpGet("{id}")]
        public ActionResult<Reservation> Get(int id)
        {
            if (id == 0)
            {
                return BadRequest("Value must be passed in the request body");
            }
            return Ok(repository[id]);
        }

        [HttpPost]
        public Reservation Post([FromBody] Reservation res) =>
        repository.AddReservation(new Reservation
        {
            Name = res.Name,
            startLocation = res.startLocation,
            endLocation = res.endLocation,
        });

        [HttpPut]
        public Reservation Put([FromForm] Reservation res) => repository.UpdateReservation(res);

        [HttpDelete("{id}")]
        public void Delete(int id) => repository.DeleteReservation(id);

        bool Authenicate()
        {
            var allowedKeys = new[] { "Secret@123", "Secret#12", "SecretABC" };
            StringValues key = Request.Headers["Key"];
            int count = (from t in allowedKeys where t == key select t).Count();
            return count == 0 ? false : true;
        }

        [HttpPost("PostXml")]
        [Consumes("application/xml")] 
        public Reservation PostXml ([FromBody] System.Xml.Linq.XElement res)
        {
            return repository.AddReservation(new Reservation
            {
                Name = res.Element("Name").Value,
                startLocation = res.Element("startLocation").Value,
                endLocation = res.Element("endLocation").Value
            });
        }

        [HttpGet("ShowReservation.{format}"), FormatFilter]
        public IEnumerable<Reservation> ShowReservation() => repository.Reservations;
    }
}
