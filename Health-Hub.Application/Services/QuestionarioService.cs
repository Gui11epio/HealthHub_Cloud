using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Health_Hub.Application.DTOs.Request;
using Health_Hub.Application.DTOs.Response;
using Health_Hub.Domain.Entities;
using Health_Hub.Domain.Interfaces;
using Sprint1_C_.Application.DTOs.Response;

namespace Health_Hub.Application.Services
{
    public class QuestionarioService
    {
        private readonly IQuestionarioRepository _repo;
        private readonly IMapper _mapper;

        public QuestionarioService(IQuestionarioRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<QuestionarioResponse>> ObterTodos()
        {
            var questionarios = await _repo.GetAllAsync();
            return _mapper.Map<List<QuestionarioResponse>>(questionarios);
        }

        public async Task<QuestionarioResponse?> ObterPorId(int id)
        {
            var questionario = await _repo.GetByIdAsync(id);
            return questionario == null ? null : _mapper.Map<QuestionarioResponse>(questionario);
        }

        public async Task<PagedResult<QuestionarioResponse>> ObterPorPagina(int pageNumber, int pageSize)
        {
            var (itens, total) = await _repo.GetAllByPageAsync(pageNumber, pageSize);
            return new PagedResult<QuestionarioResponse>
            {
                Numeropag = pageNumber,
                Tamnhopag = pageSize,
                Total = total,
                Itens = _mapper.Map<List<QuestionarioResponse>>(itens)
            };
        }

        public async Task<QuestionarioResponse> Criar(QuestionarioRequest request)
        {
            var novoQuestionario = _mapper.Map<Questionario>(request);

            // gerar avaliação automática
            novoQuestionario.Avaliacao = GerarAvaliacao(novoQuestionario);

            await _repo.AddAsync(novoQuestionario);

            return _mapper.Map<QuestionarioResponse>(novoQuestionario);
        }


        public async Task<bool> Deletar(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        // Função para análise de saúde mental no trabalho
        private string GerarAvaliacao(Questionario questionario)
        {
            int pontosRuins = 0;

            if (questionario.NivelEstresse >= 7) pontosRuins++;
            if (questionario.Ansiedade >= 7) pontosRuins++;
            if (questionario.Sobrecarga >= 7) pontosRuins++;
            if (questionario.QualidadeSono <= 3) pontosRuins++;

            if (pontosRuins == 0)
                return "Seu bem-estar mental aparenta estar estável. Continue mantendo bons hábitos.";

            if (pontosRuins == 1)
                return "Alguns sinais de alerta foram identificados. Recomendamos atenção ao equilíbrio entre vida pessoal e trabalho.";

            if (pontosRuins <= 3)
                return "Níveis elevados de estresse, ansiedade ou sobrecarga encontrados. Considere buscar apoio e conversar com um profissional.";

            return "Alto risco de burnout! Procure ajuda imediatamente e converse com a equipe de saúde ocupacional.";
        }

    }

}
