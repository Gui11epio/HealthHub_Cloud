using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using @enum;
using AutoMapper;
using Health_Hub.Application.DTOs.Request;
using Health_Hub.Application.DTOs.Response;
using Health_Hub.Application.Services;
using Health_Hub.Domain.Entities;
using Health_Hub.Domain.IRepositories;
using Moq;

namespace Health_Hub.Tests.Application
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UsuarioService _service;

        public UsuarioServiceTests()
        {
            _repoMock = new Mock<IUsuarioRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new UsuarioService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Criar_DeveAdicionarUsuario_ComTodosCampos()
        {
            var request = new UsuarioRequest
            {
                EmailCorporativo = "teste@empresa.com",
                Nome = "João da Silva",
                Senha = "Senha@123",
                Tipo = TipoUsuario.FUNCIONARIO
            };

            var entity = new Usuario
            {
                Id = 1,
                EmailCorporativo = request.EmailCorporativo,
                Nome = request.Nome,
                Senha = request.Senha,
                Tipo = request.Tipo
            };

            var response = new UsuarioResponse
            {
                Id = 1,
                EmailCorporativo = request.EmailCorporativo,
                Nome = request.Nome,
                Tipo = request.Tipo
            };

            _mapperMock.Setup(m => m.Map<Usuario>(request)).Returns(entity);
            _repoMock.Setup(r => r.AddAsync(entity)).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<UsuarioResponse>(entity)).Returns(response);

            var result = await _service.Criar(request);

            Assert.Equal("teste@empresa.com", result.EmailCorporativo);
            Assert.Equal("João da Silva", result.Nome);
            Assert.Equal(TipoUsuario.FUNCIONARIO, result.Tipo);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarUsuarioCompleto()
        {
            var usuario = new Usuario
            {
                Id = 10,
                EmailCorporativo = "alguem@empresa.com",
                Nome = "Alguém",
                Tipo = TipoUsuario.ADMIN
            };

            var usuarioResponse = new UsuarioResponse
            {
                Id = 10,
                EmailCorporativo = usuario.EmailCorporativo,
                Nome = usuario.Nome,
                Tipo = usuario.Tipo
            };

            _repoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(usuario);
            _mapperMock.Setup(m => m.Map<UsuarioResponse>(usuario)).Returns(usuarioResponse);

            var result = await _service.ObterPorId(10);

            Assert.NotNull(result);
            Assert.Equal("Alguem@empresa.com".ToLower(), result.EmailCorporativo.ToLower());
        }

        [Fact]
        public async Task Atualizar_DeveAlterarApenasDadosDoDTO()
        {
            var entity = new Usuario
            {
                Id = 1,
                EmailCorporativo = "antigo@empresa.com",
                Nome = "Nome Antigo",
                Senha = "Senha@123",
                Tipo = TipoUsuario.FUNCIONARIO
            };

            var request = new UsuarioRequest
            {
                EmailCorporativo = "novo@empresa.com",
                Nome = "Novo Nome",
                Senha = "NovaSenha@123",
                Tipo = TipoUsuario.ADMIN
            };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateAsync(entity)).ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map(request, entity));

            var result = await _service.Atualizar(1, request);

            Assert.True(result);
        }

        [Fact]
        public async Task Remover_DeveRetornarTrue()
        {
            _repoMock.Setup(r => r.DeleteAsync(5)).ReturnsAsync(true);

            var result = await _service.Remover(5);

            Assert.True(result);
        }
    }

}
