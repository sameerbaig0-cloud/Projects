using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace ServicePlatform.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            try
            {
                var count = await source.CountAsync();
            
            // Ensure pageIndex is within valid range
            pageIndex = Math.Max(1, pageIndex);

            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

                if (items == null)
            {
                // If no items are returned, handle it accordingly
                // You may return an empty list or throw an exception
                items = new List<T>();
                // Alternatively, throw an exception:
                // throw new InvalidOperationException("Items cannot be null.");

            }

            return new PaginatedList<T>(items, count, pageIndex, pageSize);

            }   
            catch (SqlNullValueException ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.ToString(), "An exception occurred while processing the request.");

                // Handle the exception by returning an empty list or rethrowing it, depending on your requirements
                return new PaginatedList<T>(new List<T>(), 0, pageIndex, pageSize);
                // Alternatively, rethrow the exception:
                // throw;
            }

        }

		internal static Task<object?> CreateAsync<TEntity>(IQueryable<TEntity> entities, object value1, object value2) where TEntity : class
		{
			throw new NotImplementedException();
		}
	}
}
