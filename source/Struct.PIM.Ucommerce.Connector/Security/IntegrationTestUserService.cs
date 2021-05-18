using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucommerce.EntitiesV2;
using Ucommerce.Security;

namespace Struct.PIM.Ucommerce.Connector.Security
{
    public class IntegrationTestUserService : IUserService
    {
        public User GetCurrentUser()
        {
            return new User("IntegrationTest")
            {
                IsAdmin = true
            };
        }

        public User GetUser(string userName)
        {
            return GetCurrentUser();
        }

        public IList<User> GetAllUsers()
        {
            return new List<User>()
            {
                GetCurrentUser()
            };
        }

        public CultureInfo GetCurrentUserCulture()
        {
            return new CultureInfo("en-US");
        }
    }
}
