using SimpleEventBus.Events;
using PlayerEntity;

namespace Events
{
    public class PlayerWonEvent : EventBase
    {
        public Player Player { get; }

        public PlayerWonEvent(Player player)
        {
            Player = player;
        }
    }
}