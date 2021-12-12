using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.ComponentModel;

namespace WindowsFormsApplication2.DelayAction
{
    public class DelayActionModel
    {
        private Timer _timerDbc;
        private Timer _timerTrt;

        /// <summary>
        /// 延迟 timeMs 后执行。 在此期间如果再次调用，则重新计时
        /// </summary>
        /// <param name="invoker">同步对象，一般为 Control 控件。如不需同步可传 null</param>
        public void Debounce(int timeMs, ISynchronizeInvoke invoker, Action action)
        {
            lock (this)
            {
                if (_timerDbc == null)
                {
                    _timerDbc = new Timer(timeMs)
                    {
                        AutoReset = false
                    };
                    _timerDbc.Elapsed += (o, e) =>
                    {
                        _timerDbc.Stop();
                        _timerDbc.Close();
                        _timerDbc = null;
                        InvokeAction(action, invoker);
                    };
                }
                _timerDbc.Stop();
                _timerDbc.Start();
            }
        }


        /// <summary>
        /// 即刻执行，执行之后，在 timeMs 内再次调用无效
        /// </summary>
        /// <param name="timeMs">不应期，这段时间内调用无效</param>
        /// <param name="invoker">同步对象，一般为控件。如不需同步可传 null</param>
        public void Throttle(int timeMs, ISynchronizeInvoke invoker, Action action)
        {
            System.Threading.Monitor.Enter(this);
            bool needExit = true;
            try
            {
                if (_timerTrt == null)
                {
                    _timerTrt = new Timer(timeMs)
                    {
                        AutoReset = false
                    };
                    _timerTrt.Elapsed += (o, e) =>
                    {
                        _timerTrt.Stop();
                        _timerTrt.Close();
                        _timerTrt = null;
                    };
                    _timerTrt.Start();
                    System.Threading.Monitor.Exit(this);
                    needExit = false;
                    InvokeAction(action, invoker);//这个过程不能锁
                }
            }
            finally
            {
                if (needExit)
                    System.Threading.Monitor.Exit(this);
            }
        }

        /// <summary>
        /// 延迟 timeMs 后执行。 
        /// </summary>
        public void Delay(int timeMs, ISynchronizeInvoke invoker, Action action)
        {
            Debounce(timeMs, invoker, action);
        }

        private static void InvokeAction(Action action, ISynchronizeInvoke invoker)
        {
            if (invoker == null)
            {
                action();
            }
            else
            {
                if (invoker.InvokeRequired)
                {
                    invoker.Invoke(action, null);
                }
                else
                {
                    action();
                }
            }
        }
    }
}
