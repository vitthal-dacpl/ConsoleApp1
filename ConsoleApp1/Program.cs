using S7.Net;
using System;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        public static Plc cpu = null;
        public static uint count = 100;
        public static ushort count1 = 1000;
        public static float count2 = 1.11f;
        public static bool b = false;
        public static PlcDataManager write;
        static void Main(string[] args)
        {
            PlcConnectionManager manager = new PlcConnectionManager();
            manager.Connect(CpuType.S71200, "192.168.1.61", 0, 1);
            cpu = manager.plc;
            write = new PlcDataManager();
            callback(null);
            try
            {
                // 2️⃣ Create monitoring objects without passing PlcConnectionManager
                PlcMonitor monitor1 = new PlcMonitor("DB1.DBD262.0", VarType.Byte, 1000);
                monitor1.DataChanged += Monitor1_DataChanged; ;
                monitor1.PlcErrorOccurred += Monitor1_PlcErrorOccurred;
                monitor1.StartMonitoring();


                PlcMonitor monitor2 = new PlcMonitor("DB1.DBD266.0", VarType.Real, 1000);
                monitor2.DataChanged += Monitor2_DataChanged;
                monitor2.PlcErrorOccurred += Monitor2_PlcErrorOccurred;
                monitor2.StartMonitoring();

                PlcMonitor monitor3 = new PlcMonitor("DB1.DBX270.0", VarType.Byte, 1000);
                monitor3.DataChanged += Monitor3_DataChanged;
                monitor3.PlcErrorOccurred += Monitor3_PlcErrorOccurred;
                monitor3.StartMonitoring();

                PlcMonitor monitor4 = new PlcMonitor("DB1.DBB6.0", VarType.String, 1000);
                monitor4.DataChanged += Monitor4_DataChanged;
                monitor4.StartMonitoring();



                PlcMonitor monitor5 = new PlcMonitor("DB1.DBW0.0", VarType.Word, 1000);
                monitor5.DataChanged += Monitor5_DataChanged;
                monitor5.StartMonitoring();

                PlcMonitor monitor6 = new PlcMonitor("DB1.DBB272.0", VarType.String, 500);
                monitor6.DataChanged += Monitor6_DataChanged;
                monitor6.StartMonitoring();

                Timer timer = new Timer(callback, null, 0, 400);


                Console.WriteLine("Press any key to stop...");
                //monitor1.DataChanged -= Monitor1_DataChanged;
                Console.ReadLine();

                // 3️⃣ Stop monitoring and disconnect PLC
                monitor1.StopMonitoring();
                monitor2.StopMonitoring();
                monitor3.StopMonitoring();
                monitor4.StopMonitoring();
                monitor5.StopMonitoring();
                manager?.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.ReadLine();

        }

        private static void Monitor2_PlcErrorOccurred(object sender, PlcErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Monitor3_PlcErrorOccurred(object sender, PlcErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Monitor1_PlcErrorOccurred(object sender, PlcErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void Monitor6_DataChanged(object sender, DataChangedEventArgs e)
        {
            Console.WriteLine($"Data at {e.DbAddress} changed: {e.NewValue} Oldvalue:  {e.OldValue} Datetime: {e.Timestamp}");

        }

        private static void Monitor5_DataChanged(object sender, DataChangedEventArgs e)
        {
            Console.WriteLine($"Data at {e.DbAddress} changed: {e.NewValue} Oldvalue:  {e.OldValue} Datetime: {DateTime.Now.ToString("HH:mm:ss.fff")}");

        }

        private static void Monitor4_DataChanged(object sender, DataChangedEventArgs e)
        {
            Console.WriteLine($"Data at {e.DbAddress} changed: {e.NewValue} Oldvalue:  {e.OldValue} Datetime: {DateTime.Now.ToString("HH:mm:ss.fff")}");

        }

        private static void Monitor3_DataChanged(object sender, DataChangedEventArgs e)
        {
            bool a = (bool)e.NewValue;
            if (a == true)
            {
                b = false;
            }
            else
            {
                b = true;
            }
            Console.WriteLine($"Data at {e.DbAddress} changed: {e.NewValue} Oldvalue:  {e.OldValue} Datetime: {DateTime.Now.ToString("HH:mm:ss.fff")}");
        }

        private static void Monitor2_DataChanged(object sender, DataChangedEventArgs e)
        {
            Console.WriteLine($"Data at {e.DbAddress} changed: {e.NewValue} Oldvalue:  {e.OldValue} Datetime: {DateTime.Now.ToString("HH:mm:ss.fff")}");
        }

        private static void Monitor1_DataChanged(object sender, DataChangedEventArgs e)
        {
            Console.WriteLine($"Data at {e.DbAddress} changed: {e.NewValue} Oldvalue:  {e.OldValue} Datetime: {DateTime.Now.ToString("HH:mm:ss.fff")}");
        }

        private static async void callback(object state)
        {

            ushort val1 = count1;
            uint val = count;
            float val2 = count2;
            write.AddWriteValue("DB1.DBX270.0", VarType.Bit, b);
            write.AddWriteValue("DB1.DBD0", VarType.DWord, val);
            write.AddWriteValue("DB1.DBD266.0", VarType.Real, val2);
            write.AddWriteValue("DB1.DBD262", VarType.DWord, val);
            write.AddWriteValue("DB1.DBB272", VarType.String, "Dhruva Automation baner pune india" + count.ToString());
            WriteResponse responce = await write.WriteAsync();
            //foreach (var result in responce.Results)
            //{
            //    Console.WriteLine($"Address: {result.DbAddress}, Status: {result.Status}, ErrorCode: {result.ErrorCode},  Message: {result.Error}");
            //}
            count++;
            count1++;
            count2++;
            write.AddReadValue("DB1.DBX270.0", VarType.Bit);
            write.AddReadValue("DB1.DBD0", VarType.DWord);
            write.AddReadValue("DB1.DBB272", VarType.String);
            write.AddReadValue("DB1.DBD266", VarType.Real);
            write.AddReadValue("DB1.DBW0", VarType.Word);
            ReadResponse readResponse = await write.ReadAsync();
            foreach (var result in readResponse.Results)
            {
                Console.WriteLine($"Read {result.DbAddress}: {result.Value}");
            }
            object rawValue = Convert.ToUInt32(cpu.Read("DB1.DBD262.0"));
            object rawValue1 = cpu.Read("DB1.DBW0.0");
            object ravalue3 = ((ushort)rawValue1);
            object rawValue4 = Convert.ToBoolean(cpu.Read("DB1.DBD270.0"));
            Console.WriteLine("----------------------------Timer-----------------------------------------");
            Console.WriteLine($" DBWord : {count1} ,  Word : {count},   datattime  :{DateTime.Now.ToString("HH:mm:ss:ff")} ");
            Console.WriteLine("---------------------------------------------------------------------");
        }



    }
}





