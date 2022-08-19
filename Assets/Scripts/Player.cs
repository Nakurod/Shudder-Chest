using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum HitBox
{
    FrontUpperLeft,
    FrontUpperRight,
    FrontLowerLeft,
    FrontLowerRight,
    BackUpperLeft,
    BackUpperRight,
    BackLowerLeft,
    BackLowerRight
}

public class Player : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float vibrationTime;
    [SerializeField] Transform head;
    [SerializeField] string ipAddress = "192.168.1.195";
    [SerializeField] int port = 12345;

    public UDP sender = new UDP();

    public string datafromnode;

    private void Start()
    {

        sender.init(ipAddress, port, 12345);
        sender.sendString("Hello from Start. ");
    }

    public void TakeDamage(HitBox hitBox)
    {
        switch (hitBox)
        {
            case HitBox.FrontUpperLeft:
                sender.sendString("fulon");
                StartCoroutine(CloseMotor("fuloff"));
                break;
            case HitBox.FrontUpperRight:
                sender.sendString("furon");
                StartCoroutine(CloseMotor("furoff"));
                break;
            case HitBox.FrontLowerLeft:
                sender.sendString("fllon");
                StartCoroutine(CloseMotor("flloff"));
                break;
            case HitBox.FrontLowerRight:
                sender.sendString("flron");
                StartCoroutine(CloseMotor("flroff"));
                break;
            case HitBox.BackUpperLeft:
                sender.sendString("bulon");
                StartCoroutine(CloseMotor("buloff"));
                break;
            case HitBox.BackUpperRight:
                sender.sendString("buron");
                StartCoroutine(CloseMotor("buroff"));
                break;
            case HitBox.BackLowerLeft:
                sender.sendString("bllon");
                StartCoroutine(CloseMotor("blloff"));
                break;
            case HitBox.BackLowerRight:
                sender.sendString("blron");
                StartCoroutine(CloseMotor("blroff"));
                break;
            default:
                break;
        }

    }

    private IEnumerator CloseMotor(string message)
    {
        yield return new WaitForSeconds(vibrationTime);
        sender.sendString(message);
    }

    public Vector3 GetHeadPosition()
    {
        return head.position;
    }

    public void OnDisable()
    {
        sender.sendString("fuloff");
        sender.sendString("furoff");
        sender.sendString("flloff");
        sender.sendString("flroff");
        sender.sendString("buloff");
        sender.sendString("buroff");
        sender.sendString("blloff");
        sender.sendString("blroff");
        sender.ClosePorts();
    }

    public void OnApplicationQuit()
    {
        sender.sendString("fuloff");
        sender.sendString("furoff");
        sender.sendString("flloff");
        sender.sendString("flroff");
        sender.sendString("buloff");
        sender.sendString("buroff");
        sender.sendString("blloff");
        sender.sendString("blroff");
        sender.ClosePorts();
    }
}