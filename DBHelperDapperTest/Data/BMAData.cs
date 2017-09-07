using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDapper.Data
{
    public class BMAData
    {
        private static object _locker = new object();//锁对象
        private static BMAData instance;
        public static BMAData Instance
        {
            get
            {
                lock (_locker)
                {
                    if (instance == null)
                    {
                        instance = new BMAData();
                    }
                }
                return instance;
            }
        }
        private sys_lottery _sys_lottery;

        public sys_lottery Sys_lottery
        {
            get
            {
                lock (_locker)
                {
                    if (_sys_lottery == null)
                        _sys_lottery = new sys_lottery();//依赖注入
                    return _sys_lottery;
                }
            }
        }

    }
}
