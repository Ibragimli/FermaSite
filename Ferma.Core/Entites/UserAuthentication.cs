using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Core.Entites
{
    public class UserAuthentication : BaseEntity
    {
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public bool IsDisabled { get; set; }
        public int Count { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddHours(4).AddMinutes(10);
    }
}
