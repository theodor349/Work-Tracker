using Microsoft.AspNetCore.Components;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Pages
{
    public partial class StartStopWorkPage : ComponentBase
    {
        [Inject]
        public HttpClient _client { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public List<EmployerModelDTO>? Employers;

        protected override async Task OnInitializedAsync()
        {
            var res = await _client.GetFromJsonAsync<List<EmployerModel>>("api/Employer");
            Employers = res.ConvertAll(x => new EmployerModelDTO()
            {
                Name = x.Name,
                StartTime = DateTime.Now.AddHours(-1),
                TimeThisMonth = new TimeSpan(20,16,0),
            });

            Employers.Add(new EmployerModelDTO()
            {
                Name = "Some other corporation",
                StartTime = null,
                TimeThisMonth = new TimeSpan(2, 46, 12),
            });
        }

        private void ResetDate()
        {
            Date = DateTime.Now;
            Console.WriteLine("Reset Date");
        }
    }

    public class EmployerModelDTO
    {
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public TimeSpan TimeThisMonth { get; set; }
        public bool IsStarted => StartTime is not null;
    }
}
