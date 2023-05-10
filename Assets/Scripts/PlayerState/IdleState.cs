using UnityEngine;

namespace PlayerState
{
    public class IdleState : BaseState
    {
        public IdleState(Player player, Intention basedOn = null) : base(player, basedOn) {}
    }
}
