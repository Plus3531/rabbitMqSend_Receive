using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
	class Program
	{
		static void Main(string[] args)
		{
			var factory = new ConnectionFactory() { HostName = "localhost", UserName= "rabbitAdmin", Password = "some_secret_password", VirtualHost="testHost" };
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				// channel.QueueDeclare(queue: "hello",
				// 					 durable: false,
				// 					 exclusive: false,
				// 					 autoDelete: false,
				// 					 arguments: null);
				 channel.QueueDeclare(queue: "testQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind("testQueue", "amq.topic", "#");

				string message = "Hello World! pj";
				var body = Encoding.UTF8.GetBytes(message);

				channel.BasicPublish(exchange: "amq.topic",
									 routingKey: "#",
									 basicProperties: null,
									 body: body);
				Console.WriteLine(" [x] Sent {0}", message);
			}

			Console.WriteLine(" Press [enter] to exit.");
			Console.ReadLine();
		}
	}
}
