using UnityEngine;

namespace PlayerState
{
    public abstract class BaseState : State<Player>
    {
        protected bool _isDone = false;
        private Intention _basedOn;
        public BaseState(Player player, Intention basedOn) : base(player) 
        {
            _basedOn = basedOn;
        }
        public Intention BasedOn { get => _basedOn; }
        public bool IsDone { get => _isDone; }
        public virtual string Name() { return this.ToString(); }
        public override void Update()
        {
            if (_subject.transform.hasChanged) _subject.Stop();
            base.Update();
        }
    }
}
