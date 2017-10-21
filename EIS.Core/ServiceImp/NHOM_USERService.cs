using System;
using System.Collections.Generic;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;
using log4net;
using System.Linq;
using FX.Core;
using FX.Context;
using IdentityManagement.Service;

namespace EIS.Core.ServiceImp
{
    public class NHOM_USERService : BaseService<NHOM_USER, int>, INHOM_USERService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NHOM_USERService));
        public NHOM_USERService(string sessionFactoryConfigPath) : base(sessionFactoryConfigPath)
        {
        }

        public bool AddNewGroup(string groupName, List<int> userId, out string mess)
        {
            var nguoidungs = ((EISContext)FXContext.Current).CurrentNguoidung;
            var UserDataService = IoC.Resolve<IuserService>();
            var userIdData = UserDataService.Query.Where(o => o.username == nguoidungs.TENDANGNHAP).FirstOrDefault().userid;
            try
            {
                if (!Query.Any(n => n.GROUP_NAME.ToUpper() == groupName.ToUpper().Trim() && n.USER_ID == userIdData))//userId.Contains(n.USER_ID)
                {
                    var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();
                    BeginTran();
                    var group = new NHOM_USER()
                    {
                        GROUP_NAME = groupName.Trim(),
                        USER_ID = Convert.ToInt32(nguoidungs.ID),

                    };
                    CreateNew(group);
                    CommitChanges();

                    foreach (var id in userId)
                    {
                        var userGroup = new USER_GROUP()
                        {
                            USER_ID = id,
                            GROUP_ID = group.ID
                        };
                        userGroupSrv.CreateNew(userGroup);
                        CommitChanges();
                    }
                    CommitTran();

                    mess = "Thêm mới nhóm người dùng thành công";
                    return true;
                }
                else
                {
                    mess = "Tên nhóm này đã tồn tại, hãy thử lại với tên khác";
                    return false;
                }
            }
            catch (Exception ex)
            {
                RolbackTran();
                log.Error(ex.StackTrace);
                mess = "Lỗi hệ thống, vui lòng thử lại sau";
                return false;
            }
        }

        public bool DeleteGroup(int groupId, out string mess)
        {
            try
            {
                var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();
                var userOfGroup = userGroupSrv.GetUserIdByGroupId(groupId).ToList();
                BeginTran();
                
                
                foreach (var item in userOfGroup)
                {
                    userGroupSrv.Delete(item);
                    var uid = item.USER_ID;
                    string query = @"delete from USER_ROLE where USER_ROLE.USERID in :uid";
                    var res = ExecuteCountQuery(query, false, new SQLParam("uid", uid));
                }
                Delete(groupId);
                CommitTran();
                mess = "Xóa thành công";
                return true;
            }
            catch (Exception e)
            {
                log.Error(e.StackTrace);
                mess = "xóa thất bại.";
                return false;
            }
        }

        public bool UpdateGroup(int groupId, string groupName, List<int> userId, out string mess)
        {
            try
            {
                if (!Query.Any(n => n.GROUP_NAME.ToUpper() == groupName.ToUpper().Trim() && n.ID != groupId))
                {
                    var userGroupSrv = IoC.Resolve<IUSER_GROUPService>();
                    BeginTran();

                    var lstUserIdOfOldGroup = userGroupSrv.Query.Where(n => n.GROUP_ID == groupId).ToList();
                    foreach (var item in lstUserIdOfOldGroup)
                    {
                        userGroupSrv.Delete(item);
                        userGroupSrv.CommitChanges();
                    }

                    var group = Getbykey(groupId);
                    try
                    {
                        group.GROUP_NAME = groupName;
                    }
                    catch (Exception e)
                    {
                        log.ErrorFormat("message: {0}{1} stacktrace: {2}", e.Message, Environment.NewLine, e.StackTrace);
                    }
                    //Clear();
                    Update(group);
                    CommitChanges();
                    foreach (var id in userId)
                    {
                        var userGroup = new USER_GROUP()
                        {
                            USER_ID = id,
                            GROUP_ID = group.ID
                        };
                        userGroupSrv.CreateNew(userGroup);
                        CommitChanges();
                    }
                    CommitTran();

                    mess = "Cập nhật nhóm người dùng thành công";
                    return true;
                }
                else
                {
                    mess = "Tên nhóm này đã tồn tại, hãy thử lại với tên khác";
                    return false;
                }
            }
            catch (Exception ex)
            {
                RolbackTran();
                log.Error(ex.StackTrace);
                mess = "Lỗi hệ thống, vui lòng thử lại sau";
                return false;
            }
        }
    }
}