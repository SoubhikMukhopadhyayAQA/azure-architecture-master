using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using System.IO;
using TransferVideos;

string blob_connection_string = "";
string servicebus_connection_string = "";

string container_name = "unprocessed";
string strPath = "C:\\tmp3";
string queue_name = "filequeue";

BlobServiceClient _client = new BlobServiceClient(blob_connection_string);
BlobContainerClient _container_client = _client.GetBlobContainerClient(container_name);

ServiceBusClient _busclient = new ServiceBusClient(servicebus_connection_string);
ServiceBusSender _sender = _busclient.CreateSender(queue_name);


string[] filenames = Directory.GetFiles(strPath);

foreach (string filename in filenames)
{

    BlobClient _blob_client = _container_client.GetBlobClient(Path.GetFileName(filename));
    await _blob_client.UploadAsync(filename);

    Item item = new Item();
    item.ItemName = Path.GetFileName(filename);
    item.ItemLocation = "https://appstore443443.blob.core.windows.net/unprocessed/" + Path.GetFileName(filename);

    ServiceBusMessage _message = new ServiceBusMessage(item.ToString());
    _message.ContentType = "application/json";
    await _sender.SendMessageAsync(_message);

    Console.WriteLine($"Uploaded blob {filename}");
}


