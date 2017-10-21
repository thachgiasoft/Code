﻿using EIS.Core.Domain;
using FX.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIS.Core.IService
{
    public interface INHOMCHUCNANGService : IBaseService<NHOMCHUCNANG, int>
    {

        bool UserPhanQuyen(int roleid);

    }
}