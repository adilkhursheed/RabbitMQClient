// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

Console.WriteLine("Hello, World!");

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
factory.ClientProvidedName = "RabbitMQ Client Test";

//using (var con = factory.CreateConnection())
//{
//    using (var model = con.CreateModel())
//    {


var con = factory.CreateConnection();
var model = con.CreateModel();
var exchangeName= "RabbitMQClientTest";
        var routingKey = "test-routing-key";
        var queueName= "TestQueue";

        model.ExchangeDeclare(exchangeName, ExchangeType.Direct);
        model.QueueDeclare(queueName);
        model.QueueBind(queueName, exchangeName, routingKey);

        var messageBody= Encoding.UTF8.GetBytes("Hello Queue 3!");
        model.BasicPublish(exchangeName, routingKey, body: messageBody);



//    }
//}
Console.WriteLine("Press any Key Exit");
Console.ReadKey();

