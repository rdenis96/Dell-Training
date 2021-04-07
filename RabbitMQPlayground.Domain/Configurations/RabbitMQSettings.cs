namespace RabbitMQPlayground.Domain.Configurations
{
    public class RabbitMQSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}