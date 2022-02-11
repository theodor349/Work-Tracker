using ApiAccess.Models;
using ApiAccess.Services;
using ClientLibrary.PopUps;
using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Models;
using Shared.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Pages
{
    public partial class EmployerPage : ComponentBase, IDisposable
    {
        [Inject]
        public IWorkTrackerApiService _api { get; set; }
        [Inject]
        public DialogService DialogService { get; set; }
        public List<EmployerDisplayModel> Employers { get; set; } = new List<EmployerDisplayModel>();
        public string NewEmployerName { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            DialogService.OnOpen += Open;
            DialogService.OnClose += Close;

            Employers = await _api.Employers.GetDisplayModelsAsync();
        }
        public void Dispose()
        {
            DialogService.OnOpen -= Open;
            DialogService.OnClose -= Close;
        }

        void Open(string title, Type type, Dictionary<string, object> parameters, DialogOptions options)
        {
        }

        void Close(dynamic result)
        {
            StateHasChanged();
        }

        public async Task CreateNewEmployer(string newName)
        {
            StateHasChanged();
            await _api.Employers.CreateEmployer(newName);
        }

        public async Task OpenDeleteConfirmation(EmployerDisplayModel model)
        {
            await DialogService.OpenAsync<DeleteEmployerDialog>($"Employer {model}",
               new Dictionary<string, object>() { { "Employer", model } },
               new DialogOptions() { Width = "700px", Height = "570px", Resizable = true, Draggable = true });
        }

    }
}
