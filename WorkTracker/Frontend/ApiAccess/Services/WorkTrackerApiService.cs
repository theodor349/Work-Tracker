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
        IInvoiceService Invoices { get; }
    }

    public class WorkTrackerApiService : IWorkTrackerApiService
    {
        public IWorkEntryService WorkEntries { get; }
        public IEmployerService Employers { get; }
        public IInvoiceService Invoices { get; }

        public WorkTrackerApiService(HttpClient client)
        {
            WorkEntries = new WorkEntryService(client, this);
            Employers = new EmployerService(client, this);
            Invoices = new InvoiceService(client, this);
        }
    }


    public interface IInvoiceService
    {
        Task<string> GetAauInvoiceAsync(CreateInvoiceModel model);
    }
    public class InvoiceService : IInvoiceService
    {
        private readonly HttpClient _client;
        private readonly IWorkTrackerApiService _api;

        public InvoiceService(HttpClient client, IWorkTrackerApiService api)
        {
            _client = client;
            _api = api;
        }

        public async Task<string> GetAauInvoiceAsync(CreateInvoiceModel model)
        {
            var start = new DateTime(model.Year, model.Month, 1);
            var end = start.AddMonths(1).AddSeconds(-1);
            var response = await _client.GetAsync("api/Invoice/" + model.EmployerId + "/AauStudentProgrammerTimeSheet" +
                "?startDate=" + start +
                "&endDate=" + end +
                "&maxMonthlyHours=" + (model.MaxMonthlyHours + (double)model.MaxMonthlyMinutes / 60) +
                "&extraHours=" + (model.ExtraHours + (double)model.ExtraMinutes / 60) +
                "&ShouldAddInvoice=" + model.ShouldAddInvoice
            );
            var file = await response.Content.ReadAsStringAsync();
            return file;
        }
    }

    public interface IEmployerService
    {
        Task<EmployerModel> CreateEmployer(string employerName);
        Task DeleteEmployer(Guid employerId);
        Task<List<EmployerModel>> GetAllAsync();
        Task<EmployerBalanace> GetBalanceAsync(Guid id, DateTime beforeDate);
        Task<List<EmployerDisplayModel>> GetDisplayModelsAsync();
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

        public async Task DeleteEmployer(Guid employerId)
        {
            var response = await _client.DeleteAsync("api/Employer/" + employerId);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException("Unable to delete Employer");
        }

        public async Task<EmployerModel> CreateEmployer(string employerName)
        {
            var response = await _client.PostAsJsonAsync("api/Employer", employerName);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<EmployerModel>();
            else
                throw new HttpRequestException("Unable to create new Employer");
        }

        public async Task<EmployerBalanace> GetBalanceAsync(Guid id, DateTime beforeDate)
        {
            return await _client.GetFromJsonAsync<EmployerBalanace>("api/Employer/" + id + "/Balance?beforedate=" + beforeDate);
        }

        public async Task<List<EmployerModel>> GetAllAsync()
        {
            return await _client.GetFromJsonAsync<List<EmployerModel>>("api/Employer");
        }

        public async Task<List<EmployerDisplayModel>> GetDisplayModelsAsync()
        {
            var employers = await GetAllAsync();
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
            displayModel.Balance = await GetBalanceAsync(model.Id, DateTime.Now.AddMonths(1));
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
        Task<WorkEntryModel?> GetLatests(Guid employerId);
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

        public async Task<WorkEntryModel?> GetLatests(Guid employerId)
        {
            var response = await _client.GetAsync("api/WorkEntry/" + employerId + "/Latests");
            if(response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return null;
                else 
                    return await response.Content.ReadFromJsonAsync<WorkEntryModel>();
            }
            return await _client.GetFromJsonAsync<WorkEntryModel?>("api/WorkEntry/" + employerId + "/Latests");
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
