namespace GameState
{
    public abstract class BaseState : State<Practice>
    {
        public BaseState(Practice game) : base(game) {}
        public abstract Intention PlayerIntention(int playerId);
        public virtual void Report(Message message) {}
    }
}
