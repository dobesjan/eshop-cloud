using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.Api.Requests.Cart
{
    public class AddToCartRequest
    {
        public string UserId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Count { get; set; }
    }
}
