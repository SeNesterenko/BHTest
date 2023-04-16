using SimpleEventBus.Events;

namespace Events
{
    public class PlayerWonEvent : EventBase
    {
        public Player.Player Player { get; }

        public PlayerWonEvent(Player.Player player)
        {
            Player = player;
        }
    }
}