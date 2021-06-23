using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celObj = _context.CelestialObjects.Find(id);
            
            if (celObj == null)
                return NotFound();
            celObj.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == id).ToList();
            return Ok(celObj);

        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celObjs = _context.CelestialObjects.Where(c => c.Name == name);

            if (!celObjs.Any())
                return NotFound();
            foreach (var celObj in celObjs)
            {
                celObj.Satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celObj.Id).ToList();
            }
            return Ok(celObjs);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celObjs = _context.CelestialObjects.ToList();
            foreach (var celObj in celObjs)
            {
                celObj.Satellites = _context.CelestialObjects.Find(celObj.Id).Satellites;
            }
            return Ok(celObjs);
        }
    }
}
