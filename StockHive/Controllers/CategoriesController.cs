using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockHive.Data;
using StockHive.DTOs;
using StockHive.DTOs.Category;
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
    /// Controller para gerenciamento de categorias.
    /// </summary>
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        // O construtor recebe o contexto do banco e o serviço de mapeamento
        public CategoriesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/v1/Categories
        /// <summary>
        /// Obtém uma lista paginada de categorias com opcionais filtros de nome, email e telefone.
        /// </summary>
        /// <param name="queryParams">Parâmetros de consulta para filtrar e paginar resultados.</param>
        /// <returns>Retorna um objeto PagedResultDto contendo os categorias.</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResultDto<CategoryDto>>> GetCategories([FromQuery] CategoryQueryParameters queryParams)
        {
            // 1. Inicia a construção da query com IQueryable.
            // Nada é executado no banco de dados neste ponto.
            IQueryable<Category> query = _context.Categories.AsQueryable();

            // 2. Aplica filtros de texto dinamicamente.
            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                query = query.Where(s => s.Name.Contains(queryParams.Name));
            }

            if (queryParams.ParentCategoryId.HasValue)
            {
                query = query.Where(s => s.ParentCategoryId != null && s.ParentCategoryId == queryParams.ParentCategoryId);
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

            query = query.Include(c => c.SubCategories);

            // 4. Obtém o número total de registros que correspondem aos filtros.
            // Esta é a primeira consulta que vai ao banco, um "SELECT COUNT(*)...".
            var totalRecords = await query.CountAsync();

            // 5. Aplica a paginação à query.
            var pagedQuery = query.Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
                                  .Take(queryParams.PageSize);

            // 6. Executa a query principal para buscar os dados da página.
            // Esta é a segunda consulta, um "SELECT ... OFFSET ... FETCH NEXT ...".
            var categoriesFromDb = await pagedQuery.ToListAsync();

            // 7. Mapeia as entidades para DTOs.
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categoriesFromDb);

            // 8. Cria e retorna o objeto de resultado paginado.
            var pagedResult = new PagedResultDto<CategoryDto>
            {
                Items = categoriesDto,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                TotalRecords = totalRecords
            };

            return Ok(pagedResult);
        }

        /// <summary>
        /// Obtém um Categoria específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do Categoria.</param>
        /// <returns>Dados do Categoria.</returns>
        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(long id)
        {
            var categoryFromDb = await _context.Categories
                                               .Include(c => c.SubCategories)
                                               .FirstOrDefaultAsync(c => c.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound($"Categoria com ID {id} não encontrado.");
            }

            var categoryDto = _mapper.Map<CategoryDto>(categoryFromDb);
            return Ok(categoryDto);
        }

        /// <summary>
        /// Cria um novo Categoria.
        /// </summary>
        /// <param name="createDto">Dados para criação do Categoria.</param>
        /// <returns>Categoria criado.</returns>
        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CreateCategoryDto createDto)
        {
            if (createDto.ParentCategoryId.HasValue)
            {
                // OTIMIZAÇÃO: Usar AnyAsync é um pouco mais performático que FindAsync para apenas checar a existência.
                var parentExists = await _context.Categories.AnyAsync(c => c.Id == createDto.ParentCategoryId.Value);
                if (!parentExists)
                {
                    return BadRequest("A categoria pai especificada não existe.");
                }
            }

            var category = _mapper.Map<Category>(createDto);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync(); // Aciona nossa lógica de auditoria (CreatedAt/UpdatedAt)

            var categoryDto = _mapper.Map<CategoryDto>(category);
            return CreatedAtAction(nameof(GetCategory), new { id = categoryDto.Id }, categoryDto);
        }

        /// <summary>
        /// Atualiza um Categoria existente.
        /// </summary>
        /// <param name="id">ID do Categoria.</param>
        /// <param name="updateDto">Dados para atualização do Categoria.</param>
        /// <returns>Nenhum conteúdo se sucesso.</returns>
        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(long id, UpdateCategoryDto updateDto)
        {
            var categoryFromDb = await _context.Categories.FindAsync(id);

            if (categoryFromDb == null)
            {
                return NotFound($"Categoria com ID {id} não encontrado para atualização.");
            }

            // O AutoMapper atualiza a entidade 'CategoryFromDb' com os dados do 'updateDto'.
            _mapper.Map(updateDto, categoryFromDb);

            // O EF Core já está rastreando a entidade, então basta salvar.
            await _context.SaveChangesAsync(); // Aciona nossa lógica de auditoria (UpdatedAt)

            return NoContent();
        }

        // MÉTODO PATCH: Aplica uma atualização PARCIAL.
        // Espera um objeto apenas com as propriedades que devem ser alteradas.
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCategory(long id, UpdateCategoryDto patchDto)
        {
            var categoryFromDb = await _context.Categories.FindAsync(id);

            if (categoryFromDb == null)
            {
                return NotFound($"Categoria com ID {id} não encontrado.");
            }

            // Lógica de atualização parcial: só atualiza o que não for nulo no DTO.
            // Para strings, checamos IsNullOrEmpty para não atualizar com uma string vazia.
            if (!string.IsNullOrEmpty(patchDto.Name))
            {
                categoryFromDb.Name = patchDto.Name;
            }
            if (patchDto.ParentCategoryId != null)
            {
                categoryFromDb.ParentCategoryId = patchDto.ParentCategoryId;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Realiza soft delete de um Categoria.
        /// </summary>
        /// <param name="id">ID do Categoria.</param>
        /// <returns>Nenhum conteúdo se sucesso.</returns>
        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var category = await _context.Categories
                                         .Include(c => c.SubCategories)
                                         .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound($"Categoria com ID {id} não encontrado para deleção.");
            }else if (category.DeletedAt.HasValue)
            {
                return BadRequest($"Categoria com ID {id} já foi deletada.");
            }else if(category.SubCategories.Any())
            {
                return BadRequest($"Categoria com ID {id} não pode ser deletada pois possui subcategorias.");
            }

            // Implementação do SOFT DELETE
            if (category is IAuditable auditableEntity)
            {
                auditableEntity.DeletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}