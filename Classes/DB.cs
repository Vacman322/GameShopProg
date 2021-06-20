using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShopProg.EF;

namespace GameShopProg
{
    public class DB
    {
        public static Entities Context { get; set; } = new Entities();
    }
}
