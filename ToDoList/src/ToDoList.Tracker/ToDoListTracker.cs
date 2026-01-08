using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TestApp.ToDoList.Entity;
using TestApp.ToDoList.Module;
using TestApp.ToDoList.Repository;
using ToDoList.Tracker;

namespace TestApp.ToDoList.Tracker
{
  /// <summary>
  /// Implementation of the to-do list tracking.
  /// </summary>
  public class ToDoListTracker : IToDoListTracker
  {
    private readonly IToDoItemsRepository repository;
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="repository"></param>
    public ToDoListTracker(IToDoItemsRepository repository)
    {
      this.repository = repository;
    }

    /// <inheritdoc/>
    public ToDoItem AddItem(string title)
    {
      // Implementation for adding a to-do item
      var newItem = new ToDoItem { Title = title, IsCompleted = false };
      newItem = repository.Create(newItem);
      return newItem;
    }
    /// <inheritdoc/>
    public ToDoItem AddItem(string title, List<string> tags)
    {
      // Implementation for adding a to-do item
      var newItem = new ToDoItem { Title = title, IsCompleted = false, Tags = tags };
      newItem = repository.Create(newItem);
      return newItem;
    }
    /// <inheritdoc/>
    public ToDoItem RemoveItem(int id)
    {
      var item = repository.GetItemById(id);
      if (null == item)
        throw new ArgumentException($"Item with id {id} not found");

      repository.Delete(id);
      return item;
    }
    /// <inheritdoc/>
    public ToDoItem GetItem(int id)
    {
      // Implementation for getting a specific to-do item
      var item = repository.GetItemById(id);
      if (null == item)
        throw new ArgumentException($"Item with id {id} not found");
      return item;
    }
    /// <inheritdoc/>
    public IEnumerable<ToDoItem> GetAllItems()
    {
      return repository.GetAllItems().ToList();
    }
    /// <inheritdoc/>
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetAllItemsAsync()
    {
      var items = await repository.GetAllItemsAsync();
      return items.ToList();
    }
    /// <inheritdoc/>
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetOnePageAsync(int skip, int pageSize)
    {
      var items = await repository.GetPageItemsAsync(skip, pageSize);
      return items.ToList();
    }
    /// <inheritdoc/>
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetFilteredAndSortedItemsAsync(Func<ToDoItem, bool> filter, Func<ToDoItem, DateTime> orderBy)
    {
      var items = await repository.GetAllItemsAsync();
      var list = items.Where(filter).OrderBy(orderBy);
      return list.ToList();
    }
    /// <inheritdoc/>
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetFilteredAndSortedItemsAsync(FilterParameters filterParameters, string? sortColumn = "Title")
    {
      IQueryable<ToDoItem> items = repository.GetAllItemsAsQueryable();
      if (!string.IsNullOrEmpty(filterParameters.Title))
      {
        items = items.Where(p => p.Title.Contains(filterParameters.Title));
      }
      if (filterParameters.IsCompleted.HasValue)
      {
        items = items.Where(p => p.IsCompleted == filterParameters.IsCompleted.Value);
      }
      if (filterParameters.CreatedBefore.HasValue)
      {
        items = items.Where(p => filterParameters.CreatedBefore.Value <= p.CreatedAt);
      }
      if (filterParameters.CreatedAfter.HasValue)
      {
        items = items.Where(p => p.CreatedAt <= filterParameters.CreatedBefore.Value);
      }
      // Todo: check this code
      if (filterParameters.Tags.Count > 0)
      {
        items = items.Where(p => ContsinsAllTags(p.Tags, filterParameters.Tags));
      }
      // Todo: add other filters
      if (sortColumn != null)
      {
        items = items.OrderBy(sortColumn);
      }
      return await items.ToListAsync();
    }
    /// <inheritdoc/>
    public ToDoItem EditItem(int id, ToDoItem updatedTask)
    {
      var item = repository.GetItemById(id);
      if (null == item)
        throw new ArgumentException($"Item with id {id} not found");

      item.Title = updatedTask.Title;
      item.IsCompleted = updatedTask.IsCompleted;
      item.CompletedAt = updatedTask.IsCompleted ? DateTime.UtcNow : null;
      item.Tags = updatedTask.Tags;
      repository.Update(item);
      return item;
    }
    private static bool ContsinsAllTags(List<string> itemTags, List<string> filterTags)
    {
      foreach (var tag in filterTags)
      {
        if (!itemTags.Contains(tag)) return false;
      }
      return true;
    }
  }
}