using TaskApi.Controllers;
using TaskApi.Models;
using TaskApi.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace TaskApi.Tests.Controllers;

public class TaskControllerTest
{
    private readonly Mock<ITaskRepository> _repoMock;
    private readonly TasksController _controller;

    public TaskControllerTest()
    {
        _repoMock = new Mock<ITaskRepository>();
        _controller = new TasksController(_repoMock.Object);
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfTasks()
    {
        _repoMock.Setup(repo => repo.GetAll()).Returns(new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Tarea 1", Description = "Descripción de la tarea 1" },
            new TaskItem { Id = 2, Title = "Tarea 2", Description = "Descripción de la tarea 2" }
        });

        _controller.GetAll()
            .Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<TaskItem>>()
            .Which.Should().HaveCount(2);
    }

    [Fact]
    public void GetById_ReturnsOkResult_WithTaskItem()
    {
        _repoMock.Setup(repo => repo.GetById(1)).Returns(new TaskItem { Id = 1, Title = "Tarea 1", Description = "Descripción de la tarea 1" });

        _controller.GetById(1)
            .Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeOfType<TaskItem>()
            .Which.Title.Should().Be("Tarea 1");
    }   

    [Fact]
    public void GetByIdNExiste()
    {
        _repoMock.Setup(repo => repo.GetById(1)).Returns((TaskItem)null!);

        _controller.GetById(1)
            .Should().BeOfType<NotFoundResult>()
            .Which.StatusCode.Should().Be(404);
    }
}