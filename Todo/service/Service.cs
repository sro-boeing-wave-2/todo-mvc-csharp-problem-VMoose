using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Models;

namespace Todo.service
{
    public class Service : IServices
    {
        private readonly TodoContext _context;

        public Service(TodoContext context)
        {
            _context = context;
        }

        public async Task<Note> Add(Note note)
        {
            _context.Note.Add(note);
            _context.SaveChanges();
            return await Task.FromResult(note);
        }

        public Task Delete(int id)
        {
            var note = _context.Note.Include(n => n.Labels).Include(n => n.Checklist).First(_ => _.Id == id);
            _context.Note.Remove(note);
            _context.SaveChanges();
            return Task.CompletedTask;

        }

        public Task DeleteAll()
        {
            var note = _context.Note.Include(n => n.Labels).Include(n => n.Checklist);
            _context.Note.RemoveRange(note);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public async Task<Note> Get(int id)
        {
            var note = await _context.Note.Include(n => n.Labels).Include(n => n.Checklist).SingleOrDefaultAsync(x => x.Id == id);
            return await Task.FromResult(note);
        }

        public async Task<List<Note>> GetByQuery(bool? pinned = null, string title = "", string label = "")
        {
            var note = await _context.Note.Include(x => x.Checklist).Include(x => x.Labels).Where(
            m => ((title == null) || (m.Title == title)) && ((label == null) || (m.Labels).Any(b => b.TagName == label)) && ((!pinned.HasValue) || (m.Pinned == pinned))).ToListAsync();
            return await Task.FromResult(note);
        }

        public async Task<bool> Update(int id, Note note)
        {
            bool flag = false;
            await _context.Note.Include(x => x.Checklist).Include(x => x.Labels).ForEachAsync(element =>
            {
                if (element.Id == note.Id)
                {
                    flag = true;
                    element.Text = note.Text;
                    element.Pinned = note.Pinned;
                    element.Title = note.Title;
                    _context.Lable.RemoveRange(element.Labels);
                    element.Labels.AddRange(note.Labels);
                    _context.Checklist.RemoveRange(element.Checklist);
                    element.Checklist.AddRange(note.Checklist);
                }
            });
            if (flag)
                await _context.SaveChangesAsync();
            return flag;
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}
