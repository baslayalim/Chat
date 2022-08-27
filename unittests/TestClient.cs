using client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace unittests
{
    public class Client
    {

        myClient client;

        public Client()
        {
            if (client == null) client = new myClient();
        }

        [Fact]
        public void CreateClient()
        {

            if (client == null) client = new myClient();
        }


        [Fact]
        public void TestConnect()
        {



            Assert.True(client.isConnected());

            client.SendMessage("Test");


            Assert.True(client.CloseConnection());



        }



    }
}
