namespace Domain.TelegramBotEntities;

public class TelegramBotCommand
{
    public Type Type { get; set; }

    public bool IsVisibleAsPartOfUserInterface { get; set; }

    public TelegramBotCommand(Type type, bool isVisibleAsPartOfUserInterface) 
        => (Type, IsVisibleAsPartOfUserInterface) = (type, isVisibleAsPartOfUserInterface);
}
