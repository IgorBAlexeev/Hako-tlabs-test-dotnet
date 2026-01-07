using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using TestApp.ToDoList.Entity;
using TestApp.ToDoList.Module;
using ToDoList.Tracker;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TestApp.Server.Controllers
{
  [ApiController]
  [Route("api/tasks")]
  // [Authorize]
  public class ToDoTasksController : ControllerBase
  {
    private readonly IToDoListTracker toDoListTracker;
    private readonly IMemoryCache memoryCache;

    public ToDoTasksController(IToDoListTracker toDoListTracker, IMemoryCache memoryCache)
    {
      this.toDoListTracker = toDoListTracker;
      this.memoryCache = memoryCache;
    }

    [HttpGet]
    [Route("allTasks")]
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetTasksAsync()
    {
      var tasks = await toDoListTracker.GetAllItemsAsync();
      if (tasks == null)
      {
        return NotFound();
      }
      return tasks;
    }

    [HttpGet(Name = "ToDoItems (page)")]
    [ResponseCache(CacheProfileName = "Any-120")]
    public async Task<ActionResult<IEnumerable<ToDoItem>>> GetOnePageAsync(int pageIndex, int pageSize)
    {
      ActionResult<IEnumerable<ToDoItem>> tasks = null;
      var cacheKey = $"{pageIndex}{pageSize}";
      if (!memoryCache.TryGetValue(cacheKey, out tasks))
      {
        tasks = await toDoListTracker.GetOnePageAsync(pageIndex * pageSize, pageSize);
        memoryCache.Set(cacheKey, tasks, new TimeSpan(0, 0, 30));
      }
      return tasks;
    }

    [HttpGet]
    [Route("filteredAndSortedTasks")]
    [ResponseCache(CacheProfileName = "Any-120")]
    public async Task<ActionResult<IEnumerable<ToDoItem>>> FilteredAndSortedTasksAsync([FromQuery] FilterParameters filterParameters, string? sortColumn = "Title")
    {
      ActionResult<IEnumerable<ToDoItem>> tasks = null;
      var cacheKey = $"{sortColumn} {JsonSerializer.Serialize(filterParameters)}";
      if (!memoryCache.TryGetValue(cacheKey, out tasks))
      {
        tasks = await toDoListTracker.GetFilteredAndSortedItemsAsync(filterParameters, sortColumn);
        memoryCache.Set(cacheKey, tasks, new TimeSpan(0, 0, 30));
      }
      return tasks;
    }

    [HttpPost]
    public ToDoItem CreateTask([FromBody] ToDoItem newTask)
    {
      var task = toDoListTracker.AddItem(newTask.Title, newTask.Tags);
      return task;
    }

    [HttpPut("{id}")]
    public ToDoItem EditTask(int id, [FromBody] ToDoItem updatedTask)
    {
      var task = toDoListTracker.EditItem(id, updatedTask);
      return task;
    }

    [HttpDelete("{id}")]
    public ToDoItem DeleteTask(int id)
    {
      var item = toDoListTracker.RemoveItem(id);
      return item;
    }
  }
}