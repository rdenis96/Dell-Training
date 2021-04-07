namespace RabbitMQPlayground.Domain.Books
{
    public interface IBook
    {
        string Date { get; set; }

        int PagesCount { get; set; }

        string Title { get; set; }

        string Category { get; set; }
    }
}