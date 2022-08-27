using client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace unittests
{
    public class TestMain
    {

        private myServer _server;
        private myClient _client;




        public TestMain()
        {


            _server = new myServer();
            _client = new myClient();

        }


        [Fact]
        public void FloodTest()
        {


            for (int i = 0; i <= 50; i++)
            {

                Assert.True(_client.isConnected());

                _client.SendMessage(DateTime.Now.Ticks.ToString());


            }





        }



    }
}
