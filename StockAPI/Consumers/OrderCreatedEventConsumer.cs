using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Shared.Events;
using Shared.Messages;
using StockAPI.Models.Entities;
using StockAPI.Services;

namespace StockAPI.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        IMongoCollection<Stock> _stockCollection;
        public OrderCreatedEventConsumer(MongoDbService mongoDbService)
        {
                _stockCollection = mongoDbService.GetCollection<Stock>();
        }
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            foreach (OrderItemMessage orderItem in context.Message.OrderItems)
            {
                stockResult.Add((await _stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count)).Any());
            }
            if(stockResult.TrueForAll(s_result => s_result.Equals(true)))
            { 
                foreach(OrderItemMessage orderItem in context.Message.OrderItems)
                {
                    Stock stock = await (await _stockCollection.FindAsync(s => s.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();
                    stock.Count -= orderItem.Count;
                    await _stockCollection.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId, stock);
                }
                //payment process...
            }
            else
            {
                //order failed process...
            }

            return Task.CompletedTask;
        }
    }
}
