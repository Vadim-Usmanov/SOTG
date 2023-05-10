namespace PlayerState
{
    public class WaitForPassState : BaseState
    {
        public WaitForPassState(Player player, Intention intention = null) : base(player, intention) {}
        public override void AtStateEnter()
        {
            _subject.Game.AddPassOption(new PassOption(_subject.Id, _subject.transform.position));
        }
        public override void AtStateExit()
        {
            _subject.Game.RemovePassOptions(_subject.Id);
        }
    }
}
