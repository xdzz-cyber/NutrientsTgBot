namespace Application.Interfaces;

public interface ICommand
{
    public string Username { get; set; }

    public long ChatId { get; set; }
}
