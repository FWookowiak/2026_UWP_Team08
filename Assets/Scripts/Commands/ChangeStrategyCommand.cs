public class ChangeStrategyCommand : ICommand
{
    private readonly TowerBase tower;
    private readonly TargetingMode newMode;
    private readonly TargetingMode previousMode;

    public string Description => $"Change strategy to {newMode} on {tower.name}";

    public ChangeStrategyCommand(TowerBase tower, TargetingMode newMode)
    {
        this.tower = tower;
        this.newMode = newMode;
        this.previousMode = tower.CurrentTargetingMode;
    }

   public void Execute()
   {
       if (tower == null) return;
       tower.SetTargetingMode(newMode);
   }
   
   public void Undo()
   {
       if (tower == null) return;
       tower.SetTargetingMode(previousMode);
   }
}