using ApiAccess.Models;
using Shared.Models;
using Shared.Models.DTOs.WorkEntry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiAccess.Services
{
    public interface IWorkTrackerApiService
    {
        IEmployerService Employers { get; }
        IWorkEntryService WorkEntries { get; }
    }

    public class WorkTrackerApiService : IWorkTrackerApiService
    {
        public IWorkEntryService WorkEntries { get; }
        public IEmployerService Employers { get; }

        public WorkTrackerApiService(HttpClient client)
        {
            WorkEntries = new WorkEntryService(client, this);
            Employers = new EmployerService(client, this);
        }
    }

    public interface IEmployerService
    {
        Task<List<EmployerDisplayModel>> GetEmployersAsync();
    }
    public class EmployerService : IEmployerService
    {
        private readonly HttpClient _client;
        private readonly IWorkTrackerApiService _api;

        public EmployerService(HttpClient client, IWorkTrackerApiService api)
        {
            _client = client;
            _api = api;
        }

        public async Task<List<EmployerDisplayModel>> GetEmployersAsync()
        {
            var employers = await _client.GetFromJsonAsync<List<EmployerModel>>("api/Employer");
            var res = new List<EmployerDisplayModel>();
            foreach (var model in employers)
            {
                var displayModel = await ConvertToDisplayModel(model);
                res.Add(displayModel);
            }
            return res;
        }

        private async Task<EmployerDisplayModel> ConvertToDisplayModel(EmployerModel model)
        {
            var displayModel = new EmployerDisplayModel();
            displayModel.Name = model.Name;
            displayModel.Id = model.Id;
            await AddStartTime(model, displayModel);
            await AddTimeThisMonth(model, displayModel);
            return displayModel;
        }

        private async Task AddStartTime(EmployerModel model, EmployerDisplayModel displayModel)
        {
            var latest = await _api.WorkEntries.GetLatests(model.Id);
            if (latest != null && latest.EndTime == null)
                displayModel.StartTime = latest.StartTime;
        }

        private async Task AddTimeThisMonth(EmployerModel model, EmployerDisplayModel displayModel)
        {
            var currentTime = DateTime.Now;
            var startDate = new DateTime(currentTime.Year, currentTime.Month, 1);
            var endDate = new DateTime(currentTime.Year, currentTime.Month + 1, 1).AddSeconds(-1);
            displayModel.TimeThisMonth = await _api.WorkEntries.GetTimeBetweenAsync(model.Id, startDate, endDate);
        }
    }

    public interface IWorkEntryService
    {
        Task<WorkEntryModel> EndLatestAsync(EndLatestWorkEntryDto model);
        Task<WorkEntryModel> GetLatests(Guid employerId);
        Task<TimeSpan> GetTimeBetweenAsync(Guid employerId, DateTime start, DateTime end);
        Task<WorkEntryModel> StartAsync(StartWorkEntryDto model);
    }
    public class WorkEntryService : IWorkEntryService
    {
        private readonly HttpClient _client;
        private readonly IWorkTrackerApiService _api;

        public WorkEntryService(HttpClient client, IWorkTrackerApiService api)
        {
            _client = client;
            _api = api;
        }

        public async Task<WorkEntryModel> GetLatests(Guid employerId)
        {
            return await _client.GetFromJsonAsync<WorkEntryModel>("api/WorkEntry/" + employerId + "/Latests");
        }

        public async Task<TimeSpan> GetTimeBetweenAsync(Guid employerId, DateTime start, DateTime end)
        {
            return await _client.GetFromJsonAsync<TimeSpan>("api/WorkEntry/" + employerId + "/TimeBetween?start=" + start + "&end=" + end);
        }

        public async Task<WorkEntryModel> StartAsync(StartWorkEntryDto model)
        {
            var res = await _client.PostAsJsonAsync("api/WorkEntry/Start", model);
            if (res.IsSuccessStatusCode)
                return await res.Content.ReadFromJsonAsync<WorkEntryModel>();
            else
                throw new HttpRequestException("Unable to Start the session");
        }

        public async Task<WorkEntryModel> EndLatestAsync(EndLatestWorkEntryDto model)
        {
            var res = await _client.PostAsJsonAsync("api/WorkEntry/EndLatest", model);
            if (res.IsSuccessStatusCode)
                return await res.Content.ReadFromJsonAsync<WorkEntryModel>();
            else
                throw new HttpRequestException("Unable to End the session");
        }
    }
}
