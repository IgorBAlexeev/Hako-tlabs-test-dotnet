using System;
using System.Collections.Generic;
using TestApp.ToDoList.Entity;

namespace TestApp.ToDoList.Repository
{
  internal static class FakeData
  {
    internal static List<ToDoItem> FillWithFakeData(int numbertOfToDos)
    {
      var titles = new[]
      {
          "Laundry",
          "Grocery Shopping",
          "Pay Bills",
          "Clean the House",
          "Walk the Dog",
          "Finish Homework",
          "Call Mom",
          "Read a Book",
          "Exercise",
          "Cook Dinner"
      };
      var urgency = new[] { "urgent", "important", "not important" };
      var tags = new[] { "home", "work", "personal" };
      var rnd = new Random();
      var items = new List<ToDoItem>();
      for (int i = 0; i < numbertOfToDos; i++)
      {
        var item = new ToDoItem { Title = $"{titles[rnd.Next(titles.Length)]} ({urgency[rnd.Next(urgency.Length)]})" };
        item.CreatedAt = DateTime.Now.AddDays(-rnd.Next(30));
        if (rnd.Next(2) == 0)
        {
          item.IsCompleted = true;
          item.CompletedAt = item.CreatedAt.AddDays(rnd.Next(1, 10));
        }
        else
        {
          item.IsCompleted = false;
        }
        var tagCount = rnd.Next(0, tags.Length + 1);
        var selectedTags = new HashSet<int>();
        for (int t = 0; t < tagCount; t++)
        {
          int tagIndex;
          do
          {
            tagIndex = rnd.Next(tags.Length);
          } while (!selectedTags.Add(tagIndex));
          item.Tags.Add(tags[tagIndex]);
        }
        items.Add(item);
      }
      return items;
    }
  }
}