using SimpleEventBus.Events;

public class PlayerWonEvent : EventBase
{
    public Player Player { get; }

    public PlayerWonEvent(Player player)
    {
        Player = player;
    }
}