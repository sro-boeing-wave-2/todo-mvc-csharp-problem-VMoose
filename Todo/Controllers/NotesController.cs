using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using Todo.Models;
using Todo.service;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        DataAccess objds;

        public NotesController(DataAccess data)
        {
            objds = data;
        }

        [HttpGet]
        public IEnumerable<Note> Get()
        {
            return objds.GetNotes();
        }
        [HttpGet("{id:length(24)}")]
        public IActionResult Get(string id)
        {
            var note = objds.GetNote(new ObjectId(id));
            if (note == null)
            {
                return NotFound();
            }
            return new ObjectResult(note);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Note p)
        {
            objds.Create(p);
            return new OkObjectResult(p);
        }
        [HttpPut("{id:length(24)}")]
        public IActionResult Put(string id, [FromBody]Note p)
        {
            var recId = new ObjectId(id);
            var note = objds.GetNote(recId);
            if (note == null)
            {
                return NotFound();
            }

            objds.Update(recId, p);
            return new OkResult();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var note = objds.GetNote(new ObjectId(id));
            if (note == null)
            {
                return NotFound();
            }

            objds.Remove(note.Id);
            return new OkResult();
        }

        //private IServices _noteService;
        //public NotesController(IServices noteService)
        //{
        //    _noteService = noteService;
        //}

        //// GET: api/Notes
        //[HttpGet]
        //public async Task<ActionResult> GetNote([FromQuery] string title, [FromQuery] bool? pinned, [FromQuery] string label)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var note = await _noteService.GetByQuery(pinned, title, label);

        //    if (note.Count ==0) 
        //    {
        //        return NotFound();
        //    }
        //    return Ok(note);
        //}

        //// GET: api/Notes/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetNote([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var note = await _noteService.Get(id);

        //    if (note == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(note);
        //}


        //// PUT: api/Notes/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != note.Id)
        //    {
        //        return NotFound();
        //    }

        //    bool flag = await _noteService.Update(id, note);
        //    if(flag)
        //        return CreatedAtAction("GetToDo", new { id = note.Id }, note);
        //    else
        //    return NoContent();
        //}

        //// POST: api/Notes
        //[HttpPost]
        //public async Task<IActionResult> PostNote([FromBody] Note note)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var todo = await _noteService.Add(note);

        //    return CreatedAtAction("GetNote", new { id = todo.Id }, todo);
        //}

        //// DELETE: api/Notes/
        //[HttpDelete]
        //[Route("all")]
        //public async Task<IActionResult> DeleteNote()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    await _noteService.DeleteAll();

        //    return NoContent();
        //}

        //// DELETE: api/Notes/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteNote([FromRoute] int id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    await _noteService.Delete(id);
        //    return Ok();
        //}
    }
}