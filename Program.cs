using System;
using System.Collections.Generic;

// Singleton Pattern
public class SingletonCacheManager
{
    private static SingletonCacheManager? _instance;
    private static readonly object _lock = new();
    private readonly Dictionary<string, string> _cache = new();

    private SingletonCacheManager() { }

    public static SingletonCacheManager Instance
    {
        get
        {
            lock (_lock)
            {
                _instance ??= new SingletonCacheManager();
                return _instance;
            }
        }
    }

    public void Set(string key, string value)
    {
        _cache[key] = value;
    }

    public string? Get(string key)
    {
        return _cache.ContainsKey(key) ? _cache[key] : null;
    }
}

// Adapter Pattern
public interface IDatabaseAdapter
{
    void Connect();
    void ExecuteQuery(string query);
}

public class MySqlDatabase
{
    public void Connect() => Console.WriteLine("Підключення до MySQL");
    public void ExecuteQuery(string query) => Console.WriteLine($"MySQL виконує: {query}");
}

public class PostgreSqlDatabase
{
    public void Connect() => Console.WriteLine("Підключення до PostgreSQL");
    public void ExecuteQuery(string query) => Console.WriteLine($"PostgreSQL виконує: {query}");
}

public class SQLiteDatabase
{
    public void Connect() => Console.WriteLine("Підключення до SQLite");
    public void ExecuteQuery(string query) => Console.WriteLine($"SQLite виконує: {query}");
}

public class MySqlAdapter : IDatabaseAdapter
{
    private readonly MySqlDatabase _db = new();
    public void Connect() => _db.Connect();
    public void ExecuteQuery(string query) => _db.ExecuteQuery(query);
}

public class PostgreSqlAdapter : IDatabaseAdapter
{
    private readonly PostgreSqlDatabase _db = new();
    public void Connect() => _db.Connect();
    public void ExecuteQuery(string query) => _db.ExecuteQuery(query);
}

public class SQLiteAdapter : IDatabaseAdapter
{
    private readonly SQLiteDatabase _db = new();
    public void Connect() => _db.Connect();
    public void ExecuteQuery(string query) => _db.ExecuteQuery(query);
}

public class DataProcessor
{
    private readonly IDatabaseAdapter _adapter;
    public DataProcessor(IDatabaseAdapter adapter) => _adapter = adapter;
    public void ProcessData(string query)
    {
        _adapter.Connect();
        _adapter.ExecuteQuery(query);
    }
}

// Observer Pattern
public interface ISubscriber
{
    void Notify(string article);
}

public class NewsBlog
{
    private readonly List<ISubscriber> _subscribers = new();
    private readonly string _name;

    public NewsBlog(string name) => _name = name;

    public void Subscribe(ISubscriber subscriber) => _subscribers.Add(subscriber);
    public void Unsubscribe(ISubscriber subscriber) => _subscribers.Remove(subscriber);

    public void PublishArticle(string title)
    {
        Console.WriteLine($"Блог {_name} опублікував новину: {title}");
        foreach (var subscriber in _subscribers)
        {
            subscriber.Notify(title);
        }
    }
}

public class NewsFan : ISubscriber
{
    private readonly string _name;
    public NewsFan(string name) => _name = name;
    public void Notify(string article) => Console.WriteLine($"{_name} отримав сповіщення: нова новина - \"{article}\"");
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Singleton ===");
        var cache1 = SingletonCacheManager.Instance;
        cache1.Set("user", "Іван");
        Console.WriteLine($"Користувач з кешу: {cache1.Get("user")}");
        Console.WriteLine();

        Console.WriteLine("=== Adapter ===");
        var mySqlAdapter = new MySqlAdapter();
        var postgreSqlAdapter = new PostgreSqlAdapter();
        var sqLiteAdapter = new SQLiteAdapter();

        var processor1 = new DataProcessor(mySqlAdapter);
        processor1.ProcessData("SELECT * FROM users");

        var processor2 = new DataProcessor(postgreSqlAdapter);
        processor2.ProcessData("SELECT * FROM employees");

        var processor3 = new DataProcessor(sqLiteAdapter);
        processor3.ProcessData("SELECT * FROM products");
        Console.WriteLine();

        Console.WriteLine("=== Observer ===");
        var blog = new NewsBlog("SportLife");

        var fan1 = new NewsFan("Андрій");
        var fan2 = new NewsFan("Марія");
        var fan3 = new NewsFan("Олег");

        blog.Subscribe(fan1);
        blog.Subscribe(fan2);
        blog.Subscribe(fan3);

        blog.PublishArticle("10 найкращих голів Ліги чемпіонів");
        blog.PublishArticle("Огляд фіналу NBA 2025");
        blog.PublishArticle("Українські спортсмени на Олімпіаді");
        Console.WriteLine();

        // Демонстрація ще одного блогу про фітнес
        Console.WriteLine("=== Observer (Ще один приклад) ===");
        var fitnessBlog = new NewsBlog("FitToday");
        var fitnessFan1 = new NewsFan("Анна");
        var fitnessFan2 = new NewsFan("Богдан");

        fitnessBlog.Subscribe(fitnessFan1);
        fitnessBlog.Subscribe(fitnessFan2);

        fitnessBlog.PublishArticle("Топ 5 вправ для пресу");
        fitnessBlog.PublishArticle("Рецепти здорових сніданків");
    }
}