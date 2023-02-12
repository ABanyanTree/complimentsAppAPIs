using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Contract.Responses
{
    public class UserHierarchyResponse 
    {
        public string UserId { get; set; }
        public string Group1Id { get; set; }
        public string Group1Name { get; set; }
        public string Group2Id { get; set; }
        public string Group2Name { get; set; }
        public string Group3Id { get; set; }
        public string Group3Name { get; set; }
        public string Group4Id { get; set; }
        public string Group4Name { get; set; }
        public string Group5Id { get; set; }
        public string Group5Name { get; set; }

        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupType { get; set; }
        public string GroupLevel { get; set; }
        public string UserRoleId { get; set; }

    }
}
