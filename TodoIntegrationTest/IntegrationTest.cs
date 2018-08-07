using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Todo;
using Todo.Models;
using Xunit;
using System.Net;

namespace TodoIntegrationTest.tests
{
    public class IntegrationTest
    {
        private readonly HttpClient _client;
        private TodoContext _context;

        public IntegrationTest()
        {
            // Arrange
            var host = new TestServer(new WebHostBuilder().UseEnvironment("Testing")
                .UseStartup<Startup>());
            _context = host.Host.Services.GetService(typeof(TodoContext)) as TodoContext;
            _client = host.CreateClient();
            _context.Note.AddRange(TestNote1);
            _context.Note.AddRange(TestNote2);
            _context.Note.AddRange(TestNoteDelete);
            _context.SaveChanges();
        }

        Note TestNote1 = new Note()
        {
            Title = "Title-1-Updatable",
            Text = "Message-1-Updatable",
            Checklist = new List<Checklist>()
                        {
                            new Checklist(){list = "checklist-1"},
                            new Checklist(){list = "checklist-2"}
                        },
            Labels = new List<Label>()
                        {
                            new Label(){TagName = "Label-5-Deletable"},
                            new Label(){TagName = "Label-2-Updatable"}
                        },
            Pinned = true
        };
        Note TestNote2 = new Note()
        {
            Title = "Title-2-Updatable",
            Text = "Message-2-Updatable",
            Checklist = new List<Checklist>()
                        {
                            new Checklist(){list = "checklist-1"},
                            new Checklist(){list = "checklist-2"}
                        },
            Labels = new List<Label>()
                        {
                            new Label(){TagName = "Label-1-Deletable"},
                            new Label(){TagName = "Label-2-Updatable"}
                        },
            Pinned = true
        };
        Note TestNote3 = new Note
        {
            Id = 16,
            Title = "Title-3-Deletable",
            Text = "Message-3-Deletable",
            Checklist = new List<Checklist>()
                        {
                            new Checklist(){ list = "checklist-1"},
                            new Checklist(){ list = "checklist-2"}
                        },
            Labels = new List<Label>()
                        {
                            new Label(){TagName = "Label-1-Deletable"},
                            new Label(){TagName = "Label-2-Deletable"}
                        },
            Pinned = false
        };
        Note TestNotePost = new Note
        {
            Id = 5,
            Title = "Title-2-Deletable",
            Text = "Message-2-Deletable",
            Checklist = new List<Checklist>()
                        {
                            new Checklist(){ list = "checklist-2-1"},
                            new Checklist(){ list = "checklist-2-2"}
                        },
            Labels = new List<Label>()
                        {
                            new Label(){TagName = "Label-2-1-Deletable"},
                            new Label(){ TagName = "Label-2-2-Deletable"}
                        },
            Pinned = false
        };
        Note TestNoteDelete = new Note()
        {
            Title = "this is deleted title",
            Text = "some text",
            Pinned = false,
            Labels = new List<Label>
               {
                   new Label{ TagName="My First Tag" },
                   new Label{ TagName = "My second Tag" },
                   new Label{ TagName = "My third Tag" },
               },
            Checklist = new List<Checklist>
               {
               new Checklist{list="first item"},
               new Checklist{list="second item"},
               new Checklist{list="third item"},
               }
        };

        Note TestNotePut = new Note()
        {
            Id =1,
            Title = "this is replaced title",
            Text = "some text",
            Pinned = false,
            Labels = new List<Label>
               {
                   new Label{ TagName="My First Tag" },
                   new Label{ TagName = "My second Tag" },
                   new Label{ TagName = "My third Tag" },
               },
            Checklist = new List<Checklist>
               {
               new Checklist{list="first item"},
               new Checklist{list="second item"},
               new Checklist{list="third item"},
               }
        };
        [Fact]
        public async Task IntegrationTestGetAllNotes()
        {
            var response = await _client.GetAsync("/api/Notes/");
            var responsestring = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<List<Note>>(responsestring);
            Assert.Equal(3, responsenote.Count);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void IntegrationTestTestGetByTitle()
        {
            var response = await _client.GetAsync("/api/Notes?title=Title-1-Updatable");
            var responsestring = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<List<Note>>(responsestring);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(TestNote1.Title, responsenote[0].Title);
        }

        [Fact]
        public async void IntegrationTestGetById()
        {
            var response = await _client.GetAsync("/api/Notes/1");
            var responsestring = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<Note>(responsestring);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(TestNote1.Id, responsenote.Id);
        }

        [Fact]
        public async void IntegrationTestGetByPinned()
        {
            var response = await _client.GetAsync("/api/Notes?Pinned=false");
            var responsestring = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<List<Note>>(responsestring);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(TestNote3.Pinned, responsenote[0].Pinned);
        }

        [Fact]
        public async void IntegrationTestGetByLabel()
        {
            var response = await _client.GetAsync("/api/Notes?label=Label-5-Deletable");
            var responsestring = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<List<Note>>(responsestring);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(TestNote1.Labels.Count, responsenote[0].Labels.Count);
        }

        [Fact]
        public async Task IntegrationTestPostNote()
        {
            var json = JsonConvert.SerializeObject(TestNotePost);
            var stringcontent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Notes", stringcontent);
            var responsecontent = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<Note>(responsecontent);
            var responsedata = response.StatusCode;
            Assert.Equal(HttpStatusCode.Created, responsedata);
            Assert.Equal(TestNotePost.Title, responsenote.Title);
        }

        [Fact]
        public async Task IntegrationTestPutNote()
        {
            var json = JsonConvert.SerializeObject(TestNote3);
            var stringcontent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/Notes/16", stringcontent);
            var responsecontent = await response.Content.ReadAsStringAsync();
            var responsenote = JsonConvert.DeserializeObject<Note>(responsecontent);
            var responsedata = response.StatusCode;
            Assert.Equal(HttpStatusCode.NoContent, responsedata);
        }

        [Fact]
        public async void IntegrationTestDelete()
        {
            var response = await _client.DeleteAsync("api/Notes/1");
            var responsecode = response.StatusCode;
            Assert.Equal(HttpStatusCode.OK, responsecode);
        }
    }
}
