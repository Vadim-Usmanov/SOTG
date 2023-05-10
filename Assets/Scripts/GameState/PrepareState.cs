using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameState
{
    public class PrepareState : BaseState
    {
        private Field _field;
        private int _closestToDiskId = -1;
        public PrepareState(Practice game) : base(game) 
        { 
            _field = GameObject.FindWithTag("Field").GetComponent<Field>();
        }
        public override void AtStateEnter()
        {
            if (_subject.ClaimedPracticeArea == null) ClaimPracticeArea();
            if (_subject.Disk.IsGrounded) _closestToDiskId = FindPlayerClosestToDiskId();
            foreach(Player player in _subject.Players) player.ShakeIdleState();
        }
        public override void Update()
        {
            if (_subject.ClaimedPracticeArea == null) return;
            if (_closestToDiskId == -1 && _subject.Disk.IsGrounded) _closestToDiskId = FindPlayerClosestToDiskId();
        }
        public override Intention PlayerIntention(int playerId)
        {
            if (_subject.Disk.IsGrounded && playerId == _closestToDiskId) return Intention.PickUpDisk(this.GetType());
            else 
            {
                LookDirection lookAt = new LookDirection(PositionFor(_subject.GetPartnerFor(playerId).Id));
                return Intention.GoToPosition(this.GetType(), PositionFor(playerId), lookAt);
            }
        }
        private void ClaimPracticeArea()
        {
            try
            {
                IList<PracticeArea> list = _field.PracticeAreas;
                _subject.ClaimedPracticeArea = list.First<PracticeArea>(area => !area.IsClaimed);
                _subject.ClaimedPracticeArea.Claim();
            }
            catch (InvalidOperationException) 
            {
                _subject.ClaimedPracticeArea = null;
            }
        }
        private int FindPlayerClosestToDiskId()
        {
            IList<Player> players = _subject.Players;
            Dictionary<int, float> distances = new Dictionary<int, float>(players.Count);
            for (int i = 0; i < players.Count; i++)
            {
                distances[i] = Vector3.Distance(players[i].transform.position, _subject.Disk.transform.position);
            }
            return distances.Aggregate((left, right) => left.Value < right.Value ? left : right).Key;
        }
        private Vector3 PositionFor(int positionNumber)
        {
            switch (positionNumber)
            {
                case 0: return _subject.ClaimedPracticeArea.LeftSpot.transform.position;
                case 1: return _subject.ClaimedPracticeArea.RightSpot.transform.position;
                default: throw new ArgumentOutOfRangeException($"positionNumber {positionNumber} does not exist");
            }
        }
    }
}
