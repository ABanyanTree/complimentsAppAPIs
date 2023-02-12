using System;
using System.Collections.Generic;
using System.Text;

namespace LikeKero.Infra.FileSystem
{
    public class FileSystemPath
    {
        public string ProfilePicPath { get; set; }
   
        public string BaseURLPath { get; set; }
        public string DefaultProfilePicName { get; set; }       
        public int InstantUserUploadSizeLimit { get; set; }    

        public string AfterDomain { get; set; }
        public int InstantUserUploadUserLimit { get; set; }     
     
        public string AllowedReferres1 { get; set; }
        public string AllowedReferres2 { get; set; }
        public string CreateAPILog { get; set; }    
        public string BaseURLPathHttps { get; set; }

        //

        public string TestCustFilePath { get; set; }
    }
}
