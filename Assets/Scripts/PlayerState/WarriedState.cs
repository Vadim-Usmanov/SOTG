
namespace PlayerState
{
    public class WarriedState : BaseState
    {
        public WarriedState(Player player, Intention basedOn = null) : base(player, basedOn = null) {}
        public override void Update()
        {
            if (_subject.transform.hasChanged) _subject.Stop();
            _subject.CheckIntention();
        }
    }
}