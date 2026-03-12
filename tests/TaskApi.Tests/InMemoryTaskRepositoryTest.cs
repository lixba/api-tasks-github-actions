using TaskApi.Repositories;
using TaskApi.Models;
using FluentAssertions;
namespace TaskApi.Tests.Repositories;
public class InMemoryTaskRepositoryTests {
    private readonly InMemoryTaskRepository _repo;
    public InMemoryTaskRepositoryTests(){
        _repo = new();
    }
    
    [Fact]
    public void Add_TareaValida_AsignaIdYRetornaTarea(){
        //Arrange        
        var tarea = new TaskItem {
            Title = "Comprar Guitarra",
            Description= "Comprar Guitarra para ser Feliz :D"
        };
        //Act
        var resultado = _repo.Add(tarea);
        //Arrange
        resultado.Id.Should().BeGreaterThan(0);
        resultado.Title.Should().Be("Comprar Guitarra");
    }

    [Fact]
    public void Add_DosTareas()
    {
        var tarea1 = new TaskItem { Title = "Tarea 1", Description = "Descripción de la tarea 1" };
        var tarea2 = new TaskItem { Title = "Tarea 2", Description = "Descripción de la tarea 2" };
        
        var r1 = _repo.Add(tarea1);
        var r2 = _repo.Add(tarea2);
        
        r2.Id.Should().Be(r1.Id + 1);
    }

    //GetAll
    [Fact]
    public void GetAll()
    {
        var resultado = _repo.GetAll();
        resultado.Should().BeEmpty();
    }

    [Fact]
    public void GetAll_DosTareas()
    {
        var tarea1 = new TaskItem { Title = "Tarea 1", Description = "Descripción de la tarea 1" };
        var tarea2 = new TaskItem { Title = "Tarea 2", Description = "Descripción de la tarea 2" };
        
        var r1 = _repo.Add(tarea1);
        var r2 = _repo.Add(tarea2);
        
        var resultado = _repo.GetAll();
        resultado.Should().HaveCount(2);
    }

    [Fact]
    public void GetByID()
    {
        var tarea1 = new TaskItem { Title = "Tarea 1", Description = "Descripción de la tarea 1" };
        
        var tareaAgregada = _repo.Add(tarea1);
        
        var resultado = _repo.GetById(tareaAgregada.Id);
        
        resultado.Should().NotBeNull();
        resultado!.Title.Should().Be("Tarea 1");
    }

    [Fact]
    public void GetById_QeuNoExiste()
    {
        var resultado = _repo.GetById(100);
        resultado.Should().BeNull();
    }

    [Fact]
    public void Update_TareaExiste_Actualizar()
    {
        var tareaOriginal = _repo.Add(new TaskItem { Title = "Tarea 1", Description = "Descripción de la tarea 1" });
        var cambioTarea = new TaskItem{ Title = "Actualizacion", Description = "Tarea 1 actualizada"};

        var resultado = _repo.Update(tareaOriginal.Id, cambioTarea);
        
        resultado.Should().NotBeNull();
        resultado!.Title.Should().Be("Actualizacion");
    }

    [Fact]
    public void UpdateNoExistente()
    {
        var resultado = _repo.Update(100, new TaskItem());
        resultado.Should().BeNull();
    }

    [Fact]
    public void Delete()
    {
        var tareaAgregada = _repo.Add(new TaskItem { Title = "Tarea 1", Description = "Descripción de la tarea 1" });
        _repo.Delete(tareaAgregada.Id);
        var resultado = _repo.GetById(tareaAgregada.Id);
        resultado.Should().BeNull();
    }

    [Fact]
    public void Delete_IdNoExiste()
    {
        var resultado = _repo.Delete(100);
        resultado.Should().BeFalse();
    }
}