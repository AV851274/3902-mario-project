public class PreviousItemCommand : ICommand
{
    private IItem item;

    public PreviousItemCommand(IItem item)
    {
        this.item = item;
    }

    public void Execute()
    {
        item.prevSprite();
    }
}
