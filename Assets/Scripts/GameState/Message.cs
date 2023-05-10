namespace GameState
{
    public class Message
    {
        private int _handlerId;
        public void Listen(IMessageListener listener)
        {
            listener.SetDiskHandlerId(_handlerId);
        }
        public void SetDiskHandlerId(int id)
        {
            _handlerId = id;
        }
    }
}
