
public abstract class State<T>
{
    protected T _subject;
    public State(T subject)
    {
        _subject = subject;
    }
    public virtual void AtStateEnter() {}
    public virtual void AtStateExit() {}
    public virtual void Update() {}
}
