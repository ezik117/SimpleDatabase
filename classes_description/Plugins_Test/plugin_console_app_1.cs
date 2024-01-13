//$TYPE APPLICATION
//$REFERENCES System.dll

using System;

namespace plugins
{
    class PluginSample
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Press a key to exit");
            Console.ReadKey(true);
        }
    }
}
