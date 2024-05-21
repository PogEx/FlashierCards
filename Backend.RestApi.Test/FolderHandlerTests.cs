using System.Reflection;
using Backend.Common.Models;
using Backend.RestApi.ContentHandlers;
using Backend.RestApi.Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
using Moq;

namespace Backend.RestApi.Test;

public class FolderHandlerTests
{
    private FolderHandler _classUnterTest;
    
    private Mock<IConfiguration> _configMock;
    private Mock<FlashiercardsContext> _dbContextMock;
    private Mock<DbSet<Folder>> _folderSetMock;
    
    [SetUp]
    public void Setup()
    {
        _configMock = new ();
        _dbContextMock = new(_configMock.Object);
        _folderSetMock = new();
        
        _classUnterTest = new FolderHandler(() => _dbContextMock.Object);

        List<Folder> folders =
        [
            new()
            {
                FolderId = new Guid("639CB1E6-05AD-4B7B-9A1E-78F2001D879E"),
                UserId = new Guid("1BED7261-B43B-4114-A11D-4243AE899ABF"),
                Name = "Home",
                ParentId = null,
                IsRoot = true
            },
            new()
            {
                FolderId = new Guid("79C09837-4DE4-4987-A6C4-3C58D852424E"),
                UserId = new Guid("1BED7261-B43B-4114-A11D-4243AE899ABF"),
                Name = "Home",
                ParentId = new Guid("639CB1E6-05AD-4B7B-9A1E-78F2001D879E"),
                IsRoot = false
            }
        ];

        _folderSetMock = folders.AsQueryable().BuildMockDbSet();
        _dbContextMock.Setup(context => context.Folders).Returns(_folderSetMock.Object);
    }

    [Test]
    public async Task GetUserRoot_ValidUser_FindsValidRoot()
    {
        (await _classUnterTest.GetUserRoot(new Guid("1BED7261-B43B-4114-A11D-4243AE899ABF")))
            .Should().Be(new Guid("639CB1E6-05AD-4B7B-9A1E-78F2001D879E"));
    }
    
    [Test]
    public async Task GetUserRoot_ValidUser_IgnoreInvalidRoot()
    {
        (await _classUnterTest.GetUserRoot(new Guid("1BED7261-B43B-4114-A11D-4243AE899ABF")))
            .Should().NotBe(new Guid("79C09837-4DE4-4987-A6C4-3C58D852424E"));
    }
    
    [Test]
    public async Task GetUserRoot_InvalidUser_FindNoRoot()
    {
        Func<Task> act = async () => await _classUnterTest.GetUserRoot(new Guid("C434D4A5-3216-4C5A-B3E7-BC1ABA749BAA"));
        await act.Should().ThrowAsync<TargetInvocationException>();
    }
}