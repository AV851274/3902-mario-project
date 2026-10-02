using Mario.Interfaces;

namespace Mario.Commands;

public class PreviousBlockCommand : ICommand
{
    private IBlock block;

    public PreviousBlockCommand(IBlock block)
    {
        this.block = block;
    }

    public void Execute()
    {
        block.prevSprite();
    }
}
