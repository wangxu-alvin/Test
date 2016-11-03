using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class MainEntry
    {
        static void abc(string[] args)
        {
            // 自带样例
            //Program.demo();

            // 连接机床
            if (!connectCNC("10.30.3.114", 8193, 10))
            {
                return;
            }

            // 读取机械坐标 失败
            //if (!mechanicalCoordinate())
            //{
            //    return;
            //}

            // 读取绝对坐标 失败
            //if (!absoluteCoordinate2())
            //{
            //    return;
            //}

            //// 读取绝对坐标
            //if (!stateInfo())
            //{
            //    return;
            //}

            //// 读取主轴转速
            //if (!acts())
            //{
            //    return;
            //}

            //// 读取主轴负载
            //if (!load())
            //{
            //    return;
            //}

            //// 读取程序名
            //if (!programName())
            //{
            //    return;
            //}

            //// 读取程序计数器
            //if (!blockCount())
            //{
            //    return;
            //}
        }

        static bool connectCNC(string ip, ushort port, int timeout)
        {
            // 获取库句柄 ( Ethernet ) 并进行连接
            ushort Flibhndl = 0;
            short ret = Focas1.cnc_allclibhndl3(ip, port, timeout, out Flibhndl);
            if (ret != Focas1.EW_OK)
            {
                Console.WriteLine("连接CNC异常！");
                string message;
                switch (ret)
                {
                    case -8:
                        message = "Get handle failed.";
                        break;
                    case -16:
                        message = "Communication socket error. The CNC power supply board, Ethernet, Ethernet cable, please check.";
                        break;
                    case -15:
                        message = "DLL type does not exist.";
                        break;
                    default:
                        message = "Unknown return value : " + ret;
                        break;
                }
                Console.WriteLine(message);
                Console.Read();
                return false;
            }
            Console.WriteLine("程序已经与CNC建立了连接,点击回车继续：");
            Console.Read();

            Console.WriteLine("准备读取机械坐标,点击回车继续：");
            Console.Read();

            Focas1.ODBAXIS odbaxis = new Focas1.ODBAXIS();
            String axis = "";
            for (short i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    axis = "X:";
                }
                else if (i == 1)
                {
                    axis = "Y:";
                }
                else
                {
                    axis = "Z:";
                }
                ret = Focas1.cnc_machine(Flibhndl, (short)(i + 1), 8, odbaxis);
                if (ret != Focas1.EW_OK)
                {
                    Console.WriteLine("读取数据失败，返回值：" + ret);
                    return false;
                }
                Console.WriteLine(axis + odbaxis.data[0] * Math.Pow(10, -3));
            }

            Console.WriteLine("准备读取绝对坐标,点击回车继续：");
            Console.Read();

            odbaxis = new Focas1.ODBAXIS();

            ret = Focas1.cnc_absolute2(Flibhndl, -1, 16, odbaxis);
            if (ret != Focas1.EW_OK)
            {
                Console.WriteLine("读取数据失败，返回值：" + ret);
            }
            else
            {
                 string absoluteX = odbaxis.data[0].ToString();
            string absoluteY = odbaxis.data[1].ToString();
            string absoluteZ = odbaxis.data[2].ToString();
            Console.WriteLine("X:" + absoluteX);
            Console.WriteLine("Y:" + absoluteY);
            Console.WriteLine("Z:" + absoluteZ);
            }

            Console.WriteLine("准备读取CNC状态信息,点击回车继续：");
            Console.Read();
            Focas1.ODBST buf = new Focas1.ODBST();
            ret = Focas1.cnc_statinfo(Flibhndl, buf);
            if (ret != Focas1.EW_OK)
            {
                Console.WriteLine("读取数据失败，返回值：" + ret);
            }
            Console.WriteLine("急停:" + buf.emergency);
            Console.WriteLine("运行:" + buf.run);
            Console.WriteLine("告警:" + buf.alarm);

            Console.WriteLine("准备读取主轴转速信息,点击回车继续：");
            Console.Read();

            Focas1.ODBACT odbact = new Focas1.ODBACT();
            ret = Focas1.cnc_acts(Flibhndl, odbact);
            if (ret != Focas1.EW_OK)
            {
                Console.WriteLine("读取数据失败，返回值：" + ret);
                return false;
            }
            Console.WriteLine("主轴转速：" + odbact.data.ToString());

            Console.WriteLine("准备读取主轴负载信息,点击回车继续：");
            Console.Read();

            Focas1.ODBSPN odbspn = new Focas1.ODBSPN();
            ret = Focas1.cnc_rdspload(Flibhndl, Focas1.ALL_SPINDLES, odbspn);
            if (ret != Focas1.EW_OK)
            {
                Console.WriteLine("读取数据失败，返回值：" + ret);
                return false;
            }
            Console.WriteLine(odbspn.data[0]); //TODO 文档没有对返回值的解释

            Console.WriteLine("准备读取正在执行的程序名,点击回车继续：");
            Console.Read();
 
            Focas1.ODBEXEPRG exeprg = new Focas1.ODBEXEPRG();
            ret = Focas1.cnc_exeprgname(Flibhndl, exeprg);
            if (ret != Focas1.EW_OK)
            {
                Console.WriteLine("读取数据失败，返回值：" + ret);
                return false;
            }
            Console.WriteLine(exeprg.name);
            Console.WriteLine(exeprg.o_num);

            Console.WriteLine("准备读取正在执行的程序块计数器,点击回车继续：");
            Console.Read();

            int prog_bc = 0;
            ret = Focas1.cnc_rdblkcount(Flibhndl, out prog_bc);
            if (ret != Focas1.EW_OK)
            {
                Console.WriteLine("读取数据失败，返回值：" + ret);
                return false;
            }
            Console.WriteLine(prog_bc);

            Console.WriteLine("准备读取正在执行的程序号,点击回车继续：");
            Console.Read();
            Focas1.ODBPRO pnum = new Focas1.ODBPRO();
            ret = Focas1.cnc_rdprgnum(Flibhndl, pnum);
            Console.WriteLine(String.Format("O{0:0000}", pnum.data));

            Console.WriteLine("准备读取正在被CNC运行NC程序的序号,点击回车继续：");
            Console.Read();
            Focas1.ODBSEQ snum = new Focas1.ODBSEQ();
            ret = Focas1.cnc_rdseqnum(Flibhndl, snum);
            Console.WriteLine(String.Format("N{0:00000}", snum.data));
            Console.Read();



           
            return true;
        }

        
    }

}
