using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LikeKero.Data.Constants;
using LikeKero.Data.Constants.User;
using LikeKero.Data.Interfaces;
using LikeKero.Domain;
using LikeKero.Domain.DomainInterfaces;
using LikeKero.Domain.Feature;
using LikeKero.Domain.Utility;
using Microsoft.Extensions.Options;
namespace LikeKero.Data.Services
{
    public class FeatureMasterRepository : Repository<FeatureMaster>, IFeatureMasterRepository
    {
        public FeatureMasterRepository(IOptions<ReadConfig> connStr, IDapperResolver<FeatureMaster> resolver) : base(connStr, resolver)
        {
        }

        public FeatureMaster GetAllAdminFeatures(FeatureMaster obj)
        {
            string[] addParams = new string[] { BaseInfra.REQUESTERUSERID };

            IEnumerable<dynamic> mapItems = new List<dynamic>()
            {
                new MapItem(typeof(FeatureMaster), "FeatureMasterList"),
                new MapItem(typeof(UserRole), "UserRoleList"),
                new MapItem(typeof(RoleFeatureMaster), "RoleFeatureMasterList")
            };

            dynamic dynamicObject = GetMultipleAsync(obj, addParams, FeatureMasterInfra.SPROC_GETALL_FEATURES_ROLES_CURRENTMAPPING, mapItems);

            var FeatureMasterList = ((List<dynamic>)dynamicObject.FeatureMasterList).Cast<FeatureMaster>().ToList();
            var UserRoleList = ((List<dynamic>)dynamicObject.UserRoleList).Cast<UserRole>().ToList();
            var RoleFeatureMasterList = ((List<dynamic>)dynamicObject.RoleFeatureMasterList).Cast<RoleFeatureMaster>().ToList();

            AdminRightsModel model = new AdminRightsModel()
            {
                FeatureMasterList = FeatureMasterList,
                UserRoleList = UserRoleList,
                RoleFeatureMasterList = RoleFeatureMasterList
            };

            FeatureMaster objResponse = new FeatureMaster();
            foreach (FeatureMaster objFeature in model.FeatureMasterList)
            {
                foreach (UserRole objRole in model.UserRoleList.OrderBy(x => x.SequenceNo))
                {
                    DateTime? dt = model.RoleFeatureMasterList.OrderByDescending(x => x.LastModifiedDate).Select(x => x.LastModifiedDate).FirstOrDefault();
                    RoleFeatureMaster objRoleFeature = new RoleFeatureMaster();
                    objRoleFeature.FeatureId = objFeature.FeatureId;
                    objRoleFeature.FeatureName = objFeature.FeatureName;
                    objRoleFeature.IsShowInMenu = objFeature.IsShowInMenu;
                    objRoleFeature.IsNoAction = objFeature.IsNoAction;
                    objRoleFeature.ParentFeatureId = objFeature.ParentFeatureId;
                    objRoleFeature.DisplayOrder = objFeature.DisplayOrder;
                    objRoleFeature.RoleId = objRole.RoleId;
                    objRoleFeature.DisplayRole = objRole.DisplayRole;
                    objRoleFeature.RoleName = objRole.RoleName;
                    objRoleFeature.SequenceNo = objRole.SequenceNo;
                    objRoleFeature.LastModifiedDate = dt;

                    RoleFeatureMaster lstRole = model.RoleFeatureMasterList.Where(x => x.FeatureId == objFeature.FeatureId && x.RoleId == objRole.RoleId).FirstOrDefault();
                    objRoleFeature.Status = lstRole == null ? false : true;

                    objResponse.RoleFeatureMasterList.Add(objRoleFeature);
                }
            }

            return objResponse;
        }


    }
}
