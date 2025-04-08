using LeaderReport.Application.DTOs;
using LeaderReport.Infrastructure.Repositories.Generic;
using LeaderReport.Models;

namespace LeaderReport.Application.Services
{
    public class MarcaService
    {
        private readonly IGenericRepository<Marca> _productRepository;

        public MarcaService(IGenericRepository<Marca> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<MarcaDTO>> GetAllProducts()
        {
            var Marcas = await _productRepository.GetAll();

            return Marcas.Select(p => new MarcaDTO
            {
                id = p.Id,
                nombre = p.Nombre
            });
        }

        public async Task<MarcaDTO> GetProductById(int id)
        {
            var Marca = await _productRepository.GetById(id);

            if (Marca == null)
                throw new Exception("Marca no encontrado");

            return new MarcaDTO
            {
                id = Marca.Id,
                nombre = Marca.Nombre
            };
        }
        public async Task AddProduct(MarcaDTO marcaDto)
        {
            var marca = new Marca()
            {
                Nombre = marcaDto.nombre,
            };

            await _productRepository.Add(marca);

        }
    }
}
