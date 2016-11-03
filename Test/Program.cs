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
            //ret = Focas1.cnc_allclibhndl3("10.30.3.114", 8193, 10, out Flibhndl);
            //if (ret != Focas1.EW_OK)
            //{
            //    Console.WriteLine("发生异常，请检查！");
            //    return;
            //}

            //#region cnc_rdprogdir2
            //Console.WriteLine("要读的第一个程序的程序号是：");
            //string _top_prog = Console.ReadLine();
            //Console.WriteLine("一共要读几个程序：");
            //string _num_prog = Console.ReadLine();
            //short type = 2;
            //short top_prog = short.Parse(_top_prog);
            //short num_prog = short.Parse(_num_prog);

            //Focas1.PRGDIR2 buf = new Focas1.PRGDIR2();
            //ret = Focas1.cnc_rdprogdir2(Flibhndl, type, ref top_prog, ref num_prog, buf);
            //Console.WriteLine("程序comment：");
            //Console.WriteLine(buf.dir1.comment);
            //Console.Read();
            //#endregion



            // 获取库句柄 ( Ethernet )
            ret = Focas1.cnc_allclibhndl3("10.30.3.114", 8193, 10, out Flibhndl);
            if (ret != Focas1.EW_OK)
            {
                Console.WriteLine("发生异常，请检查！");
                return;
            }
            #region cnc_rdparam

            Focas1.IODBPSD_1 obd = new Focas1.IODBPSD_1();
            ret = Focas1.cnc_rdparam(Flibhndl, 6711, 0, 8, obd);
            if (ret == Focas1.EW_OK)
            {
                Console.WriteLine(obd.cdata);
                Console.WriteLine(obd.datano);
                Console.WriteLine(obd.idata);
                Console.WriteLine(obd.ldata); // 加工零件应该应该是这个数
                Console.WriteLine(obd.type);
                return;
            }
            Console.Read();
        }
        #endregion
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
}
