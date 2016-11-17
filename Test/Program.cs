using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ushort Flibhndl = 0;

            short ret;                 // 返回值

            // 获取库句柄 ( Ethernet )
            ret = Focas1.cnc_allclibhndl3("192.168.1.193", 8193, 10, out Flibhndl);
            if (ret != Focas1.EW_OK)
            {
                Console.WriteLine("发生异常，请检查！");
                return;
            }

            #region cnc_rdexecprog

            ushort length = 1000;
            short blknum = 0;
            object data = 0;

            ret = Focas1.cnc_rdexecprog(Flibhndl, ref length, out blknum, data);

            Console.Read();

            #endregion

        }
    }
}











    ////    }
    ////}

    //class Observable
    //{
    //    public event EventHandler SomethingHappened;

    //    public void DoSomething()
    //    {
    //        EventHandler handler = SomethingHappened;
    //        if (handler != null)
    //        {
    //            handler(this, EventArgs.Empty);
    //        }
    //    }
    //}

    //class Observer
    //{
    //    string _name;
    //    public Observer(string name)
    //    {
    //        _name = name;
    //    }
    //    public void HandleEvent(object sender, EventArgs args)
    //    {
    //        Console.WriteLine(_name + "Something happened to " + sender);
    //    }
    //}

    //class Test
    //{
    //    static void Main(string[] args)
    //    {
    //        Observable observable = new Observable();
    //        Observer observer = new Observer("小张");
    //        Observer observer1 = new Observer("小李");
    //        observable.SomethingHappened += observer.HandleEvent;
    //        observable.SomethingHappened += observer1.HandleEvent;

    //        observable.DoSomething();
    //        Console.Read();
    //    }
    //}

