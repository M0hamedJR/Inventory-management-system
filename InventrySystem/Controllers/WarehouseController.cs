using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Warehouse;

namespace InventrySystem.Controllers
{
    [Route("api/Warehouses")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public WarehouseController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWarehouses()
        {
            try
            {
                var Warehouses = await _repository.Warehouse.GetAllWarehousesAsync(trackChanges: false);
                _logger.LogInfo("Returned all Warehouses from database.");

                var WarehousesResult = _mapper.Map<IEnumerable<WarehouseDto>>(Warehouses);
                return Ok(WarehousesResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllWarehouses action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "WarehouseById")]
        public async Task<IActionResult> GetWarehouseById(Guid id)
        {
            try
            {
                var warehouse = await _repository.Warehouse.GetWarehouseByIdAsync(id, trackChanges: false);
                if (warehouse == null)
                {
                    _logger.LogError($"Warehouse with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned warehouse with id: {id}");

                var warehouseResult = _mapper.Map<WarehouseDto>(warehouse);
                return Ok(warehouseResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetWarehouseById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseForCreationDto warehouse)
        {
            try
            {
                if (warehouse == null)
                {
                    _logger.LogError("Warehouse object sent from client is null.");
                    return BadRequest("Warehouse object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid warehouse object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var warehouseEntity = _mapper.Map<Warehouse>(warehouse);

                _repository.Warehouse.CreateWarehouse(warehouseEntity);
                await _repository.SaveAsync();

                var createdWarehouse = _mapper.Map<WarehouseDto>(warehouseEntity);

                return CreatedAtRoute("WarehouseById", new { id = createdWarehouse.Id }, createdWarehouse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateWarehouse action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWarehouse(Guid id, [FromBody] WarehouseForUpdateDto warehouse)
        {
            try
            {
                if (warehouse == null)
                {
                    _logger.LogError("Warehouse object sent from client is null.");
                    return BadRequest("Warehouse object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid warehouse object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var warehouseEntity = await _repository.Warehouse.GetWarehouseByIdAsync(id, trackChanges: true);
                if (warehouseEntity == null)
                {
                    _logger.LogError($"Warehouse with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(warehouse, warehouseEntity);

                _repository.Warehouse.UpdateWarehouse(warehouseEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateWarehouse action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(Guid id)
        {
            try
            {
                var warehouse = await _repository.Warehouse.GetWarehouseByIdAsync(id, trackChanges: false);
                if (warehouse == null)
                {
                    _logger.LogError($"Warehouse with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Warehouse.DeleteWarehouse(warehouse);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteWarehouse action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
