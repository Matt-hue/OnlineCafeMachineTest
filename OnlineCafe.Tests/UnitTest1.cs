using OnlineCafe.HostedServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnlineCafe.Tests
{
    public class UnitTest1
    {

        //[Fact]
        //public async void t4()
        //{
        //    var drink1 = new Drink(DrinkType.Coffee);
        //    var drink2 = new Drink(DrinkType.LemonTea);

        //    var ll = new List<List<Func<Task<string>>>>()
        //    {
        //        drink1.Instructions0,
        //        drink2.Instructions0
        //    };

        //    var q = new ConcurrentQueue<string>();

        //    var tasks = ll.SelectMany(x => x, async (t, i) =>
        //    {
        //        string result = await i.Invoke();
        //        q.Enqueue(result);
        //    }).ToList();

        //    while (tasks.Any())
        //    {
        //        //Debug.WriteLine($"Tasks count: {tasks.Count()} : {DateTime.Now.Second}");
        //        Task task = await Task.WhenAny(tasks);
        //        q.TryDequeue(out string msg);
        //        if (msg != null)
        //        {
        //            Debug.WriteLine($"{msg} : {DateTime.Now.Second}");
        //        }
        //        tasks.Remove(task);
        //    }
        //}

    }
}
