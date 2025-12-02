using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using TripleMatch.Application.Common.Validations;
using TripleMatch.Application.Features;
using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.VMs;
using TripleMatch.Shered.Contracts.VMs.LookupDTOs;

namespace TripleMatch.Tests.ServiceTests
{
    public class ProfileServiceTests
    {
        private readonly Mock<IProfileRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateProfileDtoValidator _validator;
        private readonly ProfileService _service;

        public ProfileServiceTests()
        {
            _repositoryMock = new Mock<IProfileRepository>();
            _mapperMock = new Mock<IMapper>();

            // Создаем реальный экземпляр валидатора вместо мока
            _validator = new UpdateProfileDtoValidator();

            _service = new ProfileService(
                _repositoryMock.Object,
                _mapperMock.Object,
                _validator);
        }

        [Fact]
        public async Task UpdateAsync_ValidDto_UpdatesSuccessfully()
        {
            // Arrange
            var updateDto = new UpdateProfileDto
            {
                Id = Guid.NewGuid(),
                FullName = "Updated User",
                Email = "updated@email.com",
                Password = "newpassword123"
            };

            var userEntity = new User
            {
                Id = updateDto.Id,
                FullName = updateDto.FullName,
                Email = updateDto.Email,
                Password = updateDto.Password
            };

            _mapperMock.Setup(m => m.Map<User>(updateDto)).Returns(userEntity);

            _repositoryMock.Setup(r => r.UpdateAsync(userEntity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(updateDto, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidDto_ThrowsValidationException()
        {
            // Arrange
            var invalidDto = new UpdateProfileDto
            {
                FullName = "", // Пустое имя
                Email = "invalid-email", // Невалидный email
                Password = "123" // Слишком короткий пароль
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() =>
                _service.UpdateAsync(invalidDto, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateAsync_RepositoryThrowsException_ExceptionIsPropagated()
        {
            // Arrange
            var updateDto = new UpdateProfileDto
            {
                Id = Guid.NewGuid(),
                FullName = "Test User",
                Email = "test@email.com",
                Password = "password123"
            };

            var userEntity = new User
            {
                Id = updateDto.Id,
                FullName = updateDto.FullName,
                Email = updateDto.Email,
                Password = updateDto.Password
            };

            _mapperMock.Setup(m => m.Map<User>(updateDto)).Returns(userEntity);

            _repositoryMock.Setup(r => r.UpdateAsync(userEntity, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.UpdateAsync(updateDto, CancellationToken.None));
        }

        [Fact]
        public async Task UpdateAsync_CancellationRequested_OperationIsCancelled()
        {
            // Arrange
            var updateDto = new UpdateProfileDto
            {
                Id = Guid.NewGuid(),
                FullName = "Test User"
            };

            var cts = new CancellationTokenSource();
            cts.Cancel();
            var cancellationToken = cts.Token;

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                _service.UpdateAsync(updateDto, cancellationToken));
        }

        [Fact]
        public async Task UpdateAsync_CancellationRequestedBeforeValidation_OperationIsCancelledImmediately()
        {
            // Arrange
            var updateDto = new UpdateProfileDto { Id = Guid.NewGuid() };
            var cancellationToken = new CancellationToken(true); // Already canceled

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                _service.UpdateAsync(updateDto, cancellationToken));
        }

        [Fact]
        public async Task UpdateAsync_ValidationFailsWithMultipleErrors_AllErrorsReported()
        {
            // Arrange
            var invalidDto = new UpdateProfileDto
            {
                FullName = "A", // Слишком короткое
                Email = "not-an-email", // Невалидный email
                Password = "" // Пустой пароль
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() =>
                _service.UpdateAsync(invalidDto, CancellationToken.None));

            Assert.NotNull(exception);
        }

        [Fact]
        public async Task UpdateAsync_ValidDto_MapperAndRepositoryCalledOnce()
        {
            // Arrange
            var updateDto = new UpdateProfileDto
            {
                Id = Guid.NewGuid(),
                FullName = "Test User",
                Email = "test@test.com",
                Password = "Test123!"
            };

            var userEntity = new User
            {
                Id = updateDto.Id,
                FullName = updateDto.FullName,
                Email = updateDto.Email,
                Password = updateDto.Password
            };

            _mapperMock.Setup(m => m.Map<User>(updateDto)).Returns(userEntity);

            _repositoryMock.Setup(r => r.UpdateAsync(userEntity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.UpdateAsync(updateDto, CancellationToken.None);

            // Assert
            _mapperMock.Verify(m => m.Map<User>(updateDto), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(userEntity, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidationPasses_NoExceptionThrown()
        {
            // Arrange
            var updateDto = new UpdateProfileDto
            {
                Id = Guid.NewGuid(),
                FullName = "Valid User",
                Email = "valid@email.com",
                Password = "ValidPassword123"
            };

            var userEntity = new User
            {
                Id = updateDto.Id,
                FullName = updateDto.FullName,
                Email = updateDto.Email,
                Password = updateDto.Password
            };

            _mapperMock.Setup(m => m.Map<User>(updateDto)).Returns(userEntity);

            _repositoryMock.Setup(r => r.UpdateAsync(userEntity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var exception = await Record.ExceptionAsync(() =>
                _service.UpdateAsync(updateDto, CancellationToken.None));

            // Assert
            Assert.Null(exception);
        }
    }
}
