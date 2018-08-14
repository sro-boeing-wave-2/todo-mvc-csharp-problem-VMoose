using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Models;

namespace Todo.service
{
    public interface IServices
    {
        Task<Note> Get(int NoteId);
        Task<List<Note>> GetByQuery(Note note);
        Task<Note> Add(Note note);
        Task<Note> Update(int NoteId, Note note);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAllAsync();

    }
}
