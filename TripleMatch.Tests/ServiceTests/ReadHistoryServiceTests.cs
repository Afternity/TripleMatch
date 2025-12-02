using AutoMapper;
using Moq;
using TripleMatch.Application.Features;
using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Shered.Contracts.VMs;
using TripleMatch.Shered.Contracts.VMs.LookupDTOs;

namespace TripleMatch.Tests.ServiceTests
{
    public class ReadHistoryServiceTests
    {
        private readonly Mock<IReadHistoryRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ReadHistoryService _service;

        public ReadHistoryServiceTests()
        {
            _repositoryMock = new Mock<IReadHistoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new ReadHistoryService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task BestUserHistory_ValidUser_ReturnsBestHistory()
        {
            // Arrange
            var userProfile = new UserProfileVm
            {
                Id = Guid.NewGuid(),
                FullName = "Test User",
                Email = "test@email.com"
            };
            var userEntity = new User
            {
                Id = userProfile.Id,
                FullName = userProfile.FullName,
                Email = userProfile.Email
            };
            var historyEntity = new History
            {
                Score = 100,
                DateTime = DateTime.UtcNow,
                UserId = userProfile.Id,
                User = userEntity
            };
            var expectedVm = new BestUserHistoryVm
            {
                Score = 100,
                DateTime = historyEntity.DateTime
            };

            _mapperMock.Setup(m => m.Map<User>(userProfile)).Returns(userEntity);
            _repositoryMock.Setup(r => r.BestUserHistory(userEntity, It.IsAny<CancellationToken>()))
                .ReturnsAsync(historyEntity);
            _mapperMock.Setup(m => m.Map<BestUserHistoryVm>(historyEntity)).Returns(expectedVm);

            // Act
            var result = await _service.BestUserHistory(userProfile, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedVm.Score, result.Score);
            Assert.Equal(expectedVm.DateTime, result.DateTime);
            _repositoryMock.Verify(r => r.BestUserHistory(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task BestUserHistory_UserWithoutHistory_ReturnsNull()
        {
            // Arrange
            var userProfile = new UserProfileVm { Id = Guid.NewGuid() };
            var userEntity = new User { Id = userProfile.Id };

            _mapperMock.Setup(m => m.Map<User>(userProfile)).Returns(userEntity);
            _repositoryMock.Setup(r => r.BestUserHistory(userEntity, It.IsAny<CancellationToken>()))
                .ReturnsAsync((History?)null);

            // Act
            var result = await _service.BestUserHistory(userProfile, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetFiveBestHistoriesScore_ReturnsCorrectRanking()
        {
            // Arrange
            var user1 = new User { Id = Guid.NewGuid(), FullName = "User One" };
            var user2 = new User { Id = Guid.NewGuid(), FullName = "User Two" };
            var user3 = new User { Id = Guid.NewGuid(), FullName = "User Three" };

            var entities = new List<History>
            {
                new() { Score = 500, UserId = user1.Id, User = user1, DateTime = DateTime.UtcNow.AddHours(-1) },
                new() { Score = 400, UserId = user2.Id, User = user2, DateTime = DateTime.UtcNow.AddHours(-2) },
                new() { Score = 300, UserId = user3.Id, User = user3, DateTime = DateTime.UtcNow.AddHours(-3) },
                new() { Score = 200, UserId = user1.Id, User = user1, DateTime = DateTime.UtcNow.AddHours(-4) },
                new() { Score = 100, UserId = user2.Id, User = user2, DateTime = DateTime.UtcNow.AddHours(-5) }
            };

            var dtos = new List<FiveBestHistoriesScoreLookupDto>
            {
                new() { Score = 500, DateTime = DateTime.UtcNow.AddHours(-1), FullName = "User One" },
                new() { Score = 400, DateTime = DateTime.UtcNow.AddHours(-2), FullName = "User Two" },
                new() { Score = 300, DateTime = DateTime.UtcNow.AddHours(-3), FullName = "User Three" },
                new() { Score = 200, DateTime = DateTime.UtcNow.AddHours(-4), FullName = "User One" },
                new() { Score = 100, DateTime = DateTime.UtcNow.AddHours(-5), FullName = "User Two" }
            };

            _repositoryMock.Setup(r => r.GetFiveBestHistoriesScore(It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IList<FiveBestHistoriesScoreLookupDto>>(entities))
                .Returns(dtos);

            // Act
            var result = await _service.GetFiveBestHistoriesScore(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Histories.Count);

            // Используем ToList() для индексации или ElementAt()
            var historiesList = result.Histories.ToList();

            // Проверяем ранги
            for (int i = 0; i < historiesList.Count; i++)
            {
                Assert.Equal(i + 1, historiesList[i].Rank);
            }
        }

        [Fact]
        public async Task GetFiveBestHistoriesScore_LessThanFiveRecords_ReturnsAll()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), FullName = "Test User" };
            var entities = new List<History>
            {
                new() { Score = 500, UserId = user.Id, User = user, DateTime = DateTime.UtcNow.AddDays(-1) },
                new() { Score = 400, UserId = user.Id, User = user, DateTime = DateTime.UtcNow }
            };

            var dtos = new List<FiveBestHistoriesScoreLookupDto>
            {
                new() { Score = 500, DateTime = DateTime.UtcNow.AddDays(-1), FullName = "Test User" },
                new() { Score = 400, DateTime = DateTime.UtcNow, FullName = "Test User" }
            };

            _repositoryMock.Setup(r => r.GetFiveBestHistoriesScore(It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IList<FiveBestHistoriesScoreLookupDto>>(entities))
                .Returns(dtos);

            // Act
            var result = await _service.GetFiveBestHistoriesScore(CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Histories.Count);

            // Используем ToList() или ElementAt()
            var historiesList = result.Histories.ToList();
            Assert.Equal(1, historiesList[0].Rank);
            Assert.Equal(2, historiesList[1].Rank);
        }

        [Fact]
        public async Task GetFiveBestHistoriesScore_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            var entities = new List<History>();
            var dtos = new List<FiveBestHistoriesScoreLookupDto>();

            _repositoryMock.Setup(r => r.GetFiveBestHistoriesScore(It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IList<FiveBestHistoriesScoreLookupDto>>(entities))
                .Returns(dtos);

            // Act
            var result = await _service.GetFiveBestHistoriesScore(CancellationToken.None);

            // Assert
            Assert.Empty(result.Histories);
        }

        [Fact]
        public async Task GetUserHistories_ValidUser_ReturnsUserHistories()
        {
            // Arrange
            var userProfile = new UserProfileVm { Id = Guid.NewGuid() };
            var userEntity = new User { Id = userProfile.Id };
            var entities = new List<History>
            {
                new() { Score = 100, DateTime = DateTime.UtcNow.AddDays(-1), UserId = userProfile.Id },
                new() { Score = 200, DateTime = DateTime.UtcNow, UserId = userProfile.Id }
            };

            var dtos = new List<UserHistoriesLookupDto>
            {
                new() { Score = 100, DateTime = DateTime.UtcNow.AddDays(-1) },
                new() { Score = 200, DateTime = DateTime.UtcNow }
            };

            _mapperMock.Setup(m => m.Map<User>(userProfile)).Returns(userEntity);
            _repositoryMock.Setup(r => r.GetUserHistories(userEntity, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);
            _mapperMock.Setup(m => m.Map<IList<UserHistoriesLookupDto>>(entities))
                .Returns(dtos);

            // Act
            var result = await _service.GetUserHistories(userProfile, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Histories.Count);

            // Используем ToList() для доступа к элементам
            var historiesList = result.Histories.ToList();
            Assert.Equal(100, historiesList[0].Score);
            Assert.Equal(200, historiesList[1].Score);
        }

        [Fact]
        public async Task UserLastHistory_ValidUser_ReturnsLastHistory()
        {
            // Arrange
            var userProfile = new UserProfileVm { Id = Guid.NewGuid() };
            var userEntity = new User { Id = userProfile.Id };
            var historyEntity = new History
            {
                Score = 150,
                DateTime = DateTime.UtcNow,
                UserId = userProfile.Id,
                User = userEntity
            };
            var expectedVm = new UserLastHistoryVm
            {
                Score = 150,
                DateTime = DateTime.UtcNow
            };

            _mapperMock.Setup(m => m.Map<User>(userProfile)).Returns(userEntity);
            _repositoryMock.Setup(r => r.UserLastHistory(userEntity, It.IsAny<CancellationToken>()))
                .ReturnsAsync(historyEntity);
            _mapperMock.Setup(m => m.Map<UserLastHistoryVm>(historyEntity)).Returns(expectedVm);

            // Act
            var result = await _service.UserLastHistory(userProfile, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedVm.Score, result.Score);
            Assert.Equal(expectedVm.DateTime, result.DateTime);
            _repositoryMock.Verify(r => r.UserLastHistory(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UserLastHistory_UserWithoutHistory_ReturnsNull()
        {
            // Arrange
            var userProfile = new UserProfileVm { Id = Guid.NewGuid() };
            var userEntity = new User { Id = userProfile.Id };

            _mapperMock.Setup(m => m.Map<User>(userProfile)).Returns(userEntity);
            _repositoryMock.Setup(r => r.UserLastHistory(userEntity, It.IsAny<CancellationToken>()))
                .ReturnsAsync((History?)null);

            // Act
            var result = await _service.UserLastHistory(userProfile, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
