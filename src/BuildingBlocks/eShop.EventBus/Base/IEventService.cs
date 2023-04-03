namespace eShop.EventBus.Base;

internal interface IEventService<TEventMessage> where TEventMessage : IEventMessage
{
    void Publish(TEventMessage eventMessage);
    void Consume();
}
