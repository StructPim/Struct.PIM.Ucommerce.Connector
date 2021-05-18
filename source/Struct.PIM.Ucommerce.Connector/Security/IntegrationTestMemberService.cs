using System;
using System.Collections.Generic;
using Ucommerce.EntitiesV2;
using Ucommerce.Security;

namespace Struct.PIM.Ucommerce.Connector.Security
{
    public class IntegrationTestMemberService : IMemberService
    {
        public IList<MemberGroup> GetMemberGroups()
        {
            return new List<MemberGroup>();
        }

        public IList<MemberType> GetMemberTypes()
        {
            return new List<MemberType>();
        }

        public bool IsLoggedIn()
        {
            return false;
        }

        public bool IsMember(string emailAddress)
        {
            return false;
        }

        public Member GetCurrentMember()
        {
            return null;
        }

        public Member GetMemberFromEmail(string email)
        {
            return null;
        }

        public Member GetMemberFromLoginName(string loginName)
        {
            return null;
        }

        public Member MakeNew(string loginName, string password, string email, MemberType memberType, MemberGroup memberGroup)
        {
            return new Member()
            {
                LoginName = loginName,
                Email = email,
                Password = password,
                MemberId = Guid.NewGuid().ToString(),
            };
        }
    }
}