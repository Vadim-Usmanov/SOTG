using UnityEngine;

namespace GameState
{
    public class SpawnState : BaseState
    {
        private Field _field;
        private GameController _gameController;
        public SpawnState(Practice game) : base(game)
        {
            _field = GameObject.FindWithTag("Field").GetComponent<Field>();
            _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        }
        public override void Update()
        {
            if (_subject.Disk == null) SpawnDisk();
            else if (_subject.Players.Count < _subject.RequiredPlayers) SpawnPlayer();
        }
        public override Intention PlayerIntention(int playerId)
        {
            return Intention.Idle(this.GetType());
        }
        private void SpawnDisk()
        {
            _subject.Disk = _field.SpawnDisk();
        }
        private void SpawnPlayer()
        {
            Player player = _field.SpawnPlayer();
            player.Color = _gameController.RandomNeutralColor();
            player.Name = _gameController.RandomName();
            player.Game = _subject;
        }
    }
}
