// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

Console.WriteLine("Publisher 1 Message/2 Seconds!");
Console.WriteLine("******************************");

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
model.QueueDeclare(queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);
model.QueueBind(queueName, exchangeName, routingKey);

for (int i = 0; i < 50; i++)
{
    var messageText= $"Test Message {i}!";
    var messageBody= Encoding.UTF8.GetBytes(messageText);
    model.BasicPublish(exchangeName, routingKey, body: messageBody);
    Console.WriteLine(messageText);
    Thread.Sleep(TimeSpan.FromSeconds(2));
}

Console.WriteLine("Press any Key Exit");
Console.ReadKey();

model.Close();
con.Close();