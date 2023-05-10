using UnityEngine;

namespace PlayerState
{
    public class GoForDiskState : BaseState
    {
        public GoForDiskState(Player player, Intention basedOn = null) : base(player, basedOn) {}
        public override void AtStateEnter()
        {
            _subject.LookAt(new LookDirection(_subject.Game.Disk.transform));
        }
        public override void Update()
        {
            Vector3 playerPosition = _subject.transform.position;
            Vector3 diskPosition = _subject.Game.Disk.transform.position;
            _subject.MoveToPosition(_subject.Game.Disk.transform.position, _subject.DiskHandling.DiskPickRange);
            if (Vector3.Distance(playerPosition, diskPosition) < _subject.DiskHandling.DiskPickRange) 
            {
                _subject.DiskHandling.TryCatchDisk(_subject.transform.position, _subject.Game.Disk, _subject.LookingAt);
            }
        }
    }
}
