using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineCafe.HostedServices
{
    public class Drink
    {
        public DrinkType DrinkType { get; }

        public Drink(DrinkType drinkType)
        {
            DrinkType = drinkType;
            Instructions = RecipeBook[drinkType];
        }

        public Dictionary<DrinkType, List<Func<CancellationToken, Task<string>>>> RecipeBook { get; } = new Dictionary<DrinkType, List<Func<CancellationToken, Task<string>>>>
        {
            {DrinkType.NoDrink, null },
            {DrinkType.Coffee, new List<Func<CancellationToken, Task<string>>>()
                {
                    async (token) => { 
                        await Task.Delay(TimeSpan.FromSeconds(2), token); 
                        return "Coffee Water Boiled"; 
                    },
                    async (token) => {
                        await Task.Delay(TimeSpan.FromSeconds(6), token);
                        return "Coffee Brewed";
                    },
                    async (token) => {
                        await Task.Delay(TimeSpan.FromSeconds(10), token);
                        return "Coffee filter";
                    },
                }
            },
            {DrinkType.LemonTea, new List<Func<CancellationToken, Task<string>>>()
                {
                    async (token) => {
                        await Task.Delay(TimeSpan.FromSeconds(4), token);
                        return "Tea Boil Water";
                    },
                    async (token) => {
                        await Task.Delay(TimeSpan.FromSeconds(8), token);
                        return "Tea Brew";
                    },
                    async (token) => {
                        await Task.Delay(TimeSpan.FromSeconds(12), token);
                        return "Tea add milk";
                    },
                }
            },
            {DrinkType.Chocolate, new List<Func<CancellationToken, Task<string>>>()
                {
                    async (token) => {
                        await Task.Delay(TimeSpan.FromSeconds(4), token);
                        return "Choc Boil Water";
                    },
                    async (token) => {
                        await Task.Delay(TimeSpan.FromSeconds(8), token);
                        return "Choc Brew";
                    },
                    async (token) => {
                        await Task.Delay(TimeSpan.FromSeconds(12), token);
                        return "Choc add milk";
                    },
                }
            }
        };
        public List<Func<CancellationToken, Task<string>>> Instructions { get; set; }
    }

    public enum DrinkType
    {
        NoDrink,
        Coffee,
        LemonTea,
        Chocolate
    }
}
