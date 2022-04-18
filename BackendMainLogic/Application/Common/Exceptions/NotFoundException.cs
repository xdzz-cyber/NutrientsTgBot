namespace Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key) : base($"Entity {name}, key - {key} not found.")
    {
        
    }
}
