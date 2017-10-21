using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Data;

namespace EIS.Core.ServiceImp
{
    public class NHOMCHUCNANG_OBJECTService : BaseService<NHOMCHUCNANG_OBJECT, int>, INHOMCHUCNANG_OBJECTService
    {
        public NHOMCHUCNANG_OBJECTService(string sessionFactoryConfigPath)
              : base(sessionFactoryConfigPath)
        { }
    }
}
