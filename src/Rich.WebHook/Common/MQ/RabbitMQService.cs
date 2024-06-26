using System.Text;
using RabbitMQ.Client;

namespace Rich.WebHook.Common.MQ;

public class RabbitMqService : IRabbitMqService
{
    private readonly IConnection _connection;

    public RabbitMqService(string hostName, int port, string userName, string passWord)
    {
        var factory = new ConnectionFactory()
        {
            HostName = hostName,
            Port = port,
            UserName = userName,
            Password = passWord
        };
        _connection = factory.CreateConnection();
    }

    public void PublishMessage(string exchange, string queue, string message)
    {
        using var channel = _connection.CreateModel();

        channel.QueueDeclare(queue, true, false, false, null);
        var sendBytes = Encoding.UTF8.GetBytes(message);

        //channel.BasicPublish(exchange, queueName, null, sendBytes);
        channel.BasicPublish(exchange, queue, null, sendBytes);
    }

    public void Dispose()
    {
        _connection?.Close();
    }
}