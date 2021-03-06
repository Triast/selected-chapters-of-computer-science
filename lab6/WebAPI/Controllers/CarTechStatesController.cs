﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/CarTechStates")]
    public class CarTechStatesController : Controller
    {
        readonly CarServiceContext _context;

        public CarTechStatesController(CarServiceContext context)
        {
            _context = context;
        }

        // GET: api/CarTechStates
        [HttpGet]
        public IEnumerable<CarTechState> Get()
        {
            return _context
                .CarTechStates
                .Include("Car")
                .Include("Inspector");
        }

        // GET: api/CarTechStates/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var state = _context.CarTechStates.FirstOrDefault(s => s.CarTechStateId == id);

            if (state == null)
            {
                return NotFound();
            }

            return new ObjectResult(state);
        }
        
        // POST: api/CarTechStates
        [HttpPost]
        public IActionResult Post([FromBody]CarTechState model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            _context.CarTechStates.Add(model);
            _context.SaveChanges();

            return Ok(model);
        }
        
        // PUT: api/CarTechStates/5
        [HttpPut("{id}")]
        public IActionResult Put([FromBody]CarTechState state)
        {
            if (state == null)
            {
                return BadRequest();
            }

            var fromDb = _context.CarTechStates.FirstOrDefault(s => s.CarTechStateId == state.CarTechStateId);

            if (fromDb == null)
            {
                return NotFound();
            }

            fromDb.CarId = state.CarId;
            fromDb.InspectorId = state.InspectorId;
            fromDb.Date = state.Date;
            fromDb.Mileage = state.Mileage;
            fromDb.BrakeSystem = state.BrakeSystem;
            fromDb.Suspension = state.Suspension;
            fromDb.Wheels = state.Wheels;
            fromDb.Lightning = state.Lightning;
            fromDb.AdditionalEquipment = state.AdditionalEquipment;
            fromDb.MarkOnPassageOfServiceStation = state.MarkOnPassageOfServiceStation;

            _context.Update(fromDb);
            _context.SaveChanges();

            return Ok(state);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var state = _context.CarTechStates.FirstOrDefault(s => s.CarTechStateId == id);

            if (state == null)
            {
                return NotFound();
            }

            _context.CarTechStates.Remove(state);
            _context.SaveChanges();

            return Ok(state);
        }
    }
}
