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

        private IServices _noteService;
        public NotesController(IServices noteService)
        {
            _noteService = noteService;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult> GetNote([FromQuery] string title, [FromQuery] bool pinned, [FromQuery] string label)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Label label1 = new Label()
            {
                TagName = label
            };
            Note note = new Note()
            {
                Labels = new List<Label> { label1 },
                Pinned = pinned,
                Title = title
            };
            var note1 = await _noteService.GetByQuery(note);

            if (note1.Count == 0)
            {
                return NotFound();
            }
            return Ok(note);
        }

        // GET: api/Notes/5
        [HttpGet("{NoteId}")]
        public async Task<IActionResult> GetNote([FromRoute] int NoteId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _noteService.Get(NoteId);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }


        // PUT: api/Notes/5
        [HttpPut("{NoteId}")]
        public async Task<IActionResult> PutNote([FromRoute] int NoteId, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (NoteId != note.NoteId)
            {
                return NotFound();
            }
            try
            {
                var n = await _noteService.Update(NoteId, note);
            }
            catch (DbUpdateConcurrencyException)
            {
                    return NotFound();               
            }

            return Ok(note);
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _noteService.Add(note);

            return CreatedAtAction("GetNote", new { id = todo.Id }, todo);
        }

        // DELETE: api/Notes/
        [HttpDelete]
        [Route("all")]
        public async Task<IActionResult> DeleteNote()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _noteService.DeleteAllAsync();

            return NoContent();
        }

        // DELETE: api/Notes/5
        [HttpDelete("{NoteId}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int NoteId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _noteService.DeleteAsync(NoteId);
            return Ok();
        }
    }
}