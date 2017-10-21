using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using FX.Data;
using IdentityManagement.Service;
using System;
using System.Linq;
using log4net;

namespace EIS.Core.ServiceImp
{
    public class LogSystemService : BaseService<LogSystem, int>, ILogSystemService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LogSystemService));
        public LogSystemService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }

        public IQueryable<LogSystem> GetByFilter(string keyword, DateTime? fromdate, DateTime? todate, bool? IsAdmin, string TenNguoiDung)
        {
            var _iuserService = IoC.Resolve<IuserService>();
            if (IsAdmin == true)
            {
                return Query.Where(x => string.IsNullOrEmpty(keyword) || x.EVENT.ToUpper().Contains(keyword.ToUpper())
                               || x.COMMENT_LOG.ToUpper().Contains(keyword.ToUpper())
                               || x.User.username.ToUpper().Contains(keyword.ToUpper()))
                               .Where(x => fromdate == null || x.CREATEDATE >= fromdate)
                               .Where(x => todate == null || x.CREATEDATE <= todate).OrderByDescending(c => c.CREATEDATE);
            }
            else
            {
                if (_iuserService.Query.FirstOrDefault(m => m.username == TenNguoiDung).ISADMIN)
                {
                    var lstUserName =
                        _iuserService.Query.Where(q => q.username == TenNguoiDung || q.GroupName == TenNguoiDung).Select(q => q.username).ToList();
                    var QueryTheoTen = Query.Where(m => lstUserName.Contains(m.User.username));
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        QueryTheoTen = QueryTheoTen.Where(n => n.User.username.ToUpper().Contains(keyword.ToUpper())
                            || n.EVENT.ToUpper().Contains(keyword.ToUpper())
                            || n.COMMENT_LOG.ToUpper().Contains(keyword.ToUpper()));
                    }
                    if (fromdate != null)
                    {
                        QueryTheoTen = QueryTheoTen.Where(x => x.CREATEDATE >= fromdate);
                    }
                    if (todate != null)
                    {
                        QueryTheoTen = QueryTheoTen.Where(x => x.CREATEDATE <= todate);
                    }
                    return QueryTheoTen.OrderByDescending(c => c.CREATEDATE);
                    //return QueryTheoTen.Where(x => string.IsNullOrEmpty(keyword) || x.EVENT.ToUpper().Contains(keyword.ToUpper())
                    //               || x.COMMENT_LOG.ToUpper().Contains(keyword.ToUpper())
                    //               || x.User.username.ToUpper().Contains(keyword.ToUpper()))
                    //               .Where(x => fromdate == null || x.CREATEDATE >= fromdate)
                    //               .Where(x => todate == null || x.CREATEDATE <= todate).OrderByDescending(c => c.CREATEDATE); 
                }
                else
                {
                    var QueryTheoTen = Query.Where(m => m.User.username == TenNguoiDung);
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        QueryTheoTen = QueryTheoTen.Where(n => n.User.username.ToUpper().Contains(keyword.ToUpper())
                            || n.EVENT.ToUpper().Contains(keyword.ToUpper())
                            || n.COMMENT_LOG.ToUpper().Contains(keyword.ToUpper()));
                    }
                    if (fromdate != null)
                    {
                        QueryTheoTen = QueryTheoTen.Where(x => x.CREATEDATE >= fromdate);
                    }
                    if (todate != null)
                    {
                        QueryTheoTen = QueryTheoTen.Where(x => x.CREATEDATE <= todate);
                    }
                    return QueryTheoTen.OrderByDescending(c => c.CREATEDATE);
                    //return QueryTheoTen.Where(x => string.IsNullOrEmpty(keyword) || x.EVENT.ToUpper().Contains(keyword.ToUpper())
                    //               || x.COMMENT_LOG.ToUpper().Contains(keyword.ToUpper())
                    //               || x.User.username.ToUpper().Contains(keyword.ToUpper()))
                    //               .Where(x => fromdate == null || x.CREATEDATE >= fromdate)
                    //               .Where(x => todate == null || x.CREATEDATE <= todate).OrderByDescending(c => c.CREATEDATE);   
                }

            }

        }

        public bool CreateNew(string userName, string mEvent, string comment, string ipaddress, string browser)
        {
            try
            {
                var temp = IoC.Resolve<IuserService>();
                var userId = temp.Query.FirstOrDefault(x => x.username.Equals(userName));
                if (userId == null)
                    return false;
                var mLog = new LogSystem
                {
                    ADMINID = userId.userid,
                    EVENT = mEvent,
                    COMMENT_LOG = comment,
                    IPADDRESS = ipaddress,
                    BROWSER = browser,
                    CREATEDATE = DateTime.Now
                };
                CreateNew(mLog);
                CommitChanges();
                return true;
            }
            catch (Exception e)
            {
                log.ErrorFormat("CreateNew- Message:{0}{1}- StackTrace{2}", e.Message, System.Environment.NewLine, e.StackTrace);
                return false;
            }
        }
         public int CreateLog(string userName, string mEvent, string comment, string ipaddress, string browser)
        {
            try
            {
                var temp = IoC.Resolve<IuserService>();
                var userId = temp.Query.FirstOrDefault(x => x.username.Equals(userName));
                if (userId == null)
                    return -99;
                var mLog = new LogSystem
                {
                    ADMINID = userId.userid,
                    EVENT = mEvent,
                    COMMENT_LOG = comment,
                    IPADDRESS = ipaddress,
                    BROWSER = browser,
                    CREATEDATE = DateTime.Now,
                    TRANGTHAI = 1
                };
                CreateNew(mLog);
                CommitChanges();
                return mLog.ID;
            }
            catch (Exception e)
            {
                log.ErrorFormat("CreateLog- Message:{0}{1}- StackTrace{2}", e.Message, System.Environment.NewLine, e.StackTrace);
                return -99;
            }

        }
         public  bool UpdateError(int? logId)
        {
            try
            {
                var results = Getbykey(logId ?? 0);
                if (results == null) return false;
                results.TRANGTHAI = 0;
                Update(results);
                CommitChanges();
                return true;

            }
            catch(Exception e)
            {
                log.ErrorFormat("UpdateError- Message:{0}{1}- StackTrace{2}", e.Message, System.Environment.NewLine, e.StackTrace);
                return false;
            }
        }
    }

}