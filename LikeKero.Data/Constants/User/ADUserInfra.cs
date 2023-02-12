using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeKero.Data.Constants
{
    public class ADUserInfra: BaseInfra
    {
        public const string ADUSERID = "ADUserId";
        public const string FIRSTNAME = "FirstName";
        public const string LASTNAME = "LastName";
        public const string USERNAME = "UserName"; 
        public const string EMAILADDRESS = "EmailAddress";
        public const string PROFILEIMAGEPATH = "ProfileImagePath";
        public const string ISACTIVE = "IsActive";
        public const string CREATEDDATE = "CreatedDate";       
        public const string LOBID = "LOBID";

        public const string SPROC_ADUSER_LSTALL = "sproc_ADUser_lstAll";
        public const string SPROC_ADUSER_GETALL = "sproc_ADUser_GetAll";
        public const string SPROC_GETALLLOBAPPROVERDETAILS_LSTALL = "sproc_GetAllLOBApproverDetails_lstAll";
        

    }
}
