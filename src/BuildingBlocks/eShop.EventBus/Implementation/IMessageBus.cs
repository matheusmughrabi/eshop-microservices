using RabbitMQ.Client;

namespace eShop.EventBus.Implementation;

public interface IMessageBus
{
    public IConnection GetConnection();
    IModel GetChannel(IConnection connection);
}
