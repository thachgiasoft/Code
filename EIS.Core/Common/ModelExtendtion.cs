using EIS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace EIS.Core.Common
{
    public static class ModelExtendtion
    {
        /// <summary>
        /// check 1 thuốc có quyền được thực hiện 1 tác vụ nào đó không với các trường trong db
        /// </summary>
        /// <param name="row">vật tư được chọn</param>
        /// <param name="tacvu">tác vụ: 1 duyệt, 2 huỷ duyệt, 3 khôi phục, 4 ánh xạ, 5 huỷ ánh xạ</param>
        /// <returns></returns>
        public static bool CheckQuyenButtunDM_VatTuBV(this DM_VATTU_BV row, int tacvu)
        {
            // 1
            if (row.AX_TD != true && row.AX_CD != true && ((row.KETQUA_TD == 0 && row.KETQUA == 0) || (row.KETQUA_TD == null && row.KETQUA == null)))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return true;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return true;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 2
            if (row.AX_TD != true && row.AX_CD == true && row.KETQUA_TD == 1 && row.KETQUA == 1)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return true;

                    case 2: // tu choi
                        return true;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return true;

                    case 5: // huy anh xa
                        return true;
                }
            }
            // 3, 4
            if (row.AX_TD != true && row.AX_CD == true && row.KETQUA_TD == 1 && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return true;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 5, 6
            if (row.AX_TD != true && row.AX_CD == true && row.KETQUA_TD == row.KETQUA && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return true;

                    case 5: // huy anh xa
                        return true;
                }
            }
            // 7, 8
            if (row.AX_TD == true && row.AX_CD != true && row.KETQUA_TD == row.KETQUA && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 9
            if (row.AX_TD == true && row.AX_CD != true && row.KETQUA_TD == 1 && row.KETQUA == 1)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return true;

                    case 2: // tu choi
                        return true;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 10, 11
            if (row.AX_TD == true && row.AX_CD != true && row.KETQUA_TD == 1 && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return true;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 12
            if (row.AX_TD != true && row.AX_CD != true && row.KETQUA_TD == 3 && row.KETQUA == 3)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 13
            if (row.AX_TD != true && row.AX_CD != true && (row.KETQUA_TD == 0 || row.KETQUA_TD == null) && row.KETQUA == 3)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return true;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            return false;
        }

        public static bool CheckQuyenButtunDM_VatTuThau(this DM_VATTU_THAU row, int tacvu)
        {
            // 1
            if (row.AX_TD != true && row.AX_CD != true && ((row.KETQUA_TD == 0 && row.KETQUA == 0) || (row.KETQUA_TD == null && row.KETQUA == null)))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return true;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return true;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 2
            if (row.AX_TD != true && row.AX_CD == true && row.KETQUA_TD == 1 && row.KETQUA == 1)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return true;

                    case 2: // tu choi
                        return true;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return true;

                    case 5: // huy anh xa
                        return true;
                }
            }
            // 3, 4
            if (row.AX_TD != true && row.AX_CD == true && row.KETQUA_TD == 1 && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return true;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 5, 6
            if (row.AX_TD != true && row.AX_CD == true && row.KETQUA_TD == row.KETQUA && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return true;

                    case 5: // huy anh xa
                        return true;
                }
            }
            // 7, 8
            if (row.AX_TD == true && row.AX_CD != true && row.KETQUA_TD == row.KETQUA && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 9
            if (row.AX_TD == true && row.AX_CD != true && row.KETQUA_TD == 1 && row.KETQUA == 1)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return true;

                    case 2: // tu choi
                        return true;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 10, 11
            if (row.AX_TD == true && row.AX_CD != true && row.KETQUA_TD == 1 && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return true;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 12
            if (row.AX_TD != true && row.AX_CD != true && row.KETQUA_TD == 3 && row.KETQUA == 3)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 13
            if (row.AX_TD != true && row.AX_CD != true && (row.KETQUA_TD == 0 || row.KETQUA_TD == null) && row.KETQUA == 3)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return true;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            return false;
        }

        public static bool duocAnhXa(this DM_VATTU_THAU model)
        {
            if (model.AX_TD != true && model.AX_CD != true && model.KETQUA_TD == 0 && model.KETQUA == 0)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 2 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 3 && model.KETQUA == 3)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 2 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 3 && model.KETQUA == 3)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD != true && model.KETQUA_TD == 3 && model.KETQUA == 3)
            {
                return true;
            }

            return false;
        }

        public static bool duocHuyAnhXa(this DM_VATTU_THAU model)
        {
            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 2 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 3 && model.KETQUA == 3)
            {
                return true;
            }

            return false;
        }

        public static bool duocChapNhan(this DM_VATTU_THAU model)
        {
            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }
            return false;
        }

        public static bool duocTuChoi(this DM_VATTU_THAU model)
        {
            if (model.AX_TD != true && model.AX_CD != true && model.KETQUA_TD == 0 && model.KETQUA == 0)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 2 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 2 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 2)
            {
                return true;
            }

            return false;
        }

        public static bool duocKhoiPhuc(this DM_VATTU_THAU model)
        {
            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 3)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 3)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD != true && model.KETQUA_TD == 0 && model.KETQUA == 3)
            {
                return true;
            }

            return false;
        }

        public static bool duocAnhXa(this DM_VATTU_BV model)
        {
            if (model.AX_TD != true && model.AX_CD != true && model.KETQUA_TD == 0 && model.KETQUA == 0)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 2 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 3 && model.KETQUA == 3)
            {
                return true;
            }

            return false;
        }

        public static bool duocHuyAnhXa(this DM_VATTU_BV model)
        {
            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 2 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 3 && model.KETQUA == 3)
            {
                return true;
            }

            return false;
        }

        public static bool duocChapNhan(this DM_VATTU_BV model)
        {
            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }
            return false;
        }

        public static bool duocTuChoi(this DM_VATTU_BV model)
        {
            if (model.HIEULUC)
            {
                return false;
            }
            if (model.AX_TD != true && model.AX_CD != true && model.KETQUA_TD == 0 && model.KETQUA == 0)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 2 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 2 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 1)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 2)
            {
                return true;
            }

            return false;
        }

        public static bool duocKhoiPhuc(this DM_VATTU_BV model)
        {
            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 1 && model.KETQUA == 3)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 2)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 1 && model.KETQUA == 3)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD != true && model.KETQUA_TD == 0 && model.KETQUA == 3)
            {
                return true;
            }

            if (model.AX_TD == true && model.AX_CD != true && model.KETQUA_TD == 2 && model.KETQUA == 3)
            {
                return true;
            }

            if (model.AX_TD != true && model.AX_CD == true && model.KETQUA_TD == 2 && model.KETQUA == 3)
            {
                return true;
            }

            return false;
        }

        public static bool CheckApply(this DM_VATTU_BV model)
        {
            if (model.KETQUA == 2 && (model.AX_CD == true || model.AX_TD == true))
            {
                return true;
            }
            return false;
        }

        public static bool CheckUnApply(this DM_VATTU_BV model)
        {
            if (model.HIEULUC == true && model.TRANGTHAI == 2)
            {
                return true;
            }
            return false;
        }

        public static bool CheckApply(this DM_VATTU_THAU model)
        {
            if ((model.AX_CD == true || model.AX_TD == true) && model.KETQUA == 2)
            {
                return true;
            }
            return false;
        }

        public static bool CheckUnApply(this DM_VATTU_THAU model)
        {
            if (model.HIEULUC == true && model.TRANGTHAI == 2)
            {
                return true;
            }
            return false;
        }

        #region Thuoc Thau
        public static bool CheckApDung(this DM_THUOC_THAU model)
        {
            if (model.KETQUA == 2 && (model.AX_CD == true || model.AX_TD == true))
            {
                return true;
            }
            return false;
        }
        public static bool CheckHuyApDung(this DM_THUOC_THAU model)
        {
            if (model.HIEULUC == true && model.TRANGTHAI == 2)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Thuoc BV
        public static bool CheckApDung(this DM_THUOC_BV model)
        {
            if (model.KETQUA == 2 && (model.AX_CD == true || model.AX_TD == true) && model.HIEULUC != true)
            {
                return true;
            }
            return false;
        }
        public static bool CheckHuyApDung(this DM_THUOC_BV model)
        {
            if (model.HIEULUC == true && model.TRANGTHAI == 2)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region DichVu BV
        public static bool CheckApDung(this DM_DICHVU_BV model)
        {
            if (model.KETQUA == 2 && (model.AX_CD == true || model.AX_TD == true))
            {
                return true;
            }
            return false;
        }
        public static bool CheckHuyApDung(this DM_DICHVU_BV model)
        {
            if (model.HIEULUC == true && model.TRANGTHAI == 2)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region GiaDichVu Tinh
        public static bool CheckApDung(this DM_GIADICHVU_TINH model)
        {
            if (model.KETQUA == 2 && model.HIEULUC != true)
            {
                return true;
            }
            return false;
        }
        public static bool CheckHuyApDung(this DM_GIADICHVU_TINH model)
        {
            if (model.HIEULUC == true && model.TRANGTHAI == 2)
            {
                return true;
            }
            return false;
        }
        public static bool duocTuChoi(this DM_GIADICHVU_TINH model)
        {
            return true; // tạm thời để mặc định là được từ chối.
        }
        #endregion

        /// <summary>
        /// check 1 dịch vụ có quyền được thực hiện 1 tác vụ nào đó không với các trường trong db
        /// </summary>
        /// <param name="row">dịch vụ được chọn</param>
        /// <param name="tacvu">tác vụ: 1 duyệt, 2 huỷ duyệt, 3 khôi phục, 4 ánh xạ, 5 huỷ ánh xạ</param>
        /// <returns></returns>
        public static bool CheckQuyenButtunDM_DichVuBV(this DM_DICHVU_BV row, int tacvu)
        {
            // 1
            if (row.AX_TD != true && row.AX_CD != true && ((row.KETQUA_TD == 0 && row.KETQUA == 0) || (row.KETQUA_TD == null && row.KETQUA == null)))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return true;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return true;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 2
            if (row.AX_TD != true && row.AX_CD == true && row.KETQUA_TD == 1 && row.KETQUA == 1)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return true;

                    case 2: // tu choi
                        return true;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return true;

                    case 5: // huy anh xa
                        return true;
                }
            }
            // 3, 4
            if (row.AX_TD != true && row.AX_CD == true && row.KETQUA_TD == 1 && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return true;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 5, 6
            if (row.AX_TD != true && row.AX_CD == true && row.KETQUA_TD == row.KETQUA && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return true;

                    case 5: // huy anh xa
                        return true;
                }
            }
            // 7, 8
            if (row.AX_TD == true && row.AX_CD != true && row.KETQUA_TD == row.KETQUA && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 9
            if (row.AX_TD == true && row.AX_CD != true && row.KETQUA_TD == 1 && row.KETQUA == 1)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return true;

                    case 2: // tu choi
                        return true;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 10, 11
            if (row.AX_TD == true && row.AX_CD != true && row.KETQUA_TD == 1 && (row.KETQUA == 2 || row.KETQUA == 3))
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return true;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 12
            if (row.AX_TD != true && row.AX_CD != true && row.KETQUA_TD == 3 && row.KETQUA == 3)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return false;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            // 13
            if (row.AX_TD != true && row.AX_CD != true && (row.KETQUA_TD == 0 || row.KETQUA_TD == null) && row.KETQUA == 3)
            {
                switch (tacvu)
                {
                    case 1: // duyet
                        return false;

                    case 2: // tu choi
                        return false;

                    case 3: // khoi phuc, thu hoi ...
                        return true;

                    case 4: // anh xa
                        return false;

                    case 5: // huy anh xa
                        return false;
                }
            }
            return false;
        }

        public static void UpdateFakeModelToModel(this CHOTDL_QUY model, CHOTDL_QUY fakeModel)
        {
            model.CSKCB_ID = fakeModel.CSKCB_ID;
            model.MA_CSKCB = fakeModel.MA_CSKCB;
            model.KHOA_ID = fakeModel.KHOA_ID;
            model.MAKHOA = fakeModel.MAKHOA;
            model.LOAI = fakeModel.LOAI;
            model.QUY = fakeModel.QUY;
            model.NAM = fakeModel.NAM;
            model.SOLUOT = fakeModel.SOLUOT;
            model.TRAN = fakeModel.TRAN;
            model.TCP = fakeModel.TCP;
            model.TPBO = fakeModel.TPBO;
            model.VUOTTRAN = fakeModel.VUOTTRAN;
            model.DUTRAN = fakeModel.DUTRAN;
            model.CNTT = fakeModel.CNTT;
            model.DUQUYTRUOC = fakeModel.DUQUYTRUOC;
        }
    }
}