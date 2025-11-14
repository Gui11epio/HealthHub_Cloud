using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Health_Hub.Application.DTOs.Request;
using Health_Hub.Application.DTOs.Response;
using Health_Hub.Application.Services;
using Health_Hub.Domain.Entities;
using Health_Hub.Domain.Interfaces;
using Moq;

namespace Health_Hub.Tests.Application
{
    public class QuestionarioServiceTests
    {
        private readonly Mock<IQuestionarioRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly QuestionarioService _service;

        public QuestionarioServiceTests()
        {
            _repoMock = new Mock<IQuestionarioRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new QuestionarioService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Criar_DeveGerarAvaliacaoCorreta_QuandoHaRiscosModERados()
        {
            var request = new QuestionarioRequest
            {
                UsuarioId = 1,
                NivelEstresse = 8,
                Ansiedade = 7,
                Sobrecarga = 4,
                QualidadeSono = 2
            };

            var entity = new Questionario
            {
                UsuarioId = request.UsuarioId,
                NivelEstresse = request.NivelEstresse,
                Ansiedade = request.Ansiedade,
                Sobrecarga = request.Sobrecarga,
                QualidadeSono = request.QualidadeSono
            };

            var response = new QuestionarioResponse
            {
                UsuarioId = 1,
                NivelEstresse = 8,
                Ansiedade = 7,
                Sobrecarga = 4,
                QualidadeSono = 2,
                Avaliacao = "Níveis elevados de estresse"
            };

            _mapperMock.Setup(m => m.Map<Questionario>(request)).Returns(entity);
            _repoMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<QuestionarioResponse>(entity)).Returns(response);

            var result = await _service.Criar(request);

            Assert.Contains("Níveis elevados", result.Avaliacao);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarQuestionarioCompleto()
        {
            var entity = new Questionario
            {
                Id = 50,
                UsuarioId = 99,
                NivelEstresse = 5,
                Ansiedade = 4,
                Sobrecarga = 3,
                QualidadeSono = 7,
                Avaliacao = "OK"
            };

            var resp = new QuestionarioResponse
            {
                Id = 50,
                UsuarioId = 99,
                NivelEstresse = 5,
                Ansiedade = 4,
                Sobrecarga = 3,
                QualidadeSono = 7,
                Avaliacao = "OK"
            };

            _repoMock.Setup(r => r.GetByIdAsync(50)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<QuestionarioResponse>(entity)).Returns(resp);

            var result = await _service.ObterPorId(50);

            Assert.NotNull(result);
            Assert.Equal("OK", result.Avaliacao);
        }

        [Fact]
        public async Task Deletar_DeveRetornarTrue()
        {
            _repoMock.Setup(r => r.DeleteAsync(3)).ReturnsAsync(true);

            var result = await _service.Deletar(3);

            Assert.True(result);
        }
    }

}
