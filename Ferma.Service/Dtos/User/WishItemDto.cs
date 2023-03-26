using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Service.Dtos.User
{
    public class WishDto
    {
        public List<WishItemDto> WishItems { get; set; }

    }
    public class WishItemDto
    {
        public int PosterId { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        
    }
}
