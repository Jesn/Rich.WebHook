namespace Rich.WebHook.Common.MQ;

public interface IRabbitMqService
{
    void PublishMessage(string exchange, string queueName, string message);
}