using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class TagServiceTests
    {
        private Mock<ITagsRepository> _mockTagsRepository;
        private Tag _tag;
        private TagsDomainModel _tagDomainModel;
        private Mock<IMovieTagService> _mockMovieTagService;
        private MovieTagsDomainModel _movieTagsDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _tag = new Tag()
            {
                Id = 1,
                Name = "nazivTaga"
            };

            _tagDomainModel = new TagsDomainModel()
            {
                Id = 1,
                Name = "nazivTaga"
            };

            _movieTagsDomainModel = new MovieTagsDomainModel()
            {
                MovieId = Guid.NewGuid(),
                TagId = 1
            };


            List<Tag> tagModelsList = new List<Tag>();
            tagModelsList.Add(_tag);
            IEnumerable<Tag> tags = tagModelsList;
            Task<IEnumerable<Tag>> responseTask = Task.FromResult(tags);

            _mockTagsRepository = new Mock<ITagsRepository>();

            List<MovieTagsDomainModel> movieTagsList = new List<MovieTagsDomainModel>();
            movieTagsList.Add(_movieTagsDomainModel);
            IEnumerable<MovieTagsDomainModel> movieTags = movieTagsList;
            Task<IEnumerable<MovieTagsDomainModel>> movieTagsResponseTask = Task.FromResult(movieTags);

            _mockMovieTagService = new Mock<IMovieTagService>();

        }

        [TestMethod]
        public void TagService_GetAllAsync_ReturnsAllTags()
        {
            //Arrange

            List<Tag> tagModelsList = new List<Tag>();
            tagModelsList.Add(_tag);
            IEnumerable<Tag> tags = tagModelsList;
            Task<IEnumerable<Tag>> responseTask = Task.FromResult(tags);
            int expectedCount = 1;

            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.GetAll()).Returns(responseTask);


            //Act
            var result = tagService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(TagsDomainModel));
            Assert.AreEqual(_tag.Id, result[0].Id);
        }

        [TestMethod]
        public void TagService_GetAllAync_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange

            IEnumerable<Tag> tags = null;
            Task<IEnumerable<Tag>> responseTask = Task.FromResult(tags);

            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.GetAll()).Returns(responseTask);

            //Act
            var result = tagService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TagService_AddTag_ReturnsInsertedTag()
        {
            //Arrange
            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.Insert(It.IsAny<Tag>())).Returns(_tag);

            //Act
            var result = tagService.AddTag(_tagDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TagsDomainModel));
            Assert.AreEqual(result.Id, _tagDomainModel.Id);

        }

        [TestMethod]
        public void TagService_AddTag_RepositoryReturnsNull_ReturnNull()
        {
            //Arrange
            Tag nullTag = null;
            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.Insert(It.IsAny<Tag>())).Returns(nullTag);

            //Act
            var result = tagService.AddTag(_tagDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void TagService_AddTag_ThrowsDbUpdateException()
        {
            //Arrange
            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.Insert(It.IsAny<Tag>())).Throws(new DbUpdateException());

            //Act
            var result = tagService.AddTag(_tagDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void TagService_GetTagByIdAsync_ReturnsTag()
        {
            //Arrange
            Task<Tag> tag = Task.FromResult(_tag);
            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(tag);

            //Act
            var result = tagService.GetTagByIdAsync(_tag.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TagsDomainModel));
            Assert.AreEqual(result.Id, _tagDomainModel.Id);
        }

        [TestMethod]
        public void TagService_GetTagByIdAsync_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Tag nullTag = null;
            Task<Tag> tag = Task.FromResult(nullTag);
            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(tag);

            //Act
            var result = tagService.GetTagByIdAsync(_tag.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TagService_DeleteTag_ReturnsTag()
        {
            //Arrange
            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_tag);

            //Act
            var result = tagService.DeleteTag(_tag.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TagsDomainModel));
            Assert.AreEqual(result.Id, _tagDomainModel.Id);
        }

        [TestMethod]
        public void TagService_DeleteTag_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Tag nullTag = null;
            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(nullTag);

            //Act
            var result = tagService.DeleteTag(_tag.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TagService_GetTagByTagName_ReturnsTag()
        {
            //Arrange
            Task<Tag> tag = Task.FromResult(_tag);
            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.GetTagByTagName(It.IsAny<string>())).Returns(tag);

            //Act
            var result = tagService.GetTagByTagName(_tag.Name).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TagsDomainModel));
            Assert.AreEqual(result.Id, _tagDomainModel.Id);
            Assert.AreEqual(result.Name, _tagDomainModel.Name);
        }

        [TestMethod]
        public void TagService_GetTagByTagName_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Tag nullTag = null;
            Task<Tag> tag = Task.FromResult(nullTag);
            TagService tagService = new TagService(_mockTagsRepository.Object, _mockMovieTagService.Object);

            _mockTagsRepository.Setup(x => x.GetTagByTagName(It.IsAny<string>())).Returns(tag);

            //Act
            var result = tagService.GetTagByTagName(_tag.Name).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }
    }
}
