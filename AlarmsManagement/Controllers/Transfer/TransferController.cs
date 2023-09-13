using System.Text;
using Core.Entities.Journals.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AlarmsManagement.Controllers.Transfer;

[Route("api/[controller]")]
[ApiController]
public class TransferController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IJournalServices _journalServices;

    public TransferController(IJournalServices journalServices)
    {
        _httpClient = new HttpClient();
        _journalServices = journalServices;
    }


    [HttpPost("PushDataToApi2Async")]
    public async Task<IActionResult> PushDataToApi2Async()
    {
        try
        {
            var api2Url = "https://localhost:7207/api/Receive/endpoint";

            var journals = await _journalServices.GetAllJournal();

            var jsonData = JsonConvert.SerializeObject(journals);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(api2Url, content);

                if (response.IsSuccessStatusCode)
                {
                    return Ok(true); // Les données ont été envoyées avec succès à l'API 2
                }

                // Gérer le cas où la requête a échoué
                var errorMessage = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorMessage);
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