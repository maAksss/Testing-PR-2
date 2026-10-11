public class Book
{
    public string Title = "";
    public string Author = "";
    public double Price;
    public int Quantity;
}

public class Bookstore
{
    private readonly Dictionary<string, Book> _books = new();

    public void AddBook(string title, string author, double price, int quantity)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Порожня назва");
        if (price < 0 || quantity < 0) throw new ArgumentException("Від'ємна ціна або кількість");

        if (_books.TryGetValue(title, out var book))
            book.Quantity += quantity; // дублікат: збільшуємо кількість
        else
            _books[title] = new Book { Title = title, Author = author, Price = price, Quantity = quantity };
    }

    public bool RemoveBook(string title) => title != null && _books.Remove(title);

    public Book? SearchBook(string title) =>
        title != null && _books.TryGetValue(title, out var book) ? book : null;

    public double PurchaseBook(string title, int quantity)
    {
        var book = SearchBook(title) ?? throw new KeyNotFoundException("Книги немає в інвентарі");
        if (quantity <= 0) throw new ArgumentException("Кількість має бути > 0");
        if (quantity > book.Quantity) throw new InvalidOperationException("Недостатньо книг на складі");

        book.Quantity -= quantity;
        return book.Price * quantity;
    }

    public double InventoryValue() => _books.Values.Sum(b => b.Price * b.Quantity);
}
