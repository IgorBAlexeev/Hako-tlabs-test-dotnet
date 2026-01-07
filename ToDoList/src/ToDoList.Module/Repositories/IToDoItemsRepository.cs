using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApp.ToDoList.Entity;

namespace TestApp.ToDoList.Repository
{
  /// <summary>
  /// Repository interface for managing to-do items.
  /// </summary>
  public interface IToDoItemsRepository
  {
    /// <summary>
    /// Gets all to-do items from DB.
    /// </summary>
    /// <returns></returns>
    ICollection<ToDoItem> GetAllItems();
    /// <summary>
    /// Gets all to-do items from DB as queryable.
    /// </summary>
    /// <returns></returns>
    IQueryable<ToDoItem> GetAllItemsAsQueryable();
    /// <summary>
    /// Gets all to-do items from DB async.
    /// </summary>
    /// <returns></returns>
    Task<ICollection<ToDoItem>> GetAllItemsAsync();
    /// <summary>
    /// Gets all page items from DB async.
    /// </summary>
    /// <returns></returns>
    Task<ICollection<ToDoItem>> GetPageItemsAsync(int skip, int pageSize);
    /// <summary>
    /// Gets single to-do item by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    ToDoItem GetItemById(int id);
    /// <summary>
    /// Creates a new to-do item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    ToDoItem Create(ToDoItem item);
    /// <summary>
    /// Updates an existing to-do item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    ToDoItem Update(ToDoItem item);
    /// <summary>
    /// Delete an existing to-do item.
    /// </summary>
    /// <param name="id"></param>
    void Delete(int id);
  }
}