using Moq;
using TestApp.ToDoList.Entity;
using TestApp.ToDoList.Repository;
using TestApp.ToDoList.Tracker;

namespace ToDoList.UnitTest
{
    public class ToDoTasksServiceTests
    {
        [Fact]
        public void GetAllItemsTest()
        {
            // Arrange
            var toDoItems = GetTestSessions();
            int count = toDoItems.Count;
            var mockProductRepository = new Mock<IToDoItemsRepository>();
            mockProductRepository.Setup(repo => repo.GetAllItems()).Returns(toDoItems);
            var toDoItemService = new ToDoListTracker(mockProductRepository.Object);

            // Act
            int actualCount = toDoItemService.GetAllItems().Count();
           
            // Assert
            Assert.Equal(count, actualCount);
            // Verify the repository method was called exactly once
            mockProductRepository.Verify(repo => repo.GetAllItems(), Times.Once());
        }
        
        [Fact]
        public void AddItemTest()
        {
            // Arrange
            var toDoItems = GetTestSessions();
            int beforeCount = toDoItems.Count;
            var mockTodoRepository = new Mock<IToDoItemsRepository>();
            mockTodoRepository.Setup(repo => repo.Create(It.IsAny<ToDoItem>()))
                .Callback<ToDoItem>(prod => toDoItems.Add(prod))
                .Returns<ToDoItem>(prod => prod);
            var toDoItemService = new ToDoListTracker(mockTodoRepository.Object);
            var newTodoTitle = "new item";

            // Act
            var toDoItem = toDoItemService.AddItem(newTodoTitle);

            // Assert
            Assert.Equal(1, toDoItems.Count - beforeCount);
            mockTodoRepository.Verify(repo => repo.Create(It.Is<ToDoItem>(u => u.Title == newTodoTitle)), Times.Once());
        }
        private List<ToDoItem> GetTestSessions()
        {
            var toDoItems = new List<ToDoItem>();
            toDoItems.Add(new ToDoItem()
            {
                Id = 1,
                Title = "Todo something",
                CreatedAt = new DateTime(2025, 7, 2)
            });
            toDoItems.Add(new ToDoItem()
            {
                Id = 2,
                Title = "Todo something 2",
                CreatedAt = new DateTime(2025, 12, 30)
            });
            return toDoItems;
        }
        private List<ToDoItem> GetTestBigSessions()
        {
            var toDoItems = new List<ToDoItem>();
            toDoItems.Add(new ToDoItem()
            {
                Id = 1,
                Title = "Finish Homework (important)",
                IsCompleted = true,
                CreatedAt = new DateTime(2025, 12, 12),
                CompletedAt = new DateTime(2025, 12, 18),
                Tags = []
            });
            toDoItems.Add(new ToDoItem()
            {
                Id = 2,
                Title = "Call Mom (not important)",
                IsCompleted = true,
                CreatedAt = new DateTime(2026, 1, 2),
                CompletedAt = new DateTime(2026, 1, 5),
                Tags =
                [
                    "personal"
                ]
            });
            toDoItems.Add(new ToDoItem()
            {
                Id = 3,
                Title = "Walk the Dog (important)",
                IsCompleted = false,
                CreatedAt = new DateTime(2026, 1, 4),
                CompletedAt = null,
                Tags =
                [
                  "work"
                ]
            });
            toDoItems.Add(new ToDoItem()
            {
                Id = 4,
                Title = "Read a Book (important)",
                IsCompleted = true,
                CreatedAt = new DateTime(2025, 12, 19),
                CompletedAt = new DateTime(2025, 12, 26),
                Tags =
                [
                  "home"
                ]
            });
            toDoItems.Add(new ToDoItem()
            {
                Id = 5,
                Title = "Read a Book (important)",
                IsCompleted = false,
                CreatedAt = new DateTime(2025, 12, 31),
                CompletedAt = null,
                Tags =
                [
                  "home",
                  "personal",
                  "work"
                ]
            });
            toDoItems.Add(new ToDoItem()
            {
                Id = 6,
                Title = "Exercise (urgent)",
                IsCompleted = true,
                CreatedAt = new DateTime(2026, 1, 9),
                CompletedAt = new DateTime(2025, 1, 17),
                Tags =
                [
                  "work",
                  "home"
                ]
            });
            toDoItems.Add(new ToDoItem()
            {
                Id = 7,
                Title = "Call Mom (important)",
                IsCompleted = false,
                CreatedAt = new DateTime(2026, 1, 8),
                CompletedAt = null,
                Tags =
                [
                  "work",
                  "home",
                  "personal"
                ]
            });
            toDoItems.Add(new ToDoItem()
            {
                Id = 8,
                Title = "Finish Homework (important)",
                IsCompleted = true,
                CreatedAt = new DateTime(2025, 12, 21),
                CompletedAt = new DateTime(2025, 12, 29),
                Tags =
                [
                  "work"
                ]
            });
            toDoItems.Add(new ToDoItem()
            {
                Id = 9,
                Title = "Read a Book (not important)",
                IsCompleted = false,
                CreatedAt = new DateTime(2025, 12, 26),
                CompletedAt = null,
                Tags =
                [
                  "personal"
                ]
            });
            toDoItems.Add(new ToDoItem()
            {
                Id = 10,
                Title = "Clean the House (important)",
                IsCompleted = false,
                CreatedAt = new DateTime(2026, 1, 2),
                CompletedAt = null,
                Tags =
                [
                  "work",
                  "home"
                ]

            });
            return toDoItems;
        }
    }
}