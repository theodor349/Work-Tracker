using ApiAccess.Models;
using ApiAccess.Services;
using Microsoft.AspNetCore.Components;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Pages
{
    public partial class InvoicePage : ComponentBase
    {
        [Inject]
        public IWorkTrackerApiService _api { get; set; }
        private CreateInvoiceModel request;

        private bool initialized = false;
        private List<EmployerDisplayModel>? employers;
        private List<int> years = new List<int>();
        private List<int> months = new List<int>() { 1,2,3,4,5,6,7,8,9,10,11,12 };
        private EmployerBalanace? employerBalance;
        private EmployerDisplayModel? selectedEmployer => employers.First(x => x.Id == request.EmployerId);

        private string GetDurationString(TimeSpan duration)
        {
            return ((int)duration.TotalHours).ToString() + "H" + duration.Minutes + "m";
        }

        protected override async Task OnInitializedAsync()
        {
            employers = await _api.Employers.GetDisplayModelsAsync();
            await Reset();
            initialized = true;
        }

        private async Task Update()
        {
            employerBalance = await _api.Employers.GetBalanceAsync(request.EmployerId, new DateTime(request.Year, request.Month, 1));
            StateHasChanged();
        }

        private async Task Submit(CreateInvoiceModel model)
        {
            await _api.Invoices.GetAauInvoiceAsync(model);
            await Reset();
        }

        private async Task Reset()
        {
            var curr = DateTime.Now;
            request = new CreateInvoiceModel(employers.First().Id, curr.Year, curr.Month, 34, 24, 0, 0, false);
            await Update();
        }
    }
}
