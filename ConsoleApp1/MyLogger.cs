using log4net;

namespace ConsoleApp1
{
    public static class MyLog
    {
        public static ILog Log { get; }

        static MyLog()
        {
            Log = LogManager.GetLogger(typeof(MyLog));
        }
    }    
}