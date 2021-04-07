namespace RabbitMQPlayground.Domain.Users
{
    public interface IUser
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}