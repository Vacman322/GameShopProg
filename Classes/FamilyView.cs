using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopProg.EF
{
    public partial class FamilyView
    {
        public string Login { get => Kid.Login; }
        public string Confirm { get => IsAccepted ? "": "Не подтверждено"; }
    }
}
