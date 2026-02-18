using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Application.Common.Models;

/// <summary>
/// Pagination desteği - Büyük listeleri sayfa sayfa döndürür
/// Performans için önemli (1000 task varsa hepsini göndermemek)
/// </summary>
public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = count;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    /// <summary>
    /// IQueryable'dan sayfalı liste oluştur
    /// Skip() ve Take() ile sadece o sayfadaki kayıtları getirir
    /// </summary>
    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    /// <summary>
    /// IQueryable'dan senkron olarak sayfalı liste oluştur
    /// Memory'de olan veriler için (zaten List olmuş)
    /// </summary>
    public static PaginatedList<T> Create(
        IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}