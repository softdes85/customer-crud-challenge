using AutoMapper;
using Customers.Management.Api.Dtos;
using Customers.Management.Repository.Entities;
using Customers.Management.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Customers.Management.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {

        private readonly ICustomersService _customerService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomersService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>List of customers</returns>
        /// <response code="200">Returns the list of customers</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResponse<CustomerDto>>> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var totalRecords = await _customerService.GetTotalCountAsync();
            var customers = await _customerService.GetAllCustomersAsync(pageNumber, pageSize);

            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            var paginatedResponse = new PaginatedResponse<CustomerDto>(customerDtos, totalRecords, pageNumber, pageSize);

            return Ok(paginatedResponse);
        }

        /// <summary>
        /// Retrieves a specific customer by unique ID.
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <returns>Customer details</returns>
        /// <response code="200">Returns the customer details</response>
        /// <response code="404">If the customer is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return Ok(customerDto);
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="createCustomerDto">The customer data to create</param>
        /// <returns>A newly created customer</returns>
        /// <response code="201">Returns the newly created customer</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CustomerDto createCustomerDto)
        {
            var customer = _mapper.Map<Customer>(createCustomerDto);
            var createdCustomer = await _customerService.CreateCustomerAsync(customer);
            var customerDto = _mapper.Map<CustomerDto>(createdCustomer);
            return CreatedAtAction(nameof(GetById), new { id = customerDto.Id }, customerDto);
        }

        /// <summary>
        /// Updates a customer.
        /// </summary>
        /// <param name="id">The ID of the customer to update</param>
        /// <param name="updateCustomerDto">The updated customer data</param>
        /// <returns>No content</returns>
        /// <response code="204">If the update is successful</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the customer is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDto updateCustomerDto)
        {
            var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCustomerDto, existingCustomer);
            await _customerService.UpdateCustomerAsync(existingCustomer);

            return NoContent();
        }

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        /// <param name="id">The ID of the customer to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">If the deletion is successful</response>
        /// <response code="404">If the customer is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _customerService.DeleteCustomerAsync(id);
            return NoContent();
        }
    }
}
