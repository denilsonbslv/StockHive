using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockHive.Data;
using StockHive.DTOs;
using StockHive.DTOs.Location;
using StockHive.Interfaces;
using StockHive.Models;
using StockHive.QueryParameters;
using System.Linq;

namespace StockHive.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    /// <summary>
    /// Controller para gerenciamento de locais.
    /// </summary>
    public class LocationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        // O construtor recebe o contexto do banco e o serviço de mapeamento
        public LocationsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/v1/locations
        /// <summary>
        /// Obtém uma lista paginada de locais com opcionais filtros de nome e endereço.
        /// </summary>
        /// <param name="queryParams">Parâmetros de consulta para filtrar e paginar resultados.</param>
        /// <returns>Retorna um objeto PagedResultDto contendo os locais.</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResultDto<LocationDto>>> GetLocations([FromQuery] LocationQueryParameters queryParams)
        {
            // 1. Inicia a construção da query com IQueryable.
            // Nada é executado no banco de dados neste ponto.
            IQueryable<Location> query = _context.Locations.AsQueryable();

            // 2. Aplica filtros de texto dinamicamente.
            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                query = query.Where(s => s.Name.Contains(queryParams.Name));
            }

            if (!string.IsNullOrEmpty(queryParams.Address))
            {
                query = query.Where(s => s.Address != null && s.Address.Contains(queryParams.Address));
            }

            // 3. Aplica filtros de data dinamicamente.
            if (queryParams.CreatedAtFrom.HasValue)
            {
                query = query.Where(s => s.CreatedAt >= queryParams.CreatedAtFrom.Value);
            }

            if (queryParams.CreatedAtTo.HasValue)
            {
                // Adiciona 1 dia à data final e usa "<" para incluir todos os registros
                // do dia final, até as 23:59:59.
                var dateTo = queryParams.CreatedAtTo.Value.AddDays(1);
                query = query.Where(s => s.CreatedAt < dateTo);
            }

            // 4. Obtém o número total de registros que correspondem aos filtros.
            // Esta é a primeira consulta que vai ao banco, um "SELECT COUNT(*)...".
            var totalRecords = await query.CountAsync();

            // 5. Aplica a paginação à query.
            var pagedQuery = query.Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                                  .Take(queryParams.PageSize);

            // 6. Executa a query principal para buscar os dados da página.
            // Esta é a segunda consulta, um "SELECT ... OFFSET ... FETCH NEXT ...".
            var locationsFromDb = await pagedQuery.ToListAsync();

            // 7. Mapeia as entidades para DTOs.
            var locationsDto = _mapper.Map<List<LocationDto>>(locationsFromDb);

            // 8. Cria e retorna o objeto de resultado paginado.
            var pagedResult = new PagedResultDto<LocationDto>
            {
                Items = locationsDto,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                TotalRecords = totalRecords
            };

            return Ok(pagedResult);
        }

        /// <summary>
        /// Obtém um local específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do local.</param>
        /// <returns>Dados do local.</returns>
        // GET: api/locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDto>> GetLocation(long id)
        {
            var locationFromDb = await _context.Locations.FindAsync(id);

            if (locationFromDb == null)
            {
                return NotFound($"Local com ID {id} não encontrado.");
            }

            var locationDto = _mapper.Map<LocationDto>(locationFromDb);
            return Ok(locationDto);
        }

        /// <summary>
        /// Cria um novo local.
        /// </summary>
        /// <param name="createDto">Dados para criação do local.</param>
        /// <returns>Local criado.</returns>
        // POST: api/locations
        [HttpPost]
        public async Task<ActionResult<LocationDto>> PostLocation(CreateLocationDto createDto)
        {
            var location = _mapper.Map<Location>(createDto);

            _context.Locations.Add(location);
            await _context.SaveChangesAsync(); // Aciona nossa lógica de auditoria (CreatedAt/UpdatedAt)

            var locationDto = _mapper.Map<LocationDto>(location);
            return CreatedAtAction(nameof(GetLocation), new { id = locationDto.Id }, locationDto);
        }

        /// <summary>
        /// Atualiza um local existente.
        /// </summary>
        /// <param name="id">ID do local.</param>
        /// <param name="updateDto">Dados para atualização do local.</param>
        /// <returns>Nenhum conteúdo se sucesso.</returns>
        // PUT: api/locations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(long id, UpdateLocationDto updateDto)
        {
            var locationFromDb = await _context.Locations.FindAsync(id);

            if (locationFromDb == null)
            {
                return NotFound($"Local com ID {id} não encontrado para atualização.");
            }

            // O AutoMapper atualiza a entidade 'locationFromDb' com os dados do 'updateDto'.
            _mapper.Map(updateDto, locationFromDb);

            // O EF Core já está rastreando a entidade, então basta salvar.
            await _context.SaveChangesAsync(); // Aciona nossa lógica de auditoria (UpdatedAt)

            return NoContent();
        }

        // MÉTODO PATCH: Aplica uma atualização PARCIAL.
        // Espera um objeto apenas com as propriedades que devem ser alteradas.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchLocation(long id, UpdateLocationDto patchDto)
        {
            var locationFromDb = await _context.Locations.FindAsync(id);

            if (locationFromDb == null)
            {
                return NotFound($"Local com ID {id} não encontrado.");
            }

            // Lógica de atualização parcial: só atualiza o que não for nulo no DTO.
            // Para strings, checamos IsNullOrEmpty para não atualizar com uma string vazia.
            if (!string.IsNullOrEmpty(patchDto.Name))
            {
                locationFromDb.Name = patchDto.Name;
            }
            if (patchDto.Address != null)
            {
                locationFromDb.Address = patchDto.Address;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Realiza soft delete de um local.
        /// </summary>
        /// <param name="id">ID do local.</param>
        /// <returns>Nenhum conteúdo se sucesso.</returns>
        // DELETE: api/locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(long id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound($"Local com ID {id} não encontrado para deleção.");
            }

            // Implementação do SOFT DELETE
            if (location is IAuditable auditableEntity)
            {
                auditableEntity.DeletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}