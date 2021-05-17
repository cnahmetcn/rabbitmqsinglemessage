using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQApp.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://myntuobw:GYL7dAkJfH19crSe7jAVI9fjKBwbfrWK@clam.rmq.cloudamqp.com/myntuobw"); // Normalde bu bilgiyi appsetting.json da tutmalıyız

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel(); // Bu kanal üzerinden RabbitMQ ile haberleşebiliriz


            channel.QueueDeclare("hello-queue", true, false, false);
            // Publisher ın bu kuyruğu oluşturduğuna eminsek bu kod silinebilir.

            var consumer = new EventingBasicConsumer(channel);

            /*
             * autoAck, true olursa RabbitMQ subscriber a bir mesaj gönderdiğinde
             * bu mesaj doğru da olsa yanlışda olsa kuyruktan silinir.
             * autoAck, false olursa RabbitMQ ya bu mesajı kuyruktan silmemesini
             * eğer gelen mesaj doğru şekilde işlenirse kuyruktan silinmesi için
             * haber verilecek.
             */
            channel.BasicConsume("hello-queue", true, consumer);

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Console.WriteLine("Incoming Message: " + message);
            };

            Console.ReadLine();
        }


    }
}
