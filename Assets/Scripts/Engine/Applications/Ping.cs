﻿using UnityEngine;
using System.Collections;

public class Ping : MonoBehaviour
{
    public GameObject packetprefab;
    public bool response; //for external response
    private bool answer;    //to end loop

    void Start()
    {
        response = false;
        answer = false;
    }

    private void Update()
    {

    }
    
    public Packet Echo(string IP)
    {
        GameObject pingPacket = Instantiate(packetprefab);
        Packet packet = pingPacket.GetComponent<Packet>();
        packet.CreatePacket("PING");
        packet.internet.setIP(IP, "dest");
        packet.internet.setIP(GetComponent<PC>().IP, "src");
        packet.gameObject.AddComponent<ICMP>();
        ICMP icmp = packet.GetComponent<ICMP>();
        icmp.CreateICMP("ECHO", IP);

        return packet;
    }

    public Packet Reply(string IP)
    {
        GameObject pingPacket = Instantiate(packetprefab);
        Packet packet = pingPacket.GetComponent<Packet>();
        packet.CreatePacket("PING");
        packet.internet.setIP(IP, "dest");
        packet.internet.setIP(GetComponent<PC>().IP, "src");
        packet.gameObject.AddComponent<ICMP>();
        ICMP icmp = packet.GetComponent<ICMP>();
        icmp.CreateICMP("REPLY", IP);

        return packet;
    }

    public bool WaitForResponse(string IP)
    {
        StartCoroutine(Wait());
        return answer;
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
        if (response)
        {
            answer = true;
        }else
        {
            answer = false;
        }
    }
}
