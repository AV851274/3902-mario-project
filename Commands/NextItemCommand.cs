public class NextItemCommand : ICommand
{
    private IItem item;

    public NextItemCommand(IItem item)
    {
        this.item = item;
    }

    public void Execute()
    {
        item.nextSprite();
    }
}
