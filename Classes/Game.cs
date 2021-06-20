using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameShopProg.EF
{
    public partial class Game
    {
        public string ImgSrc
        {
            get
            {
                if (Logo is null || string.IsNullOrWhiteSpace(Logo))
                    return null;

                return "/Resources/Logos/" + Logo;
            }
        }

        public string NameAndPrice { get => $"{Name} X {RegionPrice} руб"; }

        public bool IsNotAddedToCart
        {
            get => !IsAddedToCart;
        }

        public bool IsAddedToCart
        {
            get => UserInfo.User.Cart.Contains(this) ||
UserInfo.User.UserGame.Any(r => r.Game == this);
        }

        public decimal RegionPrice { get => Price * UserInfo.User.Region.RegionCoefficient; }

        public int Hours
        {
            get
            {
                var hours = UserInfo.User.GameSession
                .Where(r => r.Game == this)
                .Select(r => r.TimeSpendInHours)
                .Sum();

                return hours;
            }
        }
    }
}

