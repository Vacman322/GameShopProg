//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameShopProg.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserGame
    {
        public int IDUser { get; set; }
        public int IDGame { get; set; }
        public decimal BuyPrice { get; set; }
        public System.DateTime BuyDate { get; set; }
    
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
    }
}
