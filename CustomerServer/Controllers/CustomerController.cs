using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using CustomerServer.Models;
using CustomerServer.Data;

namespace CustomerServer.Controllers
{
    public class CustomerController
    {
        public static void HandleClientRequest(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            TransferObj to = GetObject(stream);
            switch (to.Action)
            {
                case "LOGIN":
                {
                    HandleLogin(to, stream);
                    break;
                }
                case "ADD_CUSTOMER":
                {
                    HandleAddCustomer(to,stream);
                    break;
                }
                case "REMOVE_CUSTOMER":
                {
                    HandleRemoveCustomer(to,stream);
                    break;
                }
            }
            client.Close();
        }

        public static TransferObj GetObject(NetworkStream stream)
        {
            byte[] dataFromClient = new byte[1024];
            int bytesRead = stream.Read(dataFromClient, 0, dataFromClient.Length);
            string s = Encoding.ASCII.GetString(dataFromClient, 0, bytesRead);
            TransferObj transferObj = JsonSerializer.Deserialize<TransferObj>(s);
            return transferObj;
        }

        public static async void HandleLogin(TransferObj to, NetworkStream stream)
        {
            Customer customer = JsonSerializer.Deserialize<Customer>(to.Arg);
            IWebService webService = new WebService();
            Customer resultCustomer = await webService.ValidateUser(customer.Mail, customer.Password);

            TransferObj reply = new TransferObj();
            if (resultCustomer == null)
            {
                reply.Action = "USER NOT FOUND";
            }
            else
            {
                reply.Action = "OK";
                reply.Arg = JsonSerializer.Serialize(resultCustomer);
            }
            byte[] dataToClient = Encoding.ASCII.GetBytes($"{reply}");
            stream.Write(dataToClient, 0, dataToClient.Length);
        }

        public static async void HandleAddCustomer(TransferObj to, NetworkStream stream)
        {
            Customer customer = JsonSerializer.Deserialize<Customer>(to.Arg);
            IWebService webService = new WebService();
            await webService.AddCustomerAsync(customer);
            
            TransferObj reply = new TransferObj();
            reply.Action = "OK";
            byte[] dataToClient = Encoding.ASCII.GetBytes($"{reply}");
            stream.Write(dataToClient, 0, dataToClient.Length);
        }

        public static async void HandleRemoveCustomer(TransferObj to, NetworkStream stream)
        {
            Customer customer = JsonSerializer.Deserialize<Customer>(to.Arg);
            IWebService webService = new WebService();
            await webService.RemoveCustomerAsync(customer.Id);
            
            TransferObj reply = new TransferObj();
            reply.Action = "OK";
            byte[] dataToClient = Encoding.ASCII.GetBytes($"{reply}");
            stream.Write(dataToClient, 0, dataToClient.Length);
        }
    }
}