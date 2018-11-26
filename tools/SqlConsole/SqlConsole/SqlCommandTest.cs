using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SqlConsole
{
    public class SqlCommandTest
    {
        private string _commandText;

        [DllImport("Test.dll")]
        private static extern void TestEnter(string test);


        [DllImport("Test.dll")]
        private static extern void TestLeave();

        public int Read()
        {
            TestEnter(_commandText);
            try
            {

            }
            finally
            {
                TestLeave();
            }
            return 1;
        }
    }
}
