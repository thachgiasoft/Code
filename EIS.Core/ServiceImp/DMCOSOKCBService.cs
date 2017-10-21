using EIS.Core.Common;
using EIS.Core.CustomView;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using FX.Data;
using IdentityManagement.Service;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using DevExpress.Data.Helpers;
using DevExpress.Web;

namespace EIS.Core.ServiceImp
{
    public class DMCOSOKCBService : BaseService<DM_COSOKCB, long>, IDMCOSOKCBService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DMCOSOKCBService));

        private IDM_DONVIService _iDMDONVIService = IoC.Resolve<IDM_DONVIService>();
        private IuserService _iuserService = IoC.Resolve<IuserService>();
        private NGUOIDUNG nguoidung = ((EIS.Core.EISContext)FX.Context.FXContext.Current).CurrentNguoidung;

        public DMCOSOKCBService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }

        public IQueryable<DM_COSOKCB> getAll()
        {
            return Query;
        }
        
        public IQueryable<DM_COSOKCB> getByTinhThanhID(long tinhThanhID)
        {
            return Query.Where(w => w.TINHTHANH_ID == tinhThanhID && w.HIEULUC == true);
        }

        public IEnumerable<DM_COSOKCB> GetDM_COSOKCBRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            var query = Query.Where(item => item.TEN.StartsWith(args.Filter)).Skip(skip).Take(take);
            return query.ToList();
        }

        public DM_COSOKCB GetDM_COSOKCBByID(ListEditItemRequestedByValueEventArgs args)
        {
            int id;
            if (args.Value == null || !int.TryParse(args.Value.ToString(), out id))
                return null;
            return Getbykey(id);
        }

        public IQueryable<DM_COSOKCB> GetByMa(string ma)
        {
            var query = Query.Where(item => item.MA.Equals(ma));
            return query;
        }

        public DM_COSOKCB GetDM_COSOKCBByMa(ListEditItemRequestedByValueEventArgs args)
        {
            if (args.Value == null)
                return null;
            return Query.Where(item => item.TEN.Equals(args.Value.ToString())).LastOrDefault();
        }
    }
}
