using AuthorizeNet.Api.Contracts.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucommerce.EntitiesV2;
using Ucommerce.Security;

namespace Struct.PIM.Ucommerce.Connector.Security
{
    public class MemberService : IMemberService
    {
        public IList<MemberGroup> GetMemberGroups()
        {
            throw new NotImplementedException();
        }

        public IList<MemberType> GetMemberTypes()
        {
            throw new NotImplementedException();
        }

        public bool IsLoggedIn()
        {
            throw new NotImplementedException();
        }

        public bool IsMember(string emailAddress)
        {
            throw new NotImplementedException();
        }

        public Member GetCurrentMember()
        {
            throw new NotImplementedException();
        }

        public Member GetMemberFromEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Member GetMemberFromLoginName(string loginName)
        {
            throw new NotImplementedException();
        }

        public Member MakeNew(string loginName, string password, string email, MemberType memberType, MemberGroup memberGroup)
        {
            throw new NotImplementedException();
        }
    }
    public class UsernameService : ICurrentUserNameService
    {
        public string CurrentUserName => "";
    }
}
