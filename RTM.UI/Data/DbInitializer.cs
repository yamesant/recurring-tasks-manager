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
                DateTime.Now.Date.AddDays(-200), DateTime.Now.Date.AddDays(-30), 40),
            new("Ensure email inbox is empty", 2,
                DateTime.Now.Date.AddDays(-20), DateTime.Now.Date.AddDays(-6), 7),
            new("Get dental checkup", 0,
                DateTime.Now.Date, null, 180),
            new("Review finances", 2,
                DateTime.Now.Date.AddDays(-100), DateTime.Now.Date.AddDays(-20), 30),
            new("Go for a walk", 10,
                DateTime.Now.Date.AddDays(-30), DateTime.Now.Date.AddDays(-1), 3),
            new("Dedicate a day to reading a book", 2,
                DateTime.Now.Date.AddDays(-60), DateTime.Now.Date.AddDays(-15), 20),
            new("Enjoy a leisurely morning", 1,
                DateTime.Now.Date.AddDays(-20), DateTime.Now.Date.AddDays(-10), 10),
            new("Clean the house", 10,
                DateTime.Now.Date.AddDays(-150), DateTime.Now.Date.AddDays(-23), 15),
            new("Review career goals", 5,
                DateTime.Now.Date.AddDays(-500), DateTime.Now.Date.AddDays(-80), 90),
            new("Go on vacation", 0,
                DateTime.Now.Date, null, 365),
        ];
        
        context.Tasks.AddRange(tasks);
        context.SaveChanges();
    }
}