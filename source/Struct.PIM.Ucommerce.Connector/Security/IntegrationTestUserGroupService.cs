using System.Collections.Generic;
using Ucommerce.EntitiesV2;
using Ucommerce.Security;

namespace Struct.PIM.Ucommerce.Connector.Security

{
    public class IntegrationTestUserGroupService : IUserGroupService
    {
        public IList<UserGroup> GetAllUserGroups()
        {
            return new List<UserGroup>();
        }

        public UserGroup GetUserGroup(string externalId)
        {
            return null;
        }
    }
}