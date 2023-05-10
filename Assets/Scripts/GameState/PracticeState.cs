
namespace GameState
{
    public class PracticeState : BaseState, IMessageListener
    {
        private int _lastDiskHandlerId = -1;
        public PracticeState(Practice game) : base(game) { }
        public int LastDiskHandlerId { get => _lastDiskHandlerId; }
        public override void AtStateEnter()
        {
            ShakeIdlePlayers();
        }
        public override Intention PlayerIntention(int playerId)
        {
            if (_subject.Players[playerId].DiskHandling.IsHoldingDisk) return Intention.HandleDisk(this.GetType());
            else if (playerId != _lastDiskHandlerId) return Intention.WaitForPass(this.GetType());
            else return Intention.Idle(this.GetType());
        }
        public override void Report(Message message)
        {
            message.Listen(this);
        }
        public void SetDiskHandlerId(int id)
        {
            _lastDiskHandlerId = id;
            ShakeIdlePlayers();
        }
        private void ShakeIdlePlayers()
        {
            foreach(Player player in _subject.Players) player.ShakeIdleState();
        }
    }
}
