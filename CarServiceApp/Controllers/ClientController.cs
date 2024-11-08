using CarServiceApp.Domain.Entity.Models;
using CarServiceApp.Domain.Extension;
using CarServiceApp.Domain.Service.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CarServiceApp.Controllers
{
    public class ClientController(
        IClientService clientService,
        IPreRecordService preRecordService) : Controller
    {
        private readonly IClientService _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        private readonly IPreRecordService _preRecordService = preRecordService ?? throw new ArgumentNullException(nameof(preRecordService));
        private const int PageSize = 10;
        private const int TakeValue15 = 15;

        public ActionResult List()
        {
            return View();
        }

        public async Task<ActionResult> AutocompleteSearch(string term)
        {
            var isContainsDigit = false;
            foreach (var ch in term.ToCharArray())
            {
                if (char.IsDigit(ch))
                    isContainsDigit = true;
            }
            if (isContainsDigit)
            {
                var nameClient = await _clientService.GetByIdAsync(term, TakeValue15);

                return Json(nameClient);
            }
            else
            {
                var nameClient = await _clientService.GetByNameAsync(term, TakeValue15);

                return Json(nameClient);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ClientSearch(string name, string id, int? page)
        {
            var clients = await _clientService.GetAsync(id, name, page ?? 1, PageSize);

            if (clients.TotalItems == 0)
            {
                return NotFound();
            }

            return PartialView(clients);
        }

        public async Task<ActionResult> CreateNewClient(long? recordId)
        {
            var record = await _preRecordService.GetPreRecordByIdAsync(recordId ?? 0);
            if (record != null)
            {
                ViewData.Add("idRecord", record.RecordId);
                var model = new Client { FullName = record.FullName, Phone = record.PhoneNumber };

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateNewClient(Client client, long? recordId)
        {
            if (ModelState.IsValid)
            {
                // var clientId = new ObjectParameter("id_client", typeof(string));
                string id = await _clientService.CreateAsync(client);
                //autoServiceDb.AddClient(client.FullName, client.NumberDriveLicense,
                //client.Phone, client.ExpirationDateLicense, client.DateBirth, client.Adress, clientId);

                if (string.IsNullOrWhiteSpace(id))
                    return View(client);

                return recordId != null
                    ? RedirectToAction("AddOrder", "Order", new { Id = id, recordId })
                    : RedirectToAction("List", "Client");
            }

            return View(client);
        }

        [HttpGet]
        public async Task<ActionResult> ShowClientDetail(string clientId)
        {
            if (clientId != null)
            {
                var client = await _clientService.GetAsync(clientId);
                ViewBag.ShortName = client?.FullName?.ConvertToShortName();

                if (client != null)
                    return View(client);

                return NotFound();
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult> EditClient(string clientId)
        {
            if (clientId != null)
            {
                var client = await _clientService.GetAsync(clientId);
                if (client == null)
                    return NotFound();

                ViewBag.ShortName = client?.FullName?.ConvertToShortName();

                return View(client);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> EditClient(Client client)
        {
            if (ModelState.IsValid)
            {
                var existingClient = await _clientService.GetAsync(client.ClientId);
                if (existingClient != null)
                {
                    await _clientService.UpdateAsync(client);

                    return RedirectToAction("ShowClientDetail", new { clientId = client.ClientId });
                }
            }

            return View(client);
        }

        public async Task<ActionResult> Delete(string clientId)
        {
            if (clientId != null)
            {
                var client = await _clientService.GetAsync(clientId);

                if (client != null)
                {
                    await _clientService.DeleteAsync(client);

                    return RedirectToAction("Index", "Home");
                }
            }

            return NotFound();
        }
    }
}