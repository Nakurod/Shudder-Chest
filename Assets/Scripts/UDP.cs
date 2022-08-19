using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDP
{
    public string IP { get; private set; }
    public int sourcePort { get; private set; }
    public int remotePort { get; private set; }

    IPEndPoint remoteEndPoint;

    Thread receiveThread;


    UdpClient client;

    public int port = 12345;

    // Information
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = "";

    public bool newdatahereboys = false;

    public void init(string IPAdress, int RemotePort, int SourcePort = -1) 
    {
        IP = IPAdress;
        sourcePort = SourcePort;
        remotePort = RemotePort;

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), remotePort);
        if (sourcePort <= -1)
        {
            client = new UdpClient();
            Debug.Log("Sending to " + IP + ": " + remotePort);
        }
        else
        {
            client = new UdpClient(sourcePort);
            Debug.Log("Sending to " + IP + ":" + remotePort + " from Source Port: " + sourcePort);
        }

        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    private void ReceiveData()
    {
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);

                string text = Encoding.UTF8.GetString(data);


                Debug.Log(text);
                newdatahereboys = true;
                lastReceivedUDPPacket = text;

                allReceivedUDPPackets = allReceivedUDPPackets + text;
            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }
    }

    public void sendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);

        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    public string getLatestUDPPacket()
    {
        allReceivedUDPPackets = "";
        return lastReceivedUDPPacket;
    }

    public void ClosePorts()
    {
        Debug.Log("closing receiving UDP on port: " + port);

        if (receiveThread != null)
            receiveThread.Abort();

        client.Close();
    }
}