using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Model.Domain
{
    public class DeliveryData
    {
        public List<int> Ids { get; set; }

        public string CarrierId { get; set; }

        public string AddressId { get; set; }
    }
}
