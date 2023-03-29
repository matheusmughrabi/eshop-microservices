namespace eShop.EventBus.Events.Base;

internal interface IEventService<TEventMessage> where TEventMessage : IEventMessage
{
    void Publish(TEventMessage eventMessage);
    void Consume();
}
