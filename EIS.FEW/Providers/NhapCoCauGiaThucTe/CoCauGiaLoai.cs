using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;

namespace EIS.FEW.Providers
{
    public class CoCauGiaLoai : ICoCauGiaLoai
    {
        public readonly ICOCAUGIA_LOAIService _ICOCAUGIA_LOAIService;
        
        public CoCauGiaLoai()
        {
            _ICOCAUGIA_LOAIService = IoC.Resolve<ICOCAUGIA_LOAIService>();
        }
        public COCAUGIA_LOAI GetCoCauGiaLoaiByID(long id)
        {
            COCAUGIA_LOAI loai = _ICOCAUGIA_LOAIService.getById(id).FirstOrDefault();
            return loai;
        }
    }
}