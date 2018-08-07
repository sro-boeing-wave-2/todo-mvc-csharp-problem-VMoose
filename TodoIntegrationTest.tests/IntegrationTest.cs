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

namespace TodoIntegrationTest.tests
{
    public class IntegrationTest
    {
        private readonly HttpClient _client;

        public IntegrationTest()
        {
            // Arrange
            var host = new TestServer(new WebHostBuilder().UseEnvironment("Testing")
                .UseStartup<Startup>());
            _client = host.CreateClient();
        }

        [Fact]
        public async Task IntegrationTestGetAllNotes()
        {
            //Act
            var response = await _client.GetAsync("/api/Notes");

            //Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            var notes = JsonConvert.DeserializeObject<List<Note>>(responseString);
            Console.WriteLine(notes);

            notes.Count().Should().Be(0);
            Console.WriteLine(notes.Count);
        }
    }
}
