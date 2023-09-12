using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Models.DTOs;
using Core.Entities.Journals.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Transfert.App.Transfert
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransfertController : ControllerBase
    {


        private readonly HttpClient httpClient;
        private readonly IJournalServices _IJournalServices;

        public TransfertController(IJournalServices IJournalServices)
        {
            httpClient = new HttpClient();
            _IJournalServices = IJournalServices;

        }



        [HttpPost("PushDataToApi2Async")]
        public async Task<IActionResult> PushDataToApi2Async()
        {
            try
            {
                var api2Url = "https://localhost:7207/api/Receive/endpoint";

                var Journals = await _IJournalServices.GetAllJournal();

                string jsonData = JsonConvert.SerializeObject(Journals);

                StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(api2Url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return Ok(true); // Les données ont été envoyées avec succès à l'API 2
                    }
                    else
                    {
                        // Gérer le cas où la requête a échoué
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs d'exception
                Console.WriteLine($"Une erreur s'est produite lors de l'envoi de la requête : {ex.Message}");
                return StatusCode(500, "Erreur interne du serveur");
            }
        }






    }
}
