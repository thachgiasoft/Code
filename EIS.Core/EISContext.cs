using EIS.Core.CustomView;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Context;
using FX.Core;
using System;
using System.Linq;

namespace EIS.Core
{
    public class EISContext : FXContext
    {
        NGUOIDUNG _nguoidung;
        public NGUOIDUNG CurrentNguoidung
        {
            get
            {
                if (_nguoidung == null && CurrentUser != null)
                {
                    var srv = IoC.Resolve<INGUOIDUNGService>();
                    var uName = CurrentUser.username;
                    if (!String.IsNullOrEmpty(uName))
                    {
                        _nguoidung = srv.Query.FirstOrDefault(a => a.TENDANGNHAP == uName);
                        if (_nguoidung != null)
                        {
                            _nguoidung.Time = new KyFilterSession
                            {
                                thoiGian = _nguoidung.DF_LOAITG,
                                nam = _nguoidung.DF_NAM,
                                thang = _nguoidung.DF_THANG,
                                quy = _nguoidung.DF_QUY
                            };
                        }
                    }
                }
                return _nguoidung;
            }
        }
    }
}
