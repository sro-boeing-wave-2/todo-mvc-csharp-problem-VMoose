using System;
using Xunit;
using Todo.Models;
using Todo.Controllers;
using Todo.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace TodoUnitTest.tests
{
    public class TodobackendTest
    {
        private readonly NotesController _controller;

        public TodobackendTest()
        {
            var optionBuilder = new DbContextOptionsBuilder<TodoContext>();
            optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var todoContext = new TodoContext(optionBuilder.Options);
            _controller = new NotesController(new Service(todoContext));
            CreateData(todoContext);
        }

        public void CreateData(TodoContext todoContext)
        {
            var note = new List<Note>() {
                new Note
            {
                Title = "first",
                Text = "Ft",
                Labels = new List<Label>
                {
                    new Label { TagName = "black"},
                    new Label { TagName = "green"}
                },
                Checklist = new List<Checklist>
                {
                    new Checklist { list = "redbull"},
                    new Checklist { list = "pepsi"}
                },
                Pinned = true
            },
                new Note
            {
                Title = "second",
                Text = "st",
                Labels = new List<Label>
                {
                    new Label { TagName = "blue"},
                    new Label { TagName = "white"}
                },
                Checklist = new List<Checklist>
                {
                    new Checklist { list = "coke"},
                    new Checklist { list = "pepsi"}
                },
                Pinned = false
            }
            };
            todoContext.Note.AddRange(note);
            todoContext.SaveChanges();
        }

        [Fact]
        public async void TestGetAll()
        {
            var result = await _controller.GetNote("first", true, "black");
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Single(notes);
        }

        [Fact]
        public async void TestGetByTitle()
        {
            var result = await _controller.GetNote("first", null, null);
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Single(notes);
        }

        [Fact]
        public async void TestGetByPinned()
        {
            var result = await _controller.GetNote(null, true, null);
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Single(notes);
        }

        [Fact]
        public async void TestGetByLabel()
        {
            var result = await _controller.GetNote(null, null, "blue");
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Single(notes);
        }

        [Fact]
        public async void TestGetByLabelPinned()
        {
            var result = await _controller.GetNote(null, false, "blue");
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Single(notes);
        }

        [Fact]
        public async void TestGetByTitlePinned()
        {
            var result = await _controller.GetNote("second", null, "blue");
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Single(notes);
        }

        [Fact]
        public async void TestGetByTitleLabel()
        {
            var result = await _controller.GetNote("first", null, "black");
            var okObjectResult = result as OkObjectResult;
            var notes = okObjectResult.Value as List<Note>;
            Assert.Single(notes);
        }

        [Fact]
        public async void TestPost()
        {
            var note = new Note
            {
                Title = "third",
                Text = "tt",
                Labels = new List<Label>
                {
                    new Label { TagName = "orange"},
                    new Label { TagName = "grey"}
                },
                Checklist = new List<Checklist>
                {
                    new Checklist { list = "sprite"},
                    new Checklist { list = "backy"}
                },
                Pinned = true
            };
            var result = await _controller.PostNote(note);
            var okObjectResult = result as CreatedAtActionResult;
            var notes = okObjectResult.Value as Note;
            Assert.Equal(note,notes);
        }

        [Fact]
        public async void TestPut()
        {
            var note = new Note
            {
                Id = 1,
                Title = "third",
                Text = "tt",
                Labels = new List<Label>
                {
                    new Label { TagName = "red"},
                    new Label { TagName = "pink"}
                },
                Checklist = new List<Checklist>
                {
                    new Checklist { list = "fanta"},
                    new Checklist { list = "redlabel"}
                },
                Pinned = true
            };
            var result = await _controller.PutNote(1, note);
            var okObjectResult = result as NoContentResult;
            var notes = okObjectResult.StatusCode;
            Assert.Equal(204,notes);
        }

        [Fact]
        public async void TestPDelete()
        {
            var result = await _controller.DeleteNote(1);
            var okObjectResult = result as OkResult;
            var notes = okObjectResult.StatusCode;
            Assert.Equal(200,notes);
        }
    }
}
