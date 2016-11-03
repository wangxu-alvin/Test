using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanucSampling
{
    class ReturnCodeDesc
    {
        private static Dictionary<short, string> retCodeDesc;

        private static void init()
        {
            retCodeDesc = new Dictionary<short, string>();
            retCodeDesc.Add((short)Focas1.focas_ret.EW_PROTOCOL, "Protocol error. Data from Ethernet Board is incorrect.Contact with the service section or the section in charge.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_SOCKET, "Socket error. Investigate CNC power supply, Ethernet cable and I/F board.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_NODLL, "DLL file error. There is no DLL file for each CNC series corresponding to specified node.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_BUS, "Bus error (HSSB version only). A bus error of CNC system occurred. Contact with the service section or the section in charge.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_SYSTEM2, "System error (2) (HSSB version only). A system error of CNC system occurred.Contact with the service section or the section in charge.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_HSSB, "Communication error of HSSB (HSSB version only). Investigate the serial line or I/F board of HSSB.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_HANDLE, "Handle number error. Get the library handle number.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_VERSION, "Version mismatch between the CNC/PMC and library. The CNC/PMC version does not match that of the library.Replace the library or the CNC/PMC control software.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_UNEXP, "Abnormal library state. An unanticipated error occurred.Contact with the section in charge.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_PARITY, "Shared RAM parity error (HSSB version only). A hardware error occurred.Contact with the service section. ");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_MMCSYS, "FANUC drivers installation error (HSSB version only) . The drivers required for execution are not installed.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_RESET, "Reset or stop request. The RESET or STOP button was pressed.Call the termination function. ");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_BUSY, "Busy. Wait until the completion of CNC processing, or retry.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_OK, "Normal termination .");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_FUNC, "Error(function is not executed, or not available). Specific function which must be executed beforehand has not been executed.Otherwise that function is not available.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_LENGTH, "Error(data block length error, error of number of data). Check and correct the data block length or number of data.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_NUMBER, "Error(data number error).Check and correct the data number.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_ATTRIB, "Error(data attribute error). Check and correct the data attribute. ");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_DATA, "Error(data error). Check and correct the data.For the following operations, this code indicates that the specified program cannot be found.Delete specified program. Search specified program.Start uploading NC program.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_NOOPT, "Error(no option) . There is no corresponding CNC option.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_PROT, "Error(write protection). Write operation is prohibited.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_OVRFLOW, "Error(memory overflow). CNC tape memory is overflowed. ");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_PARAM, "Error(CNC parameter error). CNC parameter is set incorrectly.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_BUFFER, "Error(buffer empty/full). The buffer is empty or full. Wait until completion of CNC processing, or retry. ");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_PATH, "Error(path number error). A path number is incorrect.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_MODE, "Error(CNC mode error). The CNC mode is incorrect. Correct the CNC mode. ");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_REJECT, "Error(CNC execution rejection).The execution at the CNC is rejected.Check the condition of execution.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_DTSRVR, "Error(Data server error). Some errors occur at the data server.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_ALARM, "Error(alarm).The function cannot be executed due to an alarm in CNC.Remove the cause of alarm.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_STOP, "Error(stop). CNC status is stop or emergency.");
            retCodeDesc.Add((short)Focas1.focas_ret.EW_PASSWD, "Error(State of data protection). Data is protected by the CNC data protection function. ");
        }

        public static string getDesc(short code)
        {
            if (retCodeDesc == null)
            {
                init();
            }
            return retCodeDesc[code];
        }
    }
}
