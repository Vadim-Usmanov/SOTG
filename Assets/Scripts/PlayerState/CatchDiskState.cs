namespace PlayerState
{
    public class CatchDiskState : BaseState
    {
        private Disk _disk;
        public CatchDiskState(Player player, Intention intention = null) : base(player, intention) {}
        public override void AtStateEnter()
        {
            _disk = _subject.Game.Disk;
        }
        public override void Update()
        {
            _subject.DiskHandling.TryCatchDisk(_subject.transform.position, _disk, _subject.LookingAt);
            base.Update();
        }
    }
}
