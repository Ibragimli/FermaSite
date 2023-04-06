using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferma.Mvc.ViewModels
{
    public class DeleteModal
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
