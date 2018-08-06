using System;
using Xunit;
using Todo;
using Todo.Models;
using NSuperTest;

namespace Todo.tests
{
    public class TodoTests
    {
        static Server server;
        object product = new
        {
            title = "Acme Better Thing",
            text = "abcd",
            pinned = true
        };

        object shares = new
        {
            id = 30009,
            title = "do a thing",
            text = "next thing",
            pinned = true
        };

        [Fact]
        public static void Init()
        {
            server = new Server("https://localhost:44330/api/Notes/");
        }

        [Fact]
        public void TestNoteCreate()
        {
            server
            .Post("")
            .Send(product)
            .Expect(201)
            .End();
        }

        //[Fact]
        //public void TestNotePut()
        //{
        //    server
        //    .Put("30009")
        //    .Send(shares)
        //    .Expect(204)
        //    .End();
        //}

        //[Fact]
        //public void TestNoteDelete()
        //{
        //    server
        //    .Delete("16006")
        //    .Expect(200)
        //    .End();
        //}

        [Fact]
        public void TestGetNotFoundTitle()
        {
            server.Get("?title=Arjun")
            .Expect(404)
            .End();
        }

        [Fact]
        public void GetTestTitleFound()
        {
            server.Get("?title=syed")
            .Expect(200)
            .End();
        }

        [Fact]
        public void GetTestByPinnedPass()
        {
            server.Get("?pinned=true")
            .Expect(200)
            .End();
        }

        [Fact]
        public void GetTestByPinnedFail()
        {
            server.Get("?pinned=false")
            .Expect(404)
            .End();
        }

        [Fact]
        public void GetTestByLabelsPass()
        {
            server.Get("?label=Personal")
            .Expect(200)
            .End();
        }

        [Fact]
        public void GetTestByLabelsFail()
        {
            server.Get("?label=Perso")
            .Expect(404)
            .End();
        }

        [Fact]
        public void GetAll()
        {
            server.Get("")
            .Expect(200)
            .End();
        }

        [Fact]
        public void GetTestByTitleLablePinned()
        {
            server.Get("?pinned=true&&label=Personal&&title=syed")
            .Expect(200)
            .End();
        }
    }
}
