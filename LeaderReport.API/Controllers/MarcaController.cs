using LeaderReport.Application.DTOs;
using LeaderReport.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LeaderReport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaController : ControllerBase
    {
        private readonly MarcaService _marcaService;

        public MarcaController(MarcaService marcaService)
        {
            _marcaService = marcaService;
        }

        // GET: api/Marca
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcaDTO>>> GetAll()
        {
            var marcas = await _marcaService.GetAllProducts();
            return Ok(marcas);
        }

        // GET: api/Marca/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MarcaDTO>> GetById(int id)
        {
            try
            {
                var marca = await _marcaService.GetProductById(id);
                return Ok(marca);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // POST: api/Marca
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MarcaDTO marcaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _marcaService.AddProduct(marcaDto);
                return Ok(new { Message = "Marca agregada correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
