namespace Inheritance.MapObjects
{
    internal interface IOwner
    {
        int Owner { get; set; }
    }

    internal interface IArmy
    {
        Army Army { get; set; }
    }

    internal interface ITreasure
    {
        Treasure Treasure { get; set; }
    }

    public class Dwelling : IOwner
    {
        public int Owner { get; set; }
    }

    public class Mine : IOwner, IArmy, ITreasure
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IArmy, ITreasure
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : IArmy
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasure
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IArmy army && !player.CanBeat(army.Army))
                player.Die();

            if (mapObject is IOwner owner && !player.Dead)
                owner.Owner = player.Id;

            if (mapObject is ITreasure treasure && !player.Dead)
                player.Consume(treasure.Treasure);
        }
    }
}