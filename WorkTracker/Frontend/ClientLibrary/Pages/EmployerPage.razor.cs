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
    public partial class EmployerPage : ComponentBase
    {
        [Inject]
        public IWorkTrackerApiService _api { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        public List<EmployerDisplayModel> Employers { get; set; } = new List<EmployerDisplayModel>();
        public string NewEmployerName { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            Employers = await _api.Employers.GetDisplayModelsAsync();
        }

        public async Task CreateNewEmployer(string newName)
        {
            StateHasChanged();
            await _api.Employers.CreateEmployer(newName);
        }

        public async Task DeleteEmployer(Guid id)
        {
            StateHasChanged();
            await _api.Employers.DeleteEmployer(id);
        }

    }
}
