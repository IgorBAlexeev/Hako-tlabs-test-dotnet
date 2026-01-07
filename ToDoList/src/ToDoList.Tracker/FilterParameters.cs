using System;
using System.Collections.Generic;

namespace ToDoList.Tracker
{
  public class FilterParameters
  {
    public string? Title { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
    // public DateTime? CompletedAt { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
  }
}