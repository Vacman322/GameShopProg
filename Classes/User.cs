using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShopProg.EF
{
    public partial class User
    {
        public string Fio { get => $"{LastName} {FirstName} {Patronymic}"; }

        public int? Age
        {
            get
            {
                if (DateOfBirth is null)
                    return null;

                var age = (int)Math.Floor((DateTime.Now - DateOfBirth).Value.TotalDays / 365);
                return age;
            }
        }

        public int TheSameGamesCount
        {
            get
            {
                var profileUserGames = UserInfo.User.UserGame
                    .Select(r => r.Game)
                    .ToList();
                var userGames = UserGame
                    .Select(r => r.Game)
                    .ToList();

                var sameCount = profileUserGames
                    .Intersect(userGames)
                    .Count();

                return sameCount;
            }
        }

        public int GameCount { get => UserGame.Count; }

        public bool IsOverEighteen { get => Age.HasValue && Age.Value >= 18; }

        public string ProfileImgSrc
        {
            get
            {
                if (ProfileImg is null)
                    return null;
               return new DirectoryInfo(Environment.CurrentDirectory)
                    .Parent.Parent
                    .FullName +
                    "\\Resources\\ProfileImgs\\" + ProfileImg;
            }
        }
    }
}
