using System;

namespace LikeKero.Domain
{
    public class LookupMaster : BaseEntity
    {
        public string LookupID { get; set; }
        public string LookUpType { get; set; }
        public string LookUpName { get; set; }
        public string LookUpValue { get; set; }
        public int DisplayOrder { get; set; }

        public string FieldType { get; set; }
        public bool IsGroup { get; set; }
        public string SearchonGroupId { get; set; }

        public string BRUserFieldType { get; set; }


    }
}
