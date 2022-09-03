namespace WatermarkMQ.Interfaces
{
    public interface IRabbitMQService : IAsyncDisposable
    {
        Task SendToQueue<TEvent>(TEvent eventObj) where TEvent : class, IEventModel;
    }
}
