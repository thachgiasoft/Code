using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using FX.Data;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIS.Core.ServiceImp
{
    public class COCAUGIA_THUCTE_CTService : BaseService<COCAUGIA_THUCTE_CT, long>, ICOCAUGIA_THUCTE_CTService
    {
        private readonly ICOCAUGIA_LOAIService _COCAUGIA_LOAIService;
        private readonly ICOCAUGIA_THUCTEService _COCAUGIA_THUCTEService;
        private readonly IBHYT_21Service _BHYT_21Service;
        private readonly IDMCOSOKCBService _DMCOSOKCBService;

        public COCAUGIA_THUCTE_CTService(string sessionFactoryConfigPath) : base(sessionFactoryConfigPath)
        {
            _COCAUGIA_LOAIService = IoC.Resolve<ICOCAUGIA_LOAIService>();
            _COCAUGIA_THUCTEService = IoC.Resolve<ICOCAUGIA_THUCTEService>();
            _DMCOSOKCBService = IoC.Resolve<IDMCOSOKCBService>();
            _BHYT_21Service = IoC.Resolve<IBHYT_21Service>();
        }

        public IEnumerable<COCAUGIA_THUCTE_CT_QUERY> TongHopCoCauChiTiet(int month, int year, string maCskcb)
        {
            StringBuilder queryString = new StringBuilder();

            queryString.AppendFormat(
                "SELECT thucte_ct.ID_LOAI, thucte_ct.MA_COCAU , thucte_ct.TEN_COCAU_OLD, SUM(thucte_ct.SO_LUONG_OLD * bhyt21.SOLUONG) SO_LUONG_OLD, thucte_ct.DONGIA_OLD, thucte_ct.TEN_COCAU_NEW, SUM(thucte_ct.SO_LUONG_NEW * bhyt21.SOLUONG) SO_LUONG_NEW, thucte_ct.DONGIA_NEW, loai.ten TEN_LOAI " +
                "FROM BHYT_21 bhyt21, COCAUGIA_DICHVU21 dichvu21, COCAUGIA_THUCTE thucte, COCAUGIA_THUCTE_CT thucte_ct, COCAUGIA_LOAI loai, DM_COSOKCB cskcb " +
                "WHERE SUBSTR(bhyt21.MADVKT, -4, 4) = SUBSTR(dichvu21.MA_DICHVU21, -4, 4) AND dichvu21.ID = thucte.ID_DV AND thucte_ct.ID_THUCTE = thucte.ID AND thucte_ct.ID_LOAI = loai.ID " +
                "AND bhyt21.COSOKCB_ID = cskcb.ID " +
                "AND bhyt21.THANG_QT={0} AND bhyt21.NAM_QT={1}", month, year);

            if (!string.IsNullOrEmpty(maCskcb))
            {
                queryString.AppendFormat(" AND cskcb.MA = '{0}'", maCskcb);
            }

            queryString.Append(" GROUP BY thucte_ct.MA_COCAU , thucte_ct.TEN_COCAU_OLD, thucte_ct.ID_LOAI, thucte_ct.DONGIA_NEW, thucte_ct.DONGIA_OLD, thucte_ct.TEN_COCAU_NEW, loai.TEN");

            var result = NHibernateSession.CreateSQLQuery(queryString.ToString())
                .SetResultTransformer(Transformers.AliasToBean<COCAUGIA_THUCTE_CT_QUERY>())
                .List<COCAUGIA_THUCTE_CT_QUERY>();

            return result;
           
        }
    }

}
