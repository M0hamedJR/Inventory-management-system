using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using static Shared.DTO.Shelf.Shelf;
using Shelf = Entities.Models.Shelf;

namespace InventrySystem.Controllers
{
    [Route("api/shelfs")]
    [ApiController]
    public class ShelfController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ShelfController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
      
        [HttpGet]
        public async Task<IActionResult> GetAllShelfs()
        {
            try
            {
                var shelfs = await _repository.Shelf.GetAllShelfsAsync(trackChanges: false);
                _logger.LogInfo("Returned all Shelfs from database.");

                var shelfsResult = _mapper.Map<IEnumerable<ShelfDto>>(shelfs);
                return Ok(shelfsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllShelfs action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "ShelfById")]
        public async Task<IActionResult> GetShelfById(Guid id)
        {
            try
            {
                var shelf = await _repository.Shelf.GetShelfByIdAsync(id, trackChanges: false);
                if (shelf == null)
                {
                    _logger.LogError($"shelf with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned shelf with id: {id}");

                var shelfResult = _mapper.Map<ShelfDto>(shelf);
                return Ok(shelfResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetShelfById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetShelfCount()
        {
            try
            {
                var shelfCounts = await _repository.Shelf.GetShelfCountAsync(trackChanges: false);
                _logger.LogInfo("Returned Shelf counts from database.");

                return Ok(shelfCounts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetShelfCount action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateShelf([FromBody] ShelfForCreationDto shelf)
        {
            try
            {
                if (shelf == null)
                {
                    _logger.LogError("Shelf object sent from client is null.");
                    return BadRequest("Shelf object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Shelf object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var shelfEntity = _mapper.Map<Shelf>(shelf);

                _repository.Shelf.CreateShelf(shelfEntity);
                await _repository.SaveAsync();

                var createdShelf = _mapper.Map<ShelfDto>(shelfEntity);

                return CreatedAtRoute("ShelfById", new { id = createdShelf.Id }, createdShelf);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateShelf action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShelf(Guid id, [FromBody] ShelfForUpdateDto shelf)
        {
            try
            {
                if (shelf == null)
                {
                    _logger.LogError("shelf object sent from client is null.");
                    return BadRequest("shelf object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid shelf object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var shelfEntity = await _repository.Shelf.GetShelfByIdAsync(id, trackChanges: true);
                if (shelfEntity == null)
                {
                    _logger.LogError($"shelf with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(shelf, shelfEntity);

                _repository.Shelf.UpdateShelf(shelfEntity);
                 await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateShelf action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShelf(Guid id)
        {
            try
            {
                var shelf = await _repository.Shelf.GetShelfByIdAsync(id, trackChanges: false);
                if (shelf == null)
                {
                    _logger.LogError($"Shelf with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Shelf.DeleteShelf(shelf);
                 await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteShelf action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
