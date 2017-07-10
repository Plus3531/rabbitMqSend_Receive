using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

class Receive
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName= "rabbitAdmin", Password = "some_secret_password", VirtualHost="testHost" };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
            var result = channel.QueueDeclare(exclusive:true);

            //channel.QueueDeclare(queue: "testQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            //channel.QueueBind("testQueue", "amq.topic", "#");
            channel.QueueBind(exchange:"dbMsg", queue:result.QueueName, routingKey:"#");
    
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            channel.BasicConsume(queue: result.QueueName, autoAck: true, consumer: consumer);            
            //channel.BasicConsume(queue: "testQueue", autoAck: true, consumer: consumer);            
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}


//  public static void Main()
//     {
//         var factory = new ConnectionFactory() { HostName = "localhost" };
//         using(var connection = factory.CreateConnection())
//         using(var channel = connection.CreateModel())
//         {
//             //channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
//             channel.QueueDeclare(queue: "testQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

//             var consumer = new EventingBasicConsumer(channel);
//             consumer.Received += (model, ea) =>
//             {
//                 var body = ea.Body;
//                 var message = Encoding.UTF8.GetString(body);
//                 Console.WriteLine(" [x] Received {0}", message);
//             };
//             channel.BasicConsume(queue: "testQueue", autoAck: true, consumer: consumer);
//             //channel.BasicConsume(queue: "hello", autoAck: true, consumer: consumer);

//             Console.WriteLine(" Press [enter] to exit.");
//             Console.ReadLine();
//         }
//     }