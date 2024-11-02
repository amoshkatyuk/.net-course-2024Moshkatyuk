using AutoMapper;
using BankSystem.App.Dto;
using BankSystem.App.Interfaces;
using BankSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BankSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

        public ClientController(IClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet("by guid")]
        public async Task<IActionResult> GetClient([FromQuery] Guid clientId, CancellationToken cancellationToken) 
        {
            var client = await _clientService.GetClientByIdAsync(clientId, cancellationToken);

            if (client == null) 
            {
                return NotFound();
            }

            var clientDto = _mapper.Map<ClientDto>(client);
            
            return Ok(clientDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] ClientDto clientDto, CancellationToken cancellationToken) 
        {
            var client = _mapper.Map<Client>(clientDto);

            await _clientService.AddClientAsync(client, cancellationToken);

            return Ok(client);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClient(Guid clientId, [FromBody] ClientDto clientDto, CancellationToken cancellationToken) 
        {
            if (clientDto == null) 
            {
                return BadRequest("Данные для обновления сущности не были предоставлены");
            }

            var existingClient = await _clientService.GetClientByIdAsync(clientId, cancellationToken);

            _mapper.Map(clientDto, existingClient);

            await _clientService.UpdateClientAsync(existingClient, cancellationToken);

            return Ok(existingClient);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteClient([FromQuery] Guid clientId, CancellationToken cancellationToken) 
        {
            var client = await _clientService.GetClientByIdAsync(clientId, cancellationToken);

            if (client == null)
            {
                return NotFound();
            }

            await _clientService.DeleteClientAsync(clientId, cancellationToken);

            return Ok();
        }

        [HttpGet("by filter")]
        public async Task<ActionResult<List<ClientDto>>> SearchClients(
            CancellationToken cancellationToken,
            [FromQuery] string name = null,
            [FromQuery] string surname = null,
            [FromQuery] string passportData = null,
            [FromQuery] DateTimeOffset? birthDate = null,
            [FromQuery] string telephoneNumber = null)
        {
            Expression<Func<Client, bool>> filter = c =>
            (string.IsNullOrEmpty(name) || c.Name.Contains(name)) &&
            (string.IsNullOrEmpty(surname) || c.Surname.Contains(surname)) &&
            (string.IsNullOrEmpty(passportData) || c.PassportData.Contains(passportData)) &&
            (!birthDate.HasValue || c.BirthDate == birthDate) &&
            (string.IsNullOrEmpty(telephoneNumber) || c.TelephoneNumber.Contains(telephoneNumber));

            var filteredClients = await _clientService.FilterClientsAsync(filter, cancellationToken);

            var filteredClientDtos = _mapper.Map<List<ClientDto>>(filteredClients);
            
            return Ok(filteredClientDtos);
        }
    }
}
