
namespace Backend.Restart.Models;

public class GhostContainer<T>
{
    public T Payload { get; set; }
    public bool GhostComponent { get; set; } = false;
}