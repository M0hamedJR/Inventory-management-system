using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO.Product;
using System.Collections.Generic;
using static Shared.DTO.Shelf.Shelf;

namespace InventrySystem.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public ProductController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _repository.Product.GetAllProductsAsync(trackChanges: false);
                _logger.LogInfo("Returned all products from database.");

                var productsResult = _mapper.Map<IEnumerable<ProductDto>>(products);
                return Ok(productsResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllProducts action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetProductCountPerCategory()
        {
            try
            {
                var categoryProductCounts = await _repository.Product.GetProductCountPerCategoryAsync(trackChanges: false);
                _logger.LogInfo("Returned Product counts per category from database.");

                return Ok(categoryProductCounts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetProductCountPerCategory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("{id}", Name = "ProductById")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var product = await _repository.Product.GetProductByIdAsync(id, trackChanges: false);
                if (product == null)
                {
                    _logger.LogError($"Product with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _logger.LogInfo($"Returned product with id: {id}");

                var productResult = _mapper.Map<ProductDto>(product);
                return Ok(productResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetProductById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductForCreationDto product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogError("product object sent from client is null.");
                    return BadRequest("product object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid product object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var productEntity = _mapper.Map<Product>(product);
                productEntity.SerialNumber = Guid.NewGuid();
                productEntity.Warehouse = null;
                productEntity.Shelf = null;

                _repository.Product.CreateProduct(productEntity);
                 await _repository.SaveAsync();

                var createdProduct = _mapper.Map<ProductDto>(productEntity);

                return CreatedAtRoute("ProductById", new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductForUpdateDto product)
        {
            try
            {
                if (product == null)
                {
                    _logger.LogError("product object sent from client is null.");
                    return BadRequest("product object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid product object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var productEntity = await _repository.Product.GetProductByIdAsync(id, trackChanges: true);
                if (productEntity == null)
                {
                    _logger.LogError($"product with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _mapper.Map(product, productEntity);

                _repository.Product.UpdateProduct(productEntity);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        public class ExtractParameterRequest
        {
            public string Data { get; set; }
            public ProductForUpdateDto Product { get; set; } = new ProductForUpdateDto();
        }

        [HttpPost("extract-parameter")]
        public async Task<IActionResult> ExtractParameter([FromBody] ExtractParameterRequest request)
        {

            if (request == null || string.IsNullOrEmpty(request.Data))
            {
                return BadRequest("Request body or data is required.");
            }

            string data = request.Data;

            // Split the data by spaces
            string[] parts = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 18) // Must be at least 19 parts: 1 + 1 + 16 (GUID) + 1
            {
                return BadRequest("Data format is incorrect.");
            }

            // Parse inOrOut (first value)
            bool inOrOut;
            if (parts[0] == "1")
                inOrOut = true;
            else if (parts[0] == "0")
                inOrOut = false;
            else
                return BadRequest("Invalid inOrOut parameter. Expected '1' or '0'.");

            // Parse warehouse (second value, should be a single character)
            if (parts[1].Length != 1)
            {
                return BadRequest("Invalid warehouse parameter. Expected a single character.");
            }
            char warehouse = parts[1][0];

            // Extract the 16-byte serial number (from index 2 to index 17)
            string serialHexString = string.Join("", parts.Skip(2).Take(16)); // Remove spaces and join

            if (serialHexString.Length != 32) // A valid GUID must have 32 hex digits
            {
                return BadRequest("Invalid serial number length. Expected 16 hexadecimal values.");
            }

            // Convert hex string to byte array
            byte[] serialBytes = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                if (!byte.TryParse(parts[i + 2], System.Globalization.NumberStyles.HexNumber, null, out serialBytes[i]))
                {
                    return BadRequest($"Invalid hex value at position {i + 2}: {parts[i + 2]}");
                }
            }

            // Convert byte array to GUID by ensuring correct endianess
            Guid serialNumber = new Guid(
                new byte[]
                {
                    serialBytes[3], serialBytes[2], serialBytes[1], serialBytes[0], // Data1 (4 bytes, reversed)
                    serialBytes[5], serialBytes[4], // Data2 (2 bytes, reversed)
                    serialBytes[7], serialBytes[6], // Data3 (2 bytes, reversed)
                    serialBytes[8], serialBytes[9], serialBytes[10], serialBytes[11], // Data4 (big-endian, unchanged)
                    serialBytes[12], serialBytes[13], serialBytes[14], serialBytes[15]  // Data5 (big-endian, unchanged)
                }
            );
            var productEntity = await _repository.Product.GetProductBySerialNumberAsync(serialNumber, trackChanges: false);
            try
            {
                return await AssignShelfToProduct(inOrOut, warehouse, serialNumber, _mapper.Map(request.Product, productEntity));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ExtractParam calling AssignShelfToProduct: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpPost("assign-shelf")]
        public async Task<IActionResult> AssignShelfToProduct(bool inOrOut, char warehouse, Guid serialNumber, Product product)
        {
            try
            {
                if (product.In_Out == true)
                {
                    return BadRequest("Product is assigned.");
                }
                if (serialNumber == Guid.Empty)
                {
                    _logger.LogError("Serial number is empty.");
                    return BadRequest("Invalid serial number.");
                }
                if (product == null)
                {
                    _logger.LogError("Product not found for given serial number.");
                    return NotFound("Product not found.");
                }

                // Get all warehouses asynchronously
                var warehouses = await _repository.Warehouse.GetAllWarehousesAsync(trackChanges: false);

                // Find a warehouse where the last character of its name matches the given char
                var matchingWarehouse = warehouses.FirstOrDefault(w => w.Name.LastOrDefault() == warehouse);
                if (matchingWarehouse == null)
                {
                    return BadRequest("No matching warehouse found.");
                }

                // Get shelf counts
                var shelfCounts = await _repository.Shelf.GetShelfCountAsync(trackChanges: false);

                // Find the warehouse count from the shelfCounts list
                var warehouseShelfCount = shelfCounts.FirstOrDefault(s => s.WarehouseName == matchingWarehouse.Name);

                if (warehouseShelfCount == null)
                {
                    return BadRequest("No shelf count data available for the warehouse.");
                }
                // Get product entity
                var productEntity = await _repository.Product.GetProductBySerialNumberAsync(serialNumber, trackChanges: false);
                if (productEntity == null)
                {
                    return NotFound("Product not found.");
                }

                if (productEntity.SerialNumber != serialNumber)
                {
                    return BadRequest("Product serial number does not match.");
                } 

                // Get all shelves asynchronously
                var shelves = await _repository.Shelf.GetAllShelfsAsync(trackChanges: false);
                var availableShelf = shelves.FirstOrDefault(s => s.IsAvailable && s.Warehouse.Id == matchingWarehouse.Id);

                if (inOrOut) // Check-in: Assign a shelf
                {
                    if (productEntity.ShelfId != null)
                        return BadRequest("The Product already assigned to a shelf.");
                    
                    if (availableShelf != null)
                    {
                        productEntity.ShelfId = availableShelf.Id;
                        productEntity.WarehouseId = matchingWarehouse?.Id;
                        productEntity.In_Out = true;
                        availableShelf.IsAvailable = false;

                        _repository.Shelf.UpdateShelf(availableShelf);
                        _repository.Product.UpdateProduct(productEntity);
                        await _repository.SaveAsync();

                        return Ok("Product assigned to a shelf.");
                    }
                    return BadRequest("No available shelves to assign.");
                }
                else // Check-out: Unassign shelf
                {
                    if (productEntity.ShelfId != null)
                    {
                        var assignedShelf = shelves.FirstOrDefault(s => s.Id == productEntity.ShelfId);
                        if (assignedShelf != null)
                        {
                            assignedShelf.IsAvailable = true;
                            _repository.Shelf.UpdateShelf(assignedShelf);
                        }

                        productEntity.ShelfId = null;
                        productEntity.WarehouseId = null;
                        productEntity.In_Out = false;

                        _repository.Product.UpdateProduct(productEntity);

                        await _repository.SaveAsync();
                        return Ok("Product checked out from shelf.");
                    }
                    return BadRequest("Product is not assigned to any shelf.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AssignShelfToProduct: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                var product = await _repository.Product.GetProductByIdAsync(id, trackChanges: false);
                if (product == null)
                {
                    _logger.LogError($"product with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.Product.DeleteProduct(product);
                await _repository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
