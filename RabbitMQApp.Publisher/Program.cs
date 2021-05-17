using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQApp.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            // İlk olarak RabbitMQ ya bağlanmamız gerekiyor.
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://myntuobw:GYL7dAkJfH19crSe7jAVI9fjKBwbfrWK@clam.rmq.cloudamqp.com/myntuobw"); // Normalde bu bilgiyi appsetting.json da tutmalıyız

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel(); // Bu kanal üzerinden RabbitMQ ile haberleşebiliriz

            // Mesajların boşa gitmemesi için bir kuyruk oluşturmak gerekiyor.
            /* Kuyruğa bir isim verdik. */
            /*
             * durable false olursa, oluşan bu kuyruklar memory de tutulur. 
             * rabbitMQ ya reset atılırsa momory de tutulduğu için tüm kuyruk gider.
             * 
             * durable true olursa, kuyruklar fiziksel olarak kaydedilir.
             * rabbitMQ ya reset atılsa bile kuyruklara birşey olmaz. (önerilen)
             */
            /*
             * exclusive true olursa, bu kuyruğa sadece oluşturduğumuz bu 
             * kanal üzerinden erişim sağlayabiliriz. 
             * Ancak bu kuyrağa subscriber tarafındaki farklı bir kanal üzerinden
             * bağlanacağız. O yüzden false olmalıdır.
             */
            /*
             * autodelete, kuyrağa bağlı olan son subscriber eğer bağlantısını
             * kopartırsa kuyruğu siler. Ancak herhangi birhatadan dolayı 
             * subscriber düşerse kuyruk da silinir bu yüzden kuyruğun kalıcı
             * olması önemlidir.
             */
            channel.QueueDeclare("hello-queue", true, false, false);

            string message = "HELLO rabbitmq. i'm Ahmet from the WORLD ";
            // Mesajlar byte dizisi olarak gider.

            var messageBody = Encoding.UTF8.GetBytes(message);

            /*
             * Exchange kullanmadığımız işleme default exchange denir. 
             */
            channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

            Console.WriteLine("Your message has been sent.");
            Console.ReadLine();
        }
    }
}
