using UnityEngine;

namespace PlayerState
{
    public class GoToPositionState : BaseState
    {
        private const float _defaultMaxDistance = 1.5f; //FIXME: Вывести в интспектор в Defaults
        private float _maxDistance;
        private float _lastDistance;
        private float _startDistance;
        private Vector3 _position;
        private LookDirection _lookAt;
        public GoToPositionState(
            Player player,
            Vector3 position,
            LookDirection lookAt,
            Intention basedOn = null,            
            float maxDistance = _defaultMaxDistance
            ) : base(player, basedOn)
        {
            _position = position;
            _maxDistance = maxDistance;
            _lookAt = lookAt;
        }
        public GoToPositionState(
            Player player,
            Intention basedOn,
            float maxDistance = _defaultMaxDistance
            ) : this(player, basedOn.Position, basedOn.LookAt, basedOn) {}
        public override void AtStateEnter()
        {
            _subject.LookAt(new LookDirection(_position));
            _lastDistance = Vector3.Distance(_subject.transform.position, _position);
            _startDistance = _lastDistance;
        }
        public override void Update()
        {
            _subject.MoveToPosition(_position, _maxDistance);
            if (_isDone) return;
            if (IsPositionReached(Vector3.Distance(_subject.transform.position, _position)))
            {
                _subject.LookAt(_lookAt);
                _isDone = true;
            }
        }
        private bool IsPositionReached(float distance)
        {
            if (distance <= _maxDistance) return true;
            if (distance > _lastDistance && distance < _startDistance / 2) return true;
            _lastDistance = distance;
            return false;
        }
    }
}
