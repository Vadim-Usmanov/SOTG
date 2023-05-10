using System;
using System.Linq;
using UnityEngine;

namespace PlayerState
{
    public class HandleDiskState : BaseState
    {
        private Coroutine _stanceTransit;
        public HandleDiskState(Player player, Intention intention = null) : base(player, intention) {}
        public override void AtStateEnter()
        {
            GameState.Message message = new GameState.Message();
            message.SetDiskHandlerId(_subject.Id);
            _subject.Game.Report(message);
        }
        public override void Update()
        {
            if (!_subject.DiskHandling.IsHoldingDisk) return;
            if (_subject.transform.hasChanged) _subject.Stop();
            else if (_subject.DiskHandling.IsTransitStance || _subject.DiskHandling.IsThrowing) return;
            else if (_subject.DiskHandling.Stance == Concept.Stance.Base) ChangeStance(OptimalStance());
            else if (_subject.Game.PassOptions.Any()) ThrowDiskAt(LookForOptions());
        }
        public override void AtStateExit()
        {
            if (_subject.DiskHandling.IsHoldingDisk) throw new InvalidOperationException("Cannot leave HandleDiskState with disk in hands");
        }
        private PassOption LookForOptions()
        {
            if (!_subject.Game.PassOptions.Any()) throw new System.InvalidOperationException("Options list is empty");
            return _subject.Game.PassOptions.First();
        }
        private void ThrowDiskAt(Vector3 position)
        {
            _subject.DiskHandling.ThrowDiskAt(position);
        }
        private void ThrowDiskAt(Player player)
        {
            ThrowDiskAt(player.transform.position);
        }
        private void ThrowDiskAt(PassOption passOption)
        {
            ThrowDiskAt(passOption.Position);
        }
        private void ChangeStance(Concept.Stance newStance)
        {
            _subject.DiskHandling.ChangeStance(newStance);
        }
        private Concept.Stance OptimalStance()
        {
            if (UnityEngine.Random.value >= 0.5f) return Concept.Stance.Backhand;
            return Concept.Stance.Forehand;
        }
    }
}
