using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

    struct SingUp
    {
        public string Name { set; get; }
        public string Password { set; get; }
        public string Login { set; get; }
    }

    struct SingIn
    {
        public string Password { set; get; }
        public string Login { set; get; }
    }

public class PlayerConaction : MonoBehaviour
{
    static int port = 8005;
    static string address = "192.168.1.65";        
    static TcpClient client;
    static NetworkStream stream;        
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private UIManager uiManager; 
    private static Thread receiveThread;

    void Start()
    {
        Connect();
        string json = JsonUtility.ToJson(new SingUp{
            Name = "Admin",
            Password = "Admin",
            Login = "Admin"
        });
        SendMessage($"-r{json}");

        string jsonTwo = JsonUtility.ToJson(new SingIn{            
            Password = "Admin",
            Login = "Admin"
        });
        SendMessage($"-l{jsonTwo}");
    }

    void Update()
    {
        
    }

    public void Connect()
    {        
        client = new TcpClient();
        Debug.Log("Connect");
        try
        {            
            client.Connect(address, port); 
            stream = client.GetStream();

            string message = name;
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);                        
            receiveThread = new Thread(new ThreadStart(ReceiveMessage));
            receiveThread.Start(); 
        }
        catch
        {
            Debug.Log("Error in ClientConnect!!!");
        }
    }

    private void ReceiveMessage()
    {
        while (client.Connected)
        {
            try
            {
                byte[] data = new byte[64]; 
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);

                string message = builder.ToString();                
                if (message.Length != 0) {
                    Debug.Log(message + "Mes");
                    gameManager.GetCommand(message);                    
                }
            }
            catch(Exception e)
            {
                Debug.Log("Error ReciveMessage " + e);
                Disconnect();
            }
        }
    }  

    static void Disconnect()
    {
        if (stream != null)
            stream.Close();
        if (client != null)
            client.Close();
        receiveThread.Abort();
    }

    public void SendMessage(string message)
    {            
        byte[] data = Encoding.Unicode.GetBytes(message);
        stream.Write(data, 0, data.Length);        
    }    
}
