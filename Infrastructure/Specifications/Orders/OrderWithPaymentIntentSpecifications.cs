using Core.Entities.Order_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.Orders
{
    public class OrderWithPaymentIntentSpecifications : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpecifications(string PaymentIntentId) : base(order => order.PaymentIntentId == PaymentIntentId)
        {
        }
    }
}
