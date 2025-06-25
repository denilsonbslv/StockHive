using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockHive.Data;
using StockHive.DTOs;
using StockHive.DTOs.Supplier;
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
    /// Controller para gerenciamento de fornecedores.
    /// </summary>
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        // O construtor recebe o contexto do banco e o serviço de mapeamento
        public SuppliersController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/v1/suppliers
        /// <summary>
        /// Obtém uma lista paginada de fornecedores com opcionais filtros de nome, email e telefone.
        /// </summary>
        /// <param name="queryParams">Parâmetros de consulta para filtrar e paginar resultados.</param>
        /// <returns>Retorna um objeto PagedResultDto contendo os fornecedores.</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResultDto<SupplierDto>>> GetSuppliers([FromQuery] SupplierQueryParameters queryParams)
        {
            // 1. Inicia a construção da query com IQueryable.
            // Nada é executado no banco de dados neste ponto.
            IQueryable<Supplier> query = _context.Suppliers.AsQueryable();

            // 2. Aplica filtros de texto dinamicamente.
            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                query = query.Where(s => s.Name.Contains(queryParams.Name));
            }

            if (!string.IsNullOrEmpty(queryParams.Email))
            {
                query = query.Where(s => s.Email != null && s.Email.Contains(queryParams.Email));
            }

            if (!string.IsNullOrEmpty(queryParams.Phone))
            {
                query = query.Where(s => s.Phone != null && s.Phone.Contains(queryParams.Phone));
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
            var suppliersFromDb = await pagedQuery.ToListAsync();

            // 7. Mapeia as entidades para DTOs.
            var suppliersDto = _mapper.Map<List<SupplierDto>>(suppliersFromDb);

            // 8. Cria e retorna o objeto de resultado paginado.
            var pagedResult = new PagedResultDto<SupplierDto>
            {
                Items = suppliersDto,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                TotalRecords = totalRecords
            };

            return Ok(pagedResult);
        }

        /// <summary>
        /// Obtém um fornecedor específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do fornecedor.</param>
        /// <returns>Dados do fornecedor.</returns>
        // GET: api/suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierDto>> GetSupplier(long id)
        {
            var supplierFromDb = await _context.Suppliers.FindAsync(id);

            if (supplierFromDb == null)
            {
                return NotFound($"Fornecedor com ID {id} não encontrado.");
            }

            var supplierDto = _mapper.Map<SupplierDto>(supplierFromDb);
            return Ok(supplierDto);
        }

        /// <summary>
        /// Cria um novo fornecedor.
        /// </summary>
        /// <param name="createDto">Dados para criação do fornecedor.</param>
        /// <returns>Fornecedor criado.</returns>
        // POST: api/suppliers
        [HttpPost]
        public async Task<ActionResult<SupplierDto>> PostSupplier(CreateSupplierDto createDto)
        {
            var supplier = _mapper.Map<Supplier>(createDto);

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync(); // Aciona nossa lógica de auditoria (CreatedAt/UpdatedAt)

            var supplierDto = _mapper.Map<SupplierDto>(supplier);
            return CreatedAtAction(nameof(GetSupplier), new { id = supplierDto.Id }, supplierDto);
        }

        /// <summary>
        /// Atualiza um fornecedor existente.
        /// </summary>
        /// <param name="id">ID do fornecedor.</param>
        /// <param name="updateDto">Dados para atualização do fornecedor.</param>
        /// <returns>Nenhum conteúdo se sucesso.</returns>
        // PUT: api/suppliers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(long id, UpdateSupplierDto updateDto)
        {
            var supplierFromDb = await _context.Suppliers.FindAsync(id);

            if (supplierFromDb == null)
            {
                return NotFound($"Fornecedor com ID {id} não encontrado para atualização.");
            }

            // O AutoMapper atualiza a entidade 'supplierFromDb' com os dados do 'updateDto'.
            _mapper.Map(updateDto, supplierFromDb);

            // O EF Core já está rastreando a entidade, então basta salvar.
            await _context.SaveChangesAsync(); // Aciona nossa lógica de auditoria (UpdatedAt)

            return NoContent();
        }

        // MÉTODO PATCH: Aplica uma atualização PARCIAL.
        // Espera um objeto apenas com as propriedades que devem ser alteradas.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchSupplier(long id, UpdateSupplierDto patchDto)
        {
            var supplierFromDb = await _context.Suppliers.FindAsync(id);

            if (supplierFromDb == null)
            {
                return NotFound($"Fornecedor com ID {id} não encontrado.");
            }

            // Lógica de atualização parcial: só atualiza o que não for nulo no DTO.
            // Para strings, checamos IsNullOrEmpty para não atualizar com uma string vazia.
            if (!string.IsNullOrEmpty(patchDto.Name))
            {
                supplierFromDb.Name = patchDto.Name;
            }
            if (patchDto.ContactPerson != null)
            {
                supplierFromDb.ContactPerson = patchDto.ContactPerson;
            }
            if (!string.IsNullOrEmpty(patchDto.Email))
            {
                supplierFromDb.Email = patchDto.Email;
            }
            if (patchDto.Phone != null)
            {
                supplierFromDb.Phone = patchDto.Phone;
            }
            if (patchDto.Address != null)
            {
                supplierFromDb.Address = patchDto.Address;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Realiza soft delete de um fornecedor.
        /// </summary>
        /// <param name="id">ID do fornecedor.</param>
        /// <returns>Nenhum conteúdo se sucesso.</returns>
        // DELETE: api/suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(long id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound($"Fornecedor com ID {id} não encontrado para deleção.");
            }

            // Implementação do SOFT DELETE
            if (supplier is IAuditable auditableEntity)
            {
                auditableEntity.DeletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}