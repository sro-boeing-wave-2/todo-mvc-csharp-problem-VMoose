using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
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

        public Service(IOptions<settings> settings)
        {
            _context = new TodoContext(settings); ;
        }

        public async Task<Note> Add(Note note)
        {
            
            await _context.Notes.InsertOneAsync(note);
            return await Task.FromResult(note);
        }

        public async Task<bool> DeleteAsync(int Noteid)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Notes.DeleteOneAsync(
                        Builders<Note>.Filter.Eq("NoteId", Noteid));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> DeleteAllAsync()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.Notes.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Note> Get(int noteid)
        {
            try
            {
                return await _context.Notes
                                .Find(note => note.NoteId == noteid)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Note>> GetByQuery(Note n)
        {
            var note = _context.Notes.Find(
            p => p.Title == n.Title || String.IsNullOrEmpty(n.Title)
                && (p.Pinned == n.Pinned || !n.Pinned)
                && (p.Labels.Any(y => y.TagName == n.Labels[0].TagName) || String.IsNullOrEmpty(n.Labels[0].TagName))).ToListAsync();
            return await await Task.FromResult(note);
        }

        public async Task<Note> Update(int NoteId, Note note)
        {
            var filter = Builders<Note>.Filter.Eq(p => p.NoteId, NoteId);
            var update = Builders<Note>.Update.Set(p => p.Title, note.Title)
                .Set(p => p.Text, note.Text)
                .Set(p => p.Labels, note.Labels)
                .Set(p => p.Pinned, note.Pinned)
                .Set(p => p.Checklist, note.Checklist);
            await _context.Notes.UpdateOneAsync(filter, update);
            //await _context.SaveChangesAsync();
            return note;
        }
    }
}
