using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonService.Utility
{
    public class RabbitMQConnectionHelper
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;

        public RabbitMQConnectionHelper()
        {
            _connectionFactory = new ConnectionFactory() { 
                HostName = "localhost",
                UserName = "root",
                Password = "password",
            };

            _connection = (IConnection)_connectionFactory.CreateConnectionAsync();
        }

        public IConnection GetConnection() => _connection;
    }
}
