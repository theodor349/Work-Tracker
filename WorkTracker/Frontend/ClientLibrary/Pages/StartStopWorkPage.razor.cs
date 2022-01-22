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
        public List<EmployerModel>? employers;

        protected override async Task OnInitializedAsync()
        {
            employers = await _client.GetFromJsonAsync<List<EmployerModel>>("api/Employer");
        }

    }
}
