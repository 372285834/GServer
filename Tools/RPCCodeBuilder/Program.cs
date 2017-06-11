using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RPCCodeBuilder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var a = ServerFrame.UniHash.DefaultHash("asdfasdfasef");
            //var b = ServerFrame.UniHash.DefaultHash("22123asdf");
            //var c = ServerFrame.UniHash.DefaultHash("asdfe231");
            //var d = ServerFrame.UniHash.DefaultHash("asdf212fas");



            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RPCCodeBuilderFrm());
        }
    }






}
