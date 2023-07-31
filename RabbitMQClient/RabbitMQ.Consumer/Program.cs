// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

Console.WriteLine("Hello Consumer!");
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

//model.BasicQos(0, 1, false);


var consumer= new EventingBasicConsumer(model);
consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
{
    var body= e.Body.ToArray();
    var bodyText= Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message: {bodyText}");
    model.BasicAck(e.DeliveryTag, false);
    // Processing delay
    Thread.Sleep(TimeSpan.FromSeconds(1));
};

var tag= model.BasicConsume(queueName,false,consumer);

Console.ReadKey();
//model.BasicCancel(tag);

Console.WriteLine("Press any Key Exit");
Console.ReadKey();

