using Xunit;

public class BookstoreTests
{
    private static Bookstore Create()
    {
        var s = new Bookstore();
        s.AddBook("Kobzar", "Shevchenko", 100, 5);
        return s;
    }

    // ---------- AddBook ----------
    [Fact]
    public void AddBook_AddsBook()
    {
        var book = Create().SearchBook("Kobzar")!;
        Assert.Equal("Shevchenko", book.Author);
        Assert.Equal(100, book.Price);
        Assert.Equal(5, book.Quantity);
    }

    [Fact]
    public void AddBook_Duplicate_IncreasesQuantity()
    {
        var s = Create();
        s.AddBook("Kobzar", "Shevchenko", 100, 3);
        Assert.Equal(8, s.SearchBook("Kobzar")!.Quantity);
    }

    [Fact]
    public void AddBook_NegativeQuantity_Throws() =>
        Assert.Throws<ArgumentException>(() => Create().AddBook("A", "B", 10, -1));

    [Fact]
    public void AddBook_NegativePrice_Throws() =>
        Assert.Throws<ArgumentException>(() => Create().AddBook("A", "B", -10, 1));

    // ---------- RemoveBook ----------
    [Fact]
    public void RemoveBook_RemovesBook()
    {
        var s = Create();
        Assert.True(s.RemoveBook("Kobzar"));
        Assert.Null(s.SearchBook("Kobzar"));
    }

    [Fact]
    public void RemoveBook_NotExisting_ReturnsFalse() =>
        Assert.False(Create().RemoveBook("Unknown"));

    // ---------- SearchBook ----------
    [Fact]
    public void SearchBook_NotExisting_ReturnsNull() =>
        Assert.Null(Create().SearchBook("Unknown"));

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void SearchBook_EmptyOrNull_ReturnsNull(string? title) =>
        Assert.Null(Create().SearchBook(title!));

    // ---------- PurchaseBook ----------
    [Fact]
    public void PurchaseBook_DecreasesQuantity_ReturnsTotal()
    {
        var s = Create();
        Assert.Equal(200, s.PurchaseBook("Kobzar", 2));
        Assert.Equal(3, s.SearchBook("Kobzar")!.Quantity);
    }

    [Fact]
    public void PurchaseBook_MoreThanInStock_Throws() =>
        Assert.Throws<InvalidOperationException>(() => Create().PurchaseBook("Kobzar", 6));

    [Fact]
    public void PurchaseBook_NotExisting_Throws() =>
        Assert.Throws<KeyNotFoundException>(() => Create().PurchaseBook("Unknown", 1));

    [Fact]
    public void PurchaseBook_ZeroStock_Throws()
    {
        var s = new Bookstore();
        s.AddBook("Empty", "Author", 50, 0);
        Assert.Throws<InvalidOperationException>(() => s.PurchaseBook("Empty", 1));
    }

    // ---------- InventoryValue ----------
    [Fact]
    public void InventoryValue_SumsAllBooks()
    {
        var s = Create();                    // 100 * 5 = 500
        s.AddBook("Tiger Hunters", "Bagryanyi", 50, 4); // 50 * 4 = 200
        Assert.Equal(700, s.InventoryValue());
    }

    [Fact]
    public void InventoryValue_Empty_IsZero() =>
        Assert.Equal(0, new Bookstore().InventoryValue());
}
