using System;
using System.Linq;
using RTM.Core;

namespace RTM.UI.Data;

public static class DbInitializer
{
    public static void Initialize(TaskContext context)
    {

        if (context.Tasks.Any())
        {
            return;
        }
        
        Task[] tasks = [
            new("Check up on friends or family",4,
                DateTime.Now.AddDays(-200), DateTime.Now.AddDays(-30), 40),
            new("Ensure email inbox is empty", 2,
                DateTime.Now.AddDays(-21), DateTime.Now.AddDays(-7), 7),
            new("Get dental checkup", 0,
                DateTime.Now, null, 180),
            new("Review finances", 2,
                DateTime.Now.AddDays(-100), DateTime.Now.AddDays(-20), 30),
            new("Go for a walk", 10,
                DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-1), 3),
            new("Dedicate a day to reading a book", 2,
                DateTime.Now.AddDays(-60), DateTime.Now.AddDays(-15), 20),
            new("Enjoy a leisurely morning", 1,
                DateTime.Now.AddDays(-21), DateTime.Now.AddDays(-11), 10),
            new("Clean the house", 10,
                DateTime.Now.AddDays(-150), DateTime.Now.AddDays(-23), 15),
            new("Review career goals", 5,
                DateTime.Now.AddDays(-500), DateTime.Now.AddDays(-80), 90),
            new Task("Go on vacation", 0,
                DateTime.Now, null, 365),
        ];
        
        context.Tasks.AddRange(tasks);
        context.SaveChanges();
    }
}