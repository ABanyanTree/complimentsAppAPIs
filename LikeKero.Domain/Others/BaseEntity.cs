using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Domain
{
    public class BaseEntity
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int InsertCount { get; set; }        

        public string SortExp { get; set; }
        public string sort { get; set; }
        public string sortdir { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string CreatedByID { get; set; }
        public string LastModifiedByID { get; set; }
        public string CreatorName { get; set; }
        public string LastModifiorName{ get; set; }
        public bool IsSuccess { get; set; }
        public string RequesterUserId { get; set; }        
        public string LastModifiedDateDisplay { get; set; }
    }
}
