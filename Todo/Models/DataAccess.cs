using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Models
{
    public class DataAccess
    {
        MongoClient _client;
        MongoServer _server;
        MongoDatabase _db;

        public DataAccess()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _server = _client.GetServer();
            _db = _server.GetDatabase("ToDoNotes");
        }

        public IEnumerable<Note> GetNotes()
        {
            return _db.GetCollection<Note>("Notes").FindAll();
        }


        public Note GetNote(ObjectId id)
        {
            var result = Query<Note>.EQ(p => p.Id, id);
            return _db.GetCollection<Note>("Notes").FindOne(result);
        }

        public Note Create(Note note)
        {
            _db.GetCollection<Note>("Notes").Save(note);
            return note;
        }

        public void Update(ObjectId id, Note p)
        {
            p.Id = id;
            var res = Query<Note>.EQ(pd => pd.Id, id);
            var operation = Update<Note>.Replace(p);
            _db.GetCollection<Note>("Notes").Update(res, operation);
        }
        public void Remove(ObjectId id)
        {
            var res = Query<Note>.EQ(e => e.Id, id);
            var operation = _db.GetCollection<Note>("Notes").Remove(res);
        }
    }
}
