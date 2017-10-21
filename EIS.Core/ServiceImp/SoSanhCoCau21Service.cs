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
using EIS.Core.Domain2;

namespace EIS.Core.ServiceImp
{
    class SoSanhCoCau21Service : BaseService<DichVu21DonGia, int>,ISoSanhCoCau21
    {
       
        public SoSanhCoCau21Service(String sessionFactoryConfigPath) : 
            base(sessionFactoryConfigPath)
        {

        }

        public IEnumerable<COCAUGIA_THUCTE_CT_QUERY> GetCoCauGiaThucTe(long IdDichVu,long? cskcbId)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.AppendFormat(
                "SELECT thucte_ct.ID, thucte_ct.ID_LOAI, thucte_ct.MA_COCAU , thucte_ct.TEN_COCAU_OLD, thucte_ct.SO_LUONG_OLD, thucte_ct.DONGIA_OLD, thucte_ct.TEN_COCAU_NEW, thucte_ct.SO_LUONG_NEW, thucte_ct.DONGIA_NEW, loai.ten TEN_LOAI " +
                "FROM COCAUGIA_THUCTE thucte, COCAUGIA_THUCTE_CT thucte_ct, COCAUGIA_LOAI loai, DM_COSOKCB cskcb " +
                "WHERE thucte_ct.ID_THUCTE = thucte.ID AND thucte_ct.ID_LOAI = loai.ID And thucte.MA_CSKCB = cskcb.MA " +
                "AND thucte.ID_DV={0}", IdDichVu);

            if (cskcbId != null )
            {
                sqlQuery.AppendFormat(" AND cskcb.ID = {0}", cskcbId);
            }

            var result = NHibernateSession.CreateSQLQuery(sqlQuery.ToString())
                .SetResultTransformer(Transformers.AliasToBean<COCAUGIA_THUCTE_CT_QUERY>())
                .List<COCAUGIA_THUCTE_CT_QUERY>();
            return result;
        }

        
        public IEnumerable<DichVu21DonGia> GetDichVu21(int month, int year, long? cskcbId)
        {
            StringBuilder sqlQuery = new StringBuilder();
                sqlQuery.AppendFormat(
                "SELECT dichvu21.ID Id, bhyt21.MADVKT MaDichVu, dichvu21.TEN_DICHVU21 TenDichVu,bhyt21.SOLUONG SoLuong, bhyt21.DONGIA DonGia, bhyt21.THANHTIEN ThanhTien " +
                "FROM BHYT_21 bhyt21, COCAUGIA_DICHVU21 dichvu21 " +
                "WHERE SUBSTR(bhyt21.MADVKT, -4, 4) = SUBSTR(dichvu21.MA_DICHVU21, -4, 4)" +
                "AND bhyt21.THANG_QT={0} AND bhyt21.NAM_QT={1}", month, year);

            if (cskcbId != null)
            {
                sqlQuery.AppendFormat(" AND bhyt21.COSOKCB_ID = {0}", cskcbId);
            }

            var result = NHibernateSession.CreateSQLQuery(sqlQuery.ToString())
                .SetResultTransformer(Transformers.AliasToBean<DichVu21DonGia>())
                .List<DichVu21DonGia>();
            return result;
        }

        public IEnumerable<DichVu21DonGia> GetDichVu21(int month, int year, string maCskcb)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.AppendFormat(
            "SELECT bhyt21.MADVKT MADICHVU, MIN(dichvu21.TEN_DICHVU21) TENDICHVU, SUM(bhyt21.SOLUONG) SOLUONG, MIN(bhyt21.DONGIA) DONGIA, SUM(bhyt21.THANHTIEN) THANHTIEN " +
            "FROM BHYT_21 bhyt21, COCAUGIA_DICHVU21 dichvu21, DM_COSOKCB cskcb " +
            "WHERE SUBSTR(bhyt21.MADVKT, -4, 4) = SUBSTR(dichvu21.MA_DICHVU21, -4, 4) " +
            "AND bhyt21.COSOKCB_ID = cskcb.ID " +
            "AND bhyt21.THANG_QT={0} AND bhyt21.NAM_QT={1}", month, year);

            if (!string.IsNullOrEmpty(maCskcb))
            {
                sqlQuery.AppendFormat(" AND cskcb.MA = '{0}'", maCskcb);
            }

            sqlQuery.AppendFormat(" GROUP BY bhyt21.MADVKT");

            var result = NHibernateSession.CreateSQLQuery(sqlQuery.ToString())
                .SetResultTransformer(Transformers.AliasToBean<DichVu21DonGia>())
                .List<DichVu21DonGia>();
            return result;
        }

        public IEnumerable<COCAUGIA_THUCTE_CT_QUERY> GetCocauGiaThucTe(int month, int year, string maCskcb, string maDichVu)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.AppendFormat(
                "SELECT thucte_ct.ID_LOAI, thucte_ct.MA_COCAU , thucte_ct.TEN_COCAU_OLD, SUM(thucte_ct.SO_LUONG_OLD * bhyt21.SOLUONG) SO_LUONG_OLD, thucte_ct.DONGIA_OLD, thucte_ct.TEN_COCAU_NEW, SUM(thucte_ct.SO_LUONG_NEW * bhyt21.SOLUONG) SO_LUONG_NEW, thucte_ct.DONGIA_NEW, loai.ten TEN_LOAI " +
                "FROM COCAUGIA_THUCTE thucte, COCAUGIA_THUCTE_CT thucte_ct, COCAUGIA_DICHVU21 dichvu21, COCAUGIA_LOAI loai, DM_COSOKCB cskcb, BHYT_21 bhyt21 " +
                "WHERE thucte_ct.ID_THUCTE = thucte.ID AND thucte_ct.ID_LOAI = loai.ID " +
                "AND dichvu21.ID = thucte.ID_DV AND bhyt21.COSOKCB_ID = cskcb.ID AND SUBSTR(bhyt21.MADVKT, -4, 4) = SUBSTR(dichvu21.MA_DICHVU21, -4, 4) " +
                "AND bhyt21.MADVKT='{0}' AND bhyt21.THANG_QT={1} AND bhyt21.NAM_QT={2}", maDichVu, month, year);

            if (!string.IsNullOrEmpty(maCskcb))
            {
                sqlQuery.AppendFormat(" AND cskcb.MA = '{0}'", maCskcb);
            }

            sqlQuery.Append(" GROUP BY thucte_ct.MA_COCAU , thucte_ct.TEN_COCAU_OLD, thucte_ct.ID_LOAI, thucte_ct.DONGIA_NEW, thucte_ct.DONGIA_OLD, thucte_ct.TEN_COCAU_NEW, loai.TEN");

            var result = NHibernateSession.CreateSQLQuery(sqlQuery.ToString())
                .SetResultTransformer(Transformers.AliasToBean<COCAUGIA_THUCTE_CT_QUERY>())
                .List<COCAUGIA_THUCTE_CT_QUERY>();
            return result;
        }

    }
}
