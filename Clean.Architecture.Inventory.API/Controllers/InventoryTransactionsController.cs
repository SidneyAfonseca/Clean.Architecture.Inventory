using Clean.Architecture.Inventory.Application.Commands;
using Clean.Architecture.Inventory.Application.Queries;
using Clean.Architecture.Inventory.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Architecture.Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryTransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryTransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST: api/InventoryTransactions
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateInventoryTransactionCommand command)
        {
            var transactionId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = transactionId }, transactionId);
        }

        // GET: api/InventoryTransactions/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryTransaction>> GetById(int id)
        {
            var query = new GetInventoryTransactionByIdQuery { Id = id };
            var transaction = await _mediator.Send(query);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        // GET: api/InventoryTransactions/today
        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<InventoryTransaction>>> GetTodayTransactions()
        {
            var query = new GetTodayTransactionsQuery { Date = DateTime.UtcNow.Date };
            var transactions = await _mediator.Send(query);
            return Ok(transactions);
        }
    }
}
