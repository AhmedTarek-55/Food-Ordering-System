using Core.Entities.Order_Entities;

namespace Infrastructure.Specifications.Orders
{
    public class OrderWithItemsSpecifications : BaseSpecifications<Order>
    {
        public OrderWithItemsSpecifications(string email) : base(order => order.BuyerEmail == email)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
            AddOrderByDescending(order => order.OrderDate);
        }

        public OrderWithItemsSpecifications(int Id, string email) : base(order => order.BuyerEmail == email && order.Id == Id)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);
        }
    }
}
