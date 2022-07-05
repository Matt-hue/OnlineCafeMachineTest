using Microsoft.AspNetCore.SignalR;
using OnlineCafe.HostedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineCafe.Hubs
{
    public class CafeHub : Hub
    {
        private readonly ICafeDrinksQueue _cafeDrinksQueue;

        public CafeHub(ICafeDrinksQueue cafeDrinksQueue)
        {
            _cafeDrinksQueue = cafeDrinksQueue;
        }

        public async Task CreateOrder(DrinkType drinkType)
        {
            var drink = new Drink(drinkType);

            if (drink.DrinkType == DrinkType.NoDrink)
                return;

            await Clients.All.SendAsync("ReceiveMessage", $"{drinkType} has been ordered");

            foreach (var instruction in drink.Instructions)
            {
                await _cafeDrinksQueue.QueueBackgroundWorkItemAsync(instruction);

            }
        }

    }
}
