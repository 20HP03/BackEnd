using API.DTOs;
using API.Extensions;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class OrdersController(ICartService cartService, IUnitOfWork unit) : BaseApiController
{
    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto) 
    {
        var email = User.GetEmail();

        var cart = await cartService.GetCartAsync(orderDto.CartId);

        if(cart == null) return BadRequest("Cart Not Found!");

        if(cart.PaymentIntentId == null) return BadRequest("No Payment Intent For This Order!");

        var items = new List<OrderItem>();

        foreach (var item in cart.Items)
        {
            var productItem = await unit.Repository<Product>().GetByIdAsync(item.ProductId);

            if(productItem == null) return BadRequest("Problem With The Order!");

            var itemOrder = new ProductItemOrdered
            {
                ProductId = item.ProductId,
                ProdudctName = item.ProductName,
                PictureUrl = item.PictureUrl
            };

            var orderItem = new OrderItem
            {
                ItemOrdered = itemOrder,
                Price = productItem.Price,
                Quantity = item.Quantity
            };

            items.Add(orderItem);
        }

        var deliveryMethod = await unit.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

        if(deliveryMethod == null) return BadRequest("No Delivery Method Selected!");

        var order = new Order 
        {
            OrderItems = items,
            DeliveryMethod = deliveryMethod,
            ShippingAddress = orderDto.ShippingAddress,
            Subtotal = items.Sum(x => x.Price * x.Quantity),
            PaymentSummary = orderDto.PaymentSummary,
            PaymentIntentId = cart.PaymentIntentId,
            BuyerEmail = email
        };

        unit.Repository<Order>().Add(order);

        if(await unit.Complete())
        {
            return order;
        }

        return BadRequest("Problem Creating Order!");
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
    {
        var spec = new OrderSpecification(User.GetEmail());

        var orders = await unit.Repository<Order>().ListAsync(spec);

        var ordersToReturn = orders.Select(o => o.ToDto()).ToList();

        return Ok(ordersToReturn);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(int id)
    {
        var spec = new OrderSpecification(User.GetEmail(), id);

        var order = await unit.Repository<Order>().GetEntityWithSpec(spec);

        if(order == null) return NotFound();

        return order.ToDto();
    }
}
