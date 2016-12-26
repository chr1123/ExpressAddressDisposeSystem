using EADS.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using EADS.BLL;
using EADS.Model;
using System.Threading;

namespace EADS.Server.Handler
{
    /// <summary>
    /// 负责文件处理相关业务
    /// </summary>
    public interface NumChangedListener
    {
        void NumberChanged();  
    } 
}