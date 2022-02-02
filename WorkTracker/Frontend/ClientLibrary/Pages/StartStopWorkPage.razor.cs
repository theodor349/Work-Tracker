using ApiAccess.Models;
using ApiAccess.Services;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using Shared.Models.DTOs.WorkEntry;
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
        public IWorkTrackerApiService _api { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public List<EmployerDisplayModel>? Employers;

        protected override async Task OnInitializedAsync()
        {
            Employers = await _api.Employers.GetDisplayModelsAsync();
        }

        private void ResetDate()
        {
            Date = DateTime.Now;
        }

        private async Task Start(Guid emplouerId)
        {
            var entry = await _api.WorkEntries.StartAsync(new StartWorkEntryDto() { EmployerId = emplouerId, Start = Date });
            Employers.First(x => x.Id == emplouerId).StartTime = entry.StartTime;
            StateHasChanged();
        }

        private async Task EndLatest(Guid emplouerId)
        {
            var entry = await _api.WorkEntries.EndLatestAsync(new EndLatestWorkEntryDto() { EmployerId = emplouerId, End = Date });
            var employer = Employers.First(x => x.Id == emplouerId);
            employer.StartTime = null;
            employer.TimeThisMonth = employer.TimeThisMonth.Add(entry.Duration);
            StateHasChanged();
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
