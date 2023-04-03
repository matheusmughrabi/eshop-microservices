namespace eShop.EventBus.Base;

public interface IEventPublisher<TEventMessage> where TEventMessage : IEventMessage
{
    void Publish(TEventMessage eventMessage);
}
