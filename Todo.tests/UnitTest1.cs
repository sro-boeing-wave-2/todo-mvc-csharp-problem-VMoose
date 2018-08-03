using System;
using Xunit;
using System.Net.Http;
using System.Web.Http;
using Todo.Controllers;
using Todo.Models;

namespace Todo.tests
{
    public class UnitTest1
    {
        private readonly TodoContext _context;
        [Fact]
        public void Test1()
        {
            NotesController controller = new NotesController(_context);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = newHttpConfiguration();
        }
    }
}
