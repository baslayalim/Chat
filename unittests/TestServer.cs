using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace unittests
{
    public class TestServer
    {

        private myServer server;

        [Fact]
        public void CreateServer()
        {

            server = new myServer();

        }


        [Fact]
        public void TestConnect()
        {

            if (server == null) server = new myServer();

            Assert.True(server.IsActive());
        }
    }
}
