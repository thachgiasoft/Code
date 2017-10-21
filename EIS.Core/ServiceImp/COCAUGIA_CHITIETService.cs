using EIS.Core.Domain;
using EIS.Core.IService;
using EIS.Core.Common;
using FX.Data;
using System.Linq;
using System;
using System.Collections.Generic;
using DevExpress.Web;

namespace EIS.Core.ServiceImp
{
    public class COCAUGIA_CHITIETService : BaseService<COCAUGIA_CHITIET, long>, ICOCAUGIA_CHITIETService
    {
        public COCAUGIA_CHITIETService(string sessionFactoryConfigPath)
            : base(sessionFactoryConfigPath)
        { }

        public IQueryable<COCAUGIA_CHITIET> getByFilter(int nhomdvId)
        {
            IQueryable<COCAUGIA_CHITIET> query = Query;

            query = query.Where(o => o.DICHVU21_ID.ID == nhomdvId);

            return query;
        }

        public IQueryable<COCAUGIA_CHITIET> getByFilter(string maNhomdv)
        {
            IQueryable<COCAUGIA_CHITIET> query = Query;

            query = query.Where(o => o.DICHVU21_ID.MA_DICHVU21 == maNhomdv);

            return query;
        }

        public IEnumerable<COCAUGIA_CHITIET> GetCOCAUGIA_CHITIETRange(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            var skip = args.BeginIndex;
            var take = args.EndIndex - args.BeginIndex + 1;
            var query = Query.Where(item => item.TEN_CC.StartsWith(args.Filter)).Skip(skip).Take(take);
            return query.ToList();
        }

        public COCAUGIA_CHITIET GetCOCAUGIA_CHITIETByID(ListEditItemRequestedByValueEventArgs args)
        {
            int id;
            if (args.Value == null || !int.TryParse(args.Value.ToString(), out id))
                return null;
            return Getbykey(id);
        }

        public IEnumerable<COCAUGIA_CHITIET> GetByMaTrung4So(string ma)
        {
            var query = GetAll();

            var subma = ma.Right(4);
            List<COCAUGIA_CHITIET> result = new List<COCAUGIA_CHITIET>();
            foreach (var item in query)
            {
                try
                {
                    if (item.DICHVU21_ID != null &&
                        !string.IsNullOrEmpty(item.DICHVU21_ID.MA_DICHVU21) &&
                        item.DICHVU21_ID.MA_DICHVU21.EndsWith(subma))
                        result.Add(item);
                }
                catch
                {

                }
            }

            return result;
        }
    }
}