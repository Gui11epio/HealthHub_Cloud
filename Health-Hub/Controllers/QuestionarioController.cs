using Health_Hub.Application.DTOs.Request;
using Health_Hub.Application.DTOs.Response;
using Health_Hub.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Sprint1_C_.Application.DTOs.Response;
using Swashbuckle.AspNetCore.Annotations;

namespace Health_Hub.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [SwaggerTag("Controlador para gerenciar questionários")]
    public class QuestionarioController : ControllerBase
    {
        private readonly QuestionarioService _svc;
        public QuestionarioController(QuestionarioService svc)
        {
            _svc = svc;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var questionario = await _svc.ObterPorId(id);
            if(questionario == null) return NotFound();
            return Ok(questionario);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var questionario = await _svc.ObterTodos();
            if(questionario == null) return NotFound();
            return Ok(questionario);
        }

        [HttpGet("pagina")]
        public async Task<ActionResult<PagedResult<QuestionarioResponse>>> GetPaged(int numeroPag = 1, int tamanhoPag = 10)
        {
            var result = await _svc.ObterPorPagina(numeroPag, tamanhoPag);
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Cria um novo questionário e gera uma análise de saúde mental.",
            Description = "Com base nos níveis de estresse, sono, ansiedade e sobrecarga informados, retorna um resumo sobre o bem-estar mental do colaborador."
        )]
        [ProducesResponseType(typeof(QuestionarioResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] QuestionarioRequest request)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdQuestionario = await _svc.Criar(request);
            return CreatedAtAction(nameof(GetById), new { id = createdQuestionario.Id }, createdQuestionario);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _svc.Deletar(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
