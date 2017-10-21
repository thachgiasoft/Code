using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIS.Core.Domain;
using EIS.Core.IService;
using FX.Core;
using NHibernate.Proxy;
using StackExchange.Redis;
using Newtonsoft.Json;
using log4net;
using IdentityManagement.Service;
using IdentityManagement.Domain;
using System.Diagnostics;

namespace EIS.Core
{
    public class GetSetRedis
    {
        private static IDatabase db;
        private static ConnectionMultiplexer connect;
        private static volatile GetSetRedis instance;
        private static object syncRoot = new Object();

        //IRedisClient redisClient;

        private  INGUOIDUNGService _iNguoidungService;
        private  IDM_DONVIService _iDonviService;
        private  IuserService _userDataService;
        private  IDMCOSOKCBService _iDmcosokcbServcie;
        private  IroleService _iRoleService;
        private static readonly ILog log = LogManager.GetLogger(typeof(GetSetRedis));

        public GetSetRedis()
        {
            //KhoiTao();
            Connect();
        }

        private void KhoiTao()
        {
     
            _iNguoidungService = IoC.Resolve<INGUOIDUNGService>();
            _userDataService = IoC.Resolve<IuserService>();
            _iDonviService = IoC.Resolve<IDM_DONVIService>();
            _iDmcosokcbServcie = IoC.Resolve<IDMCOSOKCBService>();
            _iRoleService = IoC.Resolve<IroleService>();
        }

        public static GetSetRedis Instance()
        {
            try
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new GetSetRedis();
                    }
                }
                else if (instance != null && !IsConnect())
                {
                    Connect();
                }
                return instance;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public static void Connect()
        {
            StringBuilder configString = new StringBuilder();
            var host = ConfigurationManager.AppSettings["REDIS_HOST"];
            var password = ConfigurationManager.AppSettings["REDIS_PASSWORD"];
            var port = ConfigurationManager.AppSettings["REDIS_PORT"];

            configString.Append(host).Append(":").Append(port).Append(",Password=").Append(password);
            ConfigurationOptions config = ConfigurationOptions.Parse(configString.ToString());
            config.AbortOnConnectFail = false;
            config.ConnectTimeout = 2 * 1000;
            config.KeepAlive = 180;
            //Giải phóng trước khi khởi tạo connect mới
            if (connect != null)
                connect.Close(false);
            connect = ConnectionMultiplexer.Connect(config);
            db = connect.GetDatabase();
        }

        public static bool IsConnect()
        {
            if (connect != null)
            {
                return connect.IsConnected;
            }
            return false;
        }


        public void Close()
        {
            if (connect != null)
            {
                connect.Close(false);
                connect.Dispose();
            }
        }

        public bool PushRedis<T>(string key, T obj)
        {
            try
            {
                if (obj != null && !string.IsNullOrEmpty(key))
                {
                    var value = JsonConvert.SerializeObject(obj);
                    return db.StringSet(key, value);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
            return false;
        }
        public bool PushNguoiDung(string key, NGUOIDUNG nguoidung)
        {
            try
            {
                //KhoiTao();
                _iDonviService = IoC.Resolve<IDM_DONVIService>();
                _iDmcosokcbServcie = IoC.Resolve<IDMCOSOKCBService>();
                if (!string.IsNullOrEmpty(key) && nguoidung != null)
                {
                    if (nguoidung.COSOKCB != null)
                    {
                        nguoidung.COSOKCB.COSOKCBCHA = null;
                        //nguoidung.COSOKCB.DM_DONVIHANHCHINH = null;
                        //nguoidung.COSOKCB.DM_TINHTHANH = null;
                        //nguoidung.COSOKCB.DM_QUANHUYEN = null;
                        if (nguoidung.COSOKCB.DM_DONVI != null)
                        {
                            DM_DONVI donvi = new DM_DONVI();
                            donvi = _iDonviService.Getbykey(nguoidung.COSOKCB.DM_DONVI.ID);
                            if (donvi != null)
                            {
                                donvi.DONVICHA = null;
                               // donvi.TINHTHANH = null;
                                nguoidung.COSOKCB.DM_DONVI = ConvertDmDonVi(donvi);
                            }
                        }
                        nguoidung.COSOKCB.DM_DONVI = null;
                    }
                    for (int i = 0; i < nguoidung.COSOKCBS.Count; i++)
                    {
                        if (nguoidung.COSOKCBS[i].IsProxy())
                        {
                            DM_COSOKCB cskcb = _iDmcosokcbServcie.Getbykey(nguoidung.COSOKCBS[i].ID);
                            if (cskcb != null)
                            {
                                
                                cskcb.COSOKCBCHA = null;
                                //cskcb.DM_DONVIHANHCHINH = null;
                                //cskcb.DM_TINHTHANH = null;
                                //cskcb.DM_QUANHUYEN = null;
                                if (cskcb.DM_DONVI != null)
                                {
                                    DM_DONVI donvi = new DM_DONVI();
                                    donvi = _iDonviService.Getbykey(nguoidung.COSOKCBS[i].DM_DONVI.ID);
                                    if (donvi != null)
                                    {
                                        donvi.DONVICHA = null;
                                      //  donvi.TINHTHANH = null;
                                        cskcb.DM_DONVI = ConvertDmDonVi(donvi);
                                    }
                                }
                                nguoidung.COSOKCBS[i] = ConvertDmCSKCB(cskcb);
                            }
                        }
                        else
                        {
                            nguoidung.COSOKCBS[i].COSOKCBCHA = null;
                            //nguoidung.COSOKCBS[i].DM_DONVIHANHCHINH = null;
                            //nguoidung.COSOKCBS[i].DM_TINHTHANH = null;
                            //nguoidung.COSOKCBS[i].DM_QUANHUYEN = null;
                            if (nguoidung.COSOKCBS[i].DM_DONVI != null)
                            {
                                if (nguoidung.COSOKCBS[i].DM_DONVI.IsProxy())
                                {
                                    DM_DONVI donvi = new DM_DONVI();
                                    donvi = _iDonviService.Getbykey(nguoidung.COSOKCBS[i].DM_DONVI.ID);
                                    if (donvi != null)
                                    {
                                        donvi.DONVICHA = null;
                                        //donvi.TINHTHANH = null;
                                        nguoidung.COSOKCBS[i].DM_DONVI = ConvertDmDonVi(donvi);
                                    }
                                }
                                else
                                {
                                    nguoidung.COSOKCBS[i].DM_DONVI.DONVICHA = null;
                                   // nguoidung.COSOKCBS[i].DM_DONVI.TINHTHANH = null;
                                }
                            }
                        }
                    }
                    var value = JsonConvert.SerializeObject(nguoidung);
                    return db.StringSet(key, value);
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }
        public bool PushUserData(string key, user userdata)
        {
            try
            {
                _iRoleService = IoC.Resolve<IroleService>();
                userdata.ApplicationList = null;
                userdata.Sessions = null;
                userdata.UserProfiles = null;
                for (int i = 0; i < userdata.Roles.Count; i ++)
                {
                    if (userdata.Roles[i].IsProxy())
                    {
                        role rol = _iRoleService.Getbykey(userdata.Roles[i].roleid);
                        if (rol != null)
                        {
                            rol.Permissions = null;
                            rol.Users = null;
                            rol.Sessions = null;
                            userdata.Roles[i] = ConvertRole(rol);
                        }
                    }
                    else
                    {
                        userdata.Roles[i].Permissions = null;
                        userdata.Roles[i].Users = null;
                        userdata.Roles[i].Sessions = null;
                    }
                   
                }
                var value = JsonConvert.SerializeObject(userdata);
                return db.StringSet(key, value);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        public bool PushRedisAll<T>(Dictionary<string, T> dic)
        {
            try
            {
                if (dic != null)
                {
                    var list = dic.ToArray();
                    foreach (var m in list)
                    {
                        PushRedis<T>(m.Key, m.Value);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        public T GetRedis<T>(string key)
        {
            try
            {
                if (!IsConnect())
                    Connect();
                RedisValue obj = db.StringGet(key);

                if (obj.IsNull)
                {
                    return default(T);
                }
                return JsonConvert.DeserializeObject<T>(obj);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return default(T);
            }
        }
        public List<T> GetValues<T>(List<string> keys)
        {
            try
            {
                List<T> models = new List<T>();
                if (keys.Count > 0)
                    foreach (var m in keys)
                    {
                        var model = GetRedis<T>(m);
                        if (model != null)
                            models.Add(model);
                    }
                return models;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        //public T GetRedisByKey<T>(string key)
        //{
        //    return redisClient.Get<T>(key);
        //}

        public bool PushRedisAddSet<T>(string key, T obj)
        {
            try
            {
                Object nObj = new Object();
                if (!String.IsNullOrEmpty(key) && obj != null)
                    if (GetRedis<T>(key).Equals(obj))
                    {
                        if (db.KeyDelete(key))
                            return PushRedis<T>(key, obj);
                    }
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }
        }

        public bool RemoveRedisKey(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                    return db.KeyDelete(key);
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }

        }
        public List<string> GetAllKeys()
        {
            List<string> keys = new List<string>();
            foreach (var n in connect.GetEndPoints())
            {
                var server = connect.GetServer(n);
                var list = server.Keys().ToArray();
                for (int i = 0; i < list.Count(); i++)
                {
                    keys.Add(list[i]);
                }
            }
            return keys;
        }

        public bool CheckExitsKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
                return db.KeyExists(key);
            return false;
        }
        public List<string> ListKeyContainsInput(string input)
        {
            List<string> lstRedisKey = GetAllKeys();
            List<string> lst = new List<string>();
            foreach (var item in lstRedisKey)
            {
                if (item.ToString().Contains(input))
                {
                    lst.Add(item);
                }
            }
            return lst;
        }

        public List<string> ListKeyContainsMutilInput(string input1, string input2)
        {
            List<string> lstRedisKey = GetAllKeys();
            List<string> lst = new List<string>();
            foreach (var item in lstRedisKey)
            {
                if (item.ToString().Contains(input1) && item.ToString().Contains(input2))
                {
                    lst.Add(item);
                }
            }
            return lst;
        }
        public bool DeleteAll()
        {
            foreach (var n in connect.GetEndPoints())
            {
                var server = connect.GetServer(n);
                server.FlushAllDatabases();
            }
            return true;
        }

        public bool Expire(string key)
        {
            return db.KeyExpire(key, TimeSpan.FromSeconds(10));
        }
        /// <summary>
        /// Hàm lấy dữ liệu redis theo key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        //public T GetDataRedisByKey<T>(string key)
        //{
        //    try
        //    {
        //        //KhoiTao();
        //        //đưa resolve service từ khởi tạo vào hàm
        //        //_iDmFunctionService = IoC.Resolve<IDMFUNCTIONService>();
        //        //_iDmLydoService = IoC.Resolve<IDMLYDOService>();
        //        //_iDmLuongtoithieuService = IoC.Resolve<IDMLUONGTOITHIEUService>();
        //        //_iDmMuchuongBHYTService = IoC.Resolve<IDMMUCHUONGBHYTService>();
        //        //_iDmTyleTTTraiTuyenService = IoC.Resolve<IDMTYLETTTRAITUYENService>();
        //        //_iDmIcdService = IoC.Resolve<IDMICDService>();
        //        //_iDmVattuService = IoC.Resolve<IDMVATTUService>();
        //        //_iDmVattuBVService = IoC.Resolve<IDMVATTUBVService>();
        //        int count = 0;
        //        int indexOfChar = key.IndexOf('@');
        //        if (indexOfChar != -1)
        //        {
        //            key = key.Substring(0, key.Length - 3);
        //        }
        //        T rs = GetRedis<T>(key);
        //        if (rs != null)
        //        {
        //            var pro = rs.GetType().GetProperty("Count");
        //            if (pro != null)
        //            {
        //                var c = pro.GetValue(rs, null);
        //                count = Convert.ToInt32(c);
        //            }
        //            else
        //            {
        //                count = 1;
        //            }
        //        }
        //        //nếu không có
        //        if (rs != null && count > 0)
        //        {
        //            return rs;
        //        }
        //        else
        //        {
        //            //tiến hành lấy dữ liệu trong db
        //            //bước 1: phân tích key -> loại đối tượng
        //            int index = key.IndexOf('_');
        //            string type = "";
        //            if (index == -1)
        //            {
        //                type = key;
        //            }
        //            else
        //            {
        //                type = key.Substring(0, index);
        //            }

        //            switch (type)
        //            {
        //                case "DMFUNCTION":
        //                    if (index != -1)
        //                    {
        //                        //get keyDmFun
        //                        string keyDmFun = key.Substring(key.IndexOf('_') + 1, key.Length - index - 1);
        //                        // lấy dữ liệu bảng Dm function theo key
        //                      //  var dataDmFunc = _iDmFunctionService.getByKey(keyDmFun);
        //                        //if (dataDmFunc != null)
        //                        //{
        //                        //    // đẩy lên redis
        //                        //    if (dataDmFunc.LYDO != null)
        //                        //    {
        //                        //        dataDmFunc.LYDO = null;
        //                        //    }
        //                        //    if (db.KeyExists(key))
        //                        //    {
        //                        //      //  PushRedisAddSet<DM_FUNCTION>(key, dataDmFunc);
        //                        //    }
        //                        //    else
        //                        //    {
        //                        //       // PushRedis<DM_FUNCTION>(key, dataDmFunc);
        //                        //    }
        //                        //    rs = GetRedis<T>(key);
        //                        //    return rs;
        //                        //}
        //                       // else
        //                       /// {
        //                            return rs;
        //                       // }
        //                    }
        //                    else
        //                    {
        //                        //lấy all dữ liệu bảng dm function
        //                      //  var dataDmFuncAll = _iDmFunctionService.getAll().ToList();
        //                        if (dataDmFuncAll != null && dataDmFuncAll.Count > 0)
        //                        {
        //                            foreach (var func in dataDmFuncAll)
        //                            {
        //                                if (func.LYDO != null)
        //                                {
        //                                    func.LYDO = null;
        //                                }
        //                            }
        //                            if (db.KeyExists(key))
        //                            {
        //                                PushRedisAddSet<List<DM_FUNCTION>>(key, dataDmFuncAll);
        //                            }
        //                            else
        //                            {
        //                                PushRedis<List<DM_FUNCTION>>(key, dataDmFuncAll);
        //                            }
        //                            rs = GetRedis<T>(key);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }
        //                case "DMLYDO":
        //                    if (index != -1)
        //                    {
        //                        //get idDmLydo
        //                        string idDmLydo = key.Substring(key.IndexOf('_') + 1, key.Length - index - 1);
        //                        long id = Convert.ToInt64(idDmLydo);
        //                        //DM_LYDO dataDmLydo = _iDmLydoService.Query.Where(b => b.ID == 211).FirstOrDefault();
        //                        //lấy dữ liệu bảng DmLyDo theo Id
        //                        DM_LYDO dataDmLydo = _iDmLydoService.getDataByID(id);
        //                        if (dataDmLydo != null)
        //                        {
        //                            DM_LYDO datald = new DM_LYDO();
        //                            if (dataDmLydo.IsProxy())
        //                            {
        //                                //thực hiện convert
        //                                CopyPropertyValues(dataDmLydo, datald);
        //                            }
        //                            else
        //                            {
        //                                datald = dataDmLydo;
        //                            }
        //                            if (datald != null)
        //                            {
        //                                //đẩy lên redis
        //                                if (db.KeyExists(key))
        //                                {
        //                                    PushRedisAddSet<DM_LYDO>(key, datald);
        //                                }
        //                                else
        //                                {
        //                                    PushRedis<DM_LYDO>(key, datald);
        //                                }
        //                            }
        //                            rs = GetRedis<T>(key);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //lấy dữ liệu all bảng dm lý do
        //                        List<DM_LYDO> dataDmLyDoAll = _iDmLydoService.getAll();
        //                        if (dataDmLyDoAll != null && dataDmLyDoAll.Count > 0)
        //                        {
        //                            List<DM_LYDO> lstDataDmLyDoAll = new List<DM_LYDO>();
        //                            for (int i = 0; i < dataDmLyDoAll.ToArray().Length; i++)
        //                            {
        //                                if (dataDmLyDoAll[i].IsProxy())
        //                                {
        //                                    //thực hiện convert
        //                                    CopyPropertyValues(dataDmLyDoAll[i], dataDmLyDoAll[i]);
        //                                    lstDataDmLyDoAll.Add(dataDmLyDoAll[i]);
        //                                }
        //                                else
        //                                {
        //                                    lstDataDmLyDoAll.Add(dataDmLyDoAll[i]);
        //                                }
        //                            }
        //                            //đẩy lên redis
        //                            if (db.KeyExists(key))
        //                            {
        //                                PushRedisAddSet<List<DM_LYDO>>(key, lstDataDmLyDoAll);
        //                            }
        //                            else
        //                            {
        //                                PushRedis<List<DM_LYDO>>(key, lstDataDmLyDoAll);
        //                            }
        //                            rs = GetRedis<T>(key);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }
        //                case "DMLUONGTOITHIEU":
        //                    //lấy toàn bộ dữ liệu bảng Dm lương tối thiểu
        //                    var lstDataDmLuongCB = _iDmLuongtoithieuService.getAll().ToList();
        //                    if (lstDataDmLuongCB != null && lstDataDmLuongCB.Count > 0)
        //                    {
        //                        //đẩy lên redis
        //                        if (db.KeyExists(key))
        //                        {
        //                            PushRedisAddSet<List<DM_LUONGTOITHIEU>>(key, lstDataDmLuongCB);
        //                        }
        //                        else
        //                        {
        //                            PushRedis<List<DM_LUONGTOITHIEU>>(key, lstDataDmLuongCB);
        //                        }
        //                        rs = GetRedis<T>(key);
        //                        return rs;
        //                    }
        //                    else
        //                    {
        //                        return rs;
        //                    }
        //                case "DMMUCHUONGBHYT":
        //                    //get idDmMucHuongBHYT
        //                    string tenDmMh = key.Substring(key.IndexOf('_') + 1, key.Length - index - 1);
        //                    //lấy dữ liệu của bảng dm mức hưởng bhyt theo id
        //                    var dataDmMucHuong = _iDmMuchuongBHYTService.getDmMucHuongBHYTByTen(tenDmMh);
        //                    if (dataDmMucHuong != null)
        //                    {
        //                        if (dataDmMucHuong.DM_NHOMDTBHYT != null)
        //                        {
        //                            dataDmMucHuong.DM_NHOMDTBHYT = null;
        //                        }
        //                        //đẩy lên redis
        //                        if (db.KeyExists(key))
        //                        {
        //                            PushRedisAddSet<DM_MUCHUONGBHYT>(key, dataDmMucHuong);
        //                        }
        //                        else
        //                        {
        //                            PushRedis<DM_MUCHUONGBHYT>(key, dataDmMucHuong);
        //                        }
        //                        rs = GetRedis<T>(key);
        //                        return rs;
        //                    }
        //                    else
        //                    {
        //                        return rs;
        //                    }
        //                case "DMTYLETTTRAITUYEN":
        //                    //get idTyleTTTT
        //                    if (index != -1)
        //                    {
        //                        string idTyleTTT = key.Substring(key.IndexOf('_') + 1, key.Length - index - 1);
        //                        long idTyle = Convert.ToInt64(idTyleTTT);
        //                        //lấy dữ liệu của bảng dm tỷ lệ thanh toán trái tuyến theo id
        //                        var dataDmTyle = _iDmTyleTTTraiTuyenService.getDataByID(idTyle);
        //                        if (dataDmTyle != null)
        //                        {
        //                            //đẩy lên redis
        //                            if (db.KeyExists(key))
        //                            {
        //                                PushRedisAddSet<DM_TYLETTTRAITUYEN>(key, dataDmTyle);
        //                            }
        //                            else
        //                            {
        //                                PushRedis<DM_TYLETTTRAITUYEN>(key, dataDmTyle);
        //                            }
        //                            rs = GetRedis<T>(key);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //lấy tất cả dữ liệu bảng DM Ty le thanh toan trai tuyen
        //                        var dataDmTyleAll = _iDmTyleTTTraiTuyenService.GetAll();
        //                        if (dataDmTyleAll != null && dataDmTyleAll.Count > 0)
        //                        {
        //                            //đẩy lên redis
        //                            if (db.KeyExists(key))
        //                            {
        //                                PushRedisAddSet<List<DM_TYLETTTRAITUYEN>>(key, dataDmTyleAll);
        //                            }
        //                            else
        //                            {
        //                                PushRedis<List<DM_TYLETTTRAITUYEN>>(key, dataDmTyleAll);
        //                            }
        //                            rs = GetRedis<T>(key);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }

        //                case "DMICD":
        //                    if (indexOfChar == -1)
        //                    {
        //                        //get id DM ICD
        //                        string ma = key.Substring(key.IndexOf('_') + 1, key.Length - index - 1);
        //                        //lấy dữ liệu của bảng dm ICD theo id
        //                        var dataICD = _iDmIcdService.getByMa(ma);
        //                        if (dataICD != null)
        //                        {
        //                            if (dataICD.DM_ICDNHOM != null)
        //                            {
        //                                dataICD.DM_ICDNHOM = null;
        //                            }
        //                            if (dataICD.DM_CHUYENKHOA != null)
        //                            {
        //                                dataICD.DM_CHUYENKHOA = null;
        //                            }
        //                            //đẩy lên redis
        //                            if (db.KeyExists(key))
        //                            {
        //                                PushRedisAddSet<DM_ICD>(key, dataICD);
        //                            }
        //                            else
        //                            {
        //                                PushRedis<DM_ICD>(key, dataICD);
        //                            }
        //                            rs = GetRedis<T>(key);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //get ma dm ICD
        //                        //trừ 2 kí tự cuối cùng là _@
        //                        string idICD = key.Substring(key.IndexOf('_') + 1, key.Length - index - 1);
        //                        string keyDmICD = key.Substring(0, key.Length - 3);
        //                        long idIcd = Convert.ToInt64(idICD);
        //                        var dataICD = _iDmIcdService.getByID(idIcd);
        //                        if (dataICD != null)
        //                        {
        //                            //đẩy lên redis
        //                            if (dataICD.DM_ICDNHOM != null)
        //                            {
        //                                dataICD.DM_ICDNHOM = null;
        //                            }
        //                            if (dataICD.DM_CHUYENKHOA != null)
        //                            {
        //                                dataICD.DM_CHUYENKHOA = null;
        //                            }
        //                            if (db.KeyExists(keyDmICD))
        //                            {
        //                                PushRedisAddSet<DM_ICD>(keyDmICD, dataICD);
        //                            }
        //                            else
        //                            {
        //                                PushRedis<DM_ICD>(keyDmICD, dataICD);
        //                            }
        //                            rs = GetRedis<T>(keyDmICD);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }
        //                case "DMVATTU":
        //                    if (indexOfChar == -1)
        //                    {
        //                        //get ma Dm vattu
        //                        string maDmvt = key.Substring(key.IndexOf('_') + 1, key.Length - index - 1);
        //                        var dataDmvt = _iDmVattuService.getByMa(maDmvt);
        //                        if (dataDmvt != null)
        //                        {
        //                            if (dataDmvt.DM_NHOMVATTU != null)
        //                            {
        //                                dataDmvt.DM_NHOMVATTU = null;
        //                            }
        //                            if (dataDmvt.DM_VANBANQUYETDINH != null)
        //                            {
        //                                dataDmvt.DM_VANBANQUYETDINH = null;
        //                            }
        //                            //đẩy lên redis
        //                            if (db.KeyExists(key))
        //                            {
        //                                PushRedisAddSet<DM_VATTU>(key, dataDmvt);
        //                            }
        //                            else
        //                            {
        //                                PushRedis<DM_VATTU>(key, dataDmvt);
        //                            }
        //                            rs = GetRedis<T>(key);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //get Id dm vattu
        //                        string keyDmvt = key.Substring(0, key.Length - 3);
        //                        string idDmvt = key.Substring(key.IndexOf('_') + 1, key.Length - index - 1 - 2);
        //                        long idDmvattu = Convert.ToInt64(idDmvt);
        //                        //lấy dữ liệu bảng dm vattu theo id
        //                        var dataDmvattu = _iDmVattuService.getByID(idDmvattu);
        //                        if (dataDmvattu != null)
        //                        {
        //                            if (dataDmvattu.DM_NHOMVATTU != null)
        //                            {
        //                                dataDmvattu.DM_NHOMVATTU = null;
        //                            }
        //                            if (dataDmvattu.DM_VANBANQUYETDINH != null)
        //                            {
        //                                dataDmvattu.DM_VANBANQUYETDINH = null;
        //                            }
        //                            //đẩy lên redis
        //                            if (db.KeyExists(key))
        //                            {
        //                                PushRedisAddSet<DM_VATTU>(key, dataDmvattu);
        //                            }
        //                            else
        //                            {
        //                                PushRedis<DM_VATTU>(key, dataDmvattu);
        //                            }
        //                            rs = GetRedis<T>(key);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }
        //                case "DMVATTUBV":
        //                    //lấy theo mã
        //                    if (indexOfChar == -1)
        //                    {
        //                        //get ma Dm vattuBV
        //                        string maDmvtBV = key.Substring(key.IndexOf('_') + 1, key.Length - index - 1);
        //                        var dataDmvtBV = _iDmVattuBVService.getByMabv(maDmvtBV);
        //                        if (dataDmvtBV != null)
        //                        {
        //                            if (dataDmvtBV.HOSO != null)
        //                            {
        //                                dataDmvtBV.HOSO = null;
        //                            }
        //                            if (dataDmvtBV.DM_COSOKCB != null)
        //                            {
        //                                dataDmvtBV.DM_COSOKCB = null;
        //                            }
        //                            if (dataDmvtBV.NHOMVATTU != null)
        //                            {
        //                                dataDmvtBV.NHOMVATTU = null;
        //                            }
        //                            if (dataDmvtBV.VATTU != null)
        //                            {
        //                                dataDmvtBV.VATTU = null;
        //                            }
        //                            //đẩy lên redis
        //                            if (db.KeyExists(key))
        //                            {
        //                                PushRedisAddSet<DM_VATTU_BV>(key, dataDmvtBV);
        //                            }
        //                            else
        //                            {
        //                                PushRedis<DM_VATTU_BV>(key, dataDmvtBV);
        //                            }
        //                            rs = GetRedis<T>(key);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //get Id dm vattuBV
        //                        string keyDmvtBV = key.Substring(0, key.Length - 3);
        //                        string idDmvtBV = key.Substring(key.IndexOf('_') + 1, key.Length - index - 1 - 2);
        //                        long idDmvattuBV = Convert.ToInt64(idDmvtBV);
        //                        //lấy dữ liệu bảng dm vattuBV theo id
        //                        var dataDmvattuBV = _iDmVattuBVService.getByID(idDmvattuBV);
        //                        if (dataDmvattuBV != null)
        //                        {
        //                            if (dataDmvattuBV.HOSO != null)
        //                            {
        //                                dataDmvattuBV.HOSO = null;
        //                            }
        //                            if (dataDmvattuBV.DM_COSOKCB != null)
        //                            {
        //                                dataDmvattuBV.DM_COSOKCB = null;
        //                            }
        //                            if (dataDmvattuBV.NHOMVATTU != null)
        //                            {
        //                                dataDmvattuBV.NHOMVATTU = null;
        //                            }
        //                            if (dataDmvattuBV.VATTU != null)
        //                            {
        //                                dataDmvattuBV.VATTU = null;
        //                            }
        //                            //đẩy lên redis
        //                            if (db.KeyExists(key))
        //                            {
        //                                PushRedisAddSet<DM_VATTU_BV>(key, dataDmvattuBV);
        //                            }
        //                            else
        //                            {
        //                                PushRedis<DM_VATTU_BV>(key, dataDmvattuBV);
        //                            }
        //                            rs = GetRedis<T>(key);
        //                            return rs;
        //                        }
        //                        else
        //                        {
        //                            return rs;
        //                        }
        //                    }
        //                //lấy theo id
        //                default:
        //                    return rs;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex.Message);
        //        return default(T);
        //    }
        //}
        public NGUOIDUNG getDataNguoiDung(string tenDN)
        {
            try
            {
                _iNguoidungService = IoC.Resolve<INGUOIDUNGService>();
                string key = "NGUOIDUNG_" + tenDN;
                NGUOIDUNG nguoidung = GetRedis<NGUOIDUNG>(key);
                if (nguoidung != null)
                {
                    return nguoidung;
                }
                else
                {
                    nguoidung = _iNguoidungService.Query.Where(o => o.TENDANGNHAP == tenDN).FirstOrDefault();
                    if (nguoidung != null)
                    {
                        PushRedis<NGUOIDUNG>("NGUOIDUNG_" + tenDN, nguoidung);
                    }
                    return nguoidung;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                var nguoidung = _iNguoidungService.Query.Where(o => o.TENDANGNHAP == tenDN).FirstOrDefault();
                return nguoidung;
            }
        }

        public user getDataUserData(string username)
        {
            try
            {
                _userDataService = IoC.Resolve<IuserService>();
                string key = "USERDATA_" + username;
                user userdata = GetRedis<user>(key);
                if (userdata != null)
                {
                    return userdata;
                }
                else
                {
                    userdata = _userDataService.GetByName(username);
                    if (userdata != null)
                    {
                        PushRedis<user>("USERDATA" + username, userdata);
                    }
                    return userdata;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                var userdata = _userDataService.GetByName(username);
                return userdata;
            }
        }
        private static void CopyPropertyValues(object source, object destination)
        {
            var destProperties = destination.GetType().GetProperties();

            foreach (var sourceProperty in source.GetType().GetProperties())
            {
                foreach (var destProperty in destProperties)
                {
                    if (destProperty.Name == sourceProperty.Name &&
                destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    {
                        destProperty.SetValue(destination, sourceProperty.GetValue(
                            source, new object[] { }), new object[] { });

                        break;
                    }
                }
            }
        }
        private static DM_DONVI ConvertDmDonVi(DM_DONVI donvi)
        {
            DM_DONVI dmdonvi = new DM_DONVI();
            dmdonvi.ID = donvi.ID;
            dmdonvi.TEN = donvi.TEN;
            dmdonvi.MA = donvi.MA;
            dmdonvi.MIEUTA = donvi.MIEUTA;
            dmdonvi.HIEULUC = donvi.HIEULUC;
            dmdonvi.DONVICHA_ID = donvi.DONVICHA_ID;
            dmdonvi.STT = donvi.STT;
            dmdonvi.MACHA = donvi.MACHA;
            dmdonvi.TINHTHANH_ID = donvi.TINHTHANH_ID;
            return dmdonvi;
        }
        private static DM_COSOKCB ConvertDmCSKCB(DM_COSOKCB cskcb)
        {
            DM_COSOKCB dmcskcb = new DM_COSOKCB();
            dmcskcb.ID = cskcb.ID;
            dmcskcb.TEN = cskcb.TEN;
            dmcskcb.MA = cskcb.MA;
            dmcskcb.MABHYT = cskcb.MABHYT;
            dmcskcb.DONVIHANHCHINH_ID = cskcb.DONVIHANHCHINH_ID;
            dmcskcb.DIACHI = cskcb.DIACHI;
            dmcskcb.HANGBENHVIEN = cskcb.HANGBENHVIEN;
            dmcskcb.TUYENCMKT = cskcb.TUYENCMKT;
            dmcskcb.HIEULUC = cskcb.HIEULUC;
            dmcskcb.STT = cskcb.STT;
            dmcskcb.MIEUTA = cskcb.MIEUTA;
            dmcskcb.TINHTHANH_ID = cskcb.TINHTHANH_ID;
            dmcskcb.DONVI_ID = cskcb.DONVI_ID;
            dmcskcb.QUANHUYEN_ID = cskcb.QUANHUYEN_ID;
            dmcskcb.MAQUANHUYEN = cskcb.MAQUANHUYEN;
            dmcskcb.MATINHTHANH = cskcb.MATINHTHANH;
            dmcskcb.MADONVI = cskcb.MADONVI;
            dmcskcb.DM_DONVI = cskcb.DM_DONVI;
            dmcskcb.MACOSOKCBCHA = cskcb.MACOSOKCBCHA;
            dmcskcb.COSOKCBCHA_ID = cskcb.COSOKCBCHA_ID;
            dmcskcb.THANNHANTAO = cskcb.THANNHANTAO;
            dmcskcb.THAIGHEP = cskcb.THAIGHEP;
            dmcskcb.LOAIHOPDONG = cskcb.LOAIHOPDONG;
            dmcskcb.DKKCBBD = cskcb.DKKCBBD;
            dmcskcb.HINHTHUCTT = cskcb.HINHTHUCTT;
            dmcskcb.LOAIBENHVIEN = cskcb.LOAIBENHVIEN;
            dmcskcb.KHAMTREEM = cskcb.KHAMTREEM;
            dmcskcb.NGAYNGUNGHD = cskcb.NGAYNGUNGHD;
            dmcskcb.MATAICHINH = cskcb.MATAICHINH;
            dmcskcb.DIENTHOAI = cskcb.DIENTHOAI;
            dmcskcb.MASOTHUE = cskcb.MASOTHUE;
            dmcskcb.EMAIL = cskcb.EMAIL;
            dmcskcb.FAX = cskcb.FAX;
            dmcskcb.PKDAKHOA = cskcb.PKDAKHOA;
            dmcskcb.UNGTHU = cskcb.UNGTHU;
            dmcskcb.VIEMGAN = cskcb.VIEMGAN;
            dmcskcb.TEBAOMAUTD = cskcb.TEBAOMAUTD;
            dmcskcb.KHAMT7 = cskcb.KHAMT7;
            dmcskcb.KHAMCN = cskcb.KHAMCN;
            dmcskcb.KHAMNGAYLE = cskcb.KHAMNGAYLE;
            dmcskcb.KIEUBV = cskcb.KIEUBV;
            dmcskcb.CAPCSKCB_MIN = cskcb.CAPCSKCB_MIN;
            dmcskcb.SOHOPDONG = cskcb.SOHOPDONG;
            dmcskcb.COQUANCHUQUAN = cskcb.COQUANCHUQUAN;
            dmcskcb.TTPHEDUYET = cskcb.TTPHEDUYET;
            dmcskcb.LYDO = cskcb.LYDO;
            dmcskcb.TRANGTHAI = cskcb.TRANGTHAI;
            dmcskcb.NGAYKYHOPDONG = cskcb.NGAYKYHOPDONG;
            dmcskcb.TUCHU = cskcb.TUCHU;
            dmcskcb.HANGDICHVU_TD = cskcb.HANGDICHVU_TD;
            dmcskcb.HANGTHUOC_TD = cskcb.HANGTHUOC_TD;
            dmcskcb.HANGVATTU_TD = cskcb.HANGVATTU_TD;
            dmcskcb.BYT = cskcb.BYT;
            return dmcskcb;
        }
        private static role ConvertRole(role rol)
        {
            role r = new role();
            r.AppID = rol.AppID;
            r.name = rol.name;
            r.roleid = rol.roleid;
            return r;
        }
    }
}
