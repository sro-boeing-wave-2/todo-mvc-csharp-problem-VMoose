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
        Task<Note> Get(ObjectId id);
        Task<List<Note>> GetByQuery(bool? pinned = null, string title = "", string label = "");
        Task<Note> Add(Note note);
        Task<bool> Update(ObjectId id, Note note);
        Task Delete(ObjectId id);
        Task DeleteAll();

    }
}
