using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Todo.Models
{
    public class Checklist
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string list { get; set; }
    }
}
