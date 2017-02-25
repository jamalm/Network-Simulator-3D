using System.Text.RegularExpressions;
using System;
using UnityEngine;

public class Subnet : MonoBehaviour {

    public PC pc;
    //this states the PC's subnet
    public string mask;
    public string network;
    public string broadcast;
    public string defaultGateway;
    public bool validMask;
    
	// Use this for initialization
	void Start () {
        pc = GetComponent<PC>();
        mask = "255.255.255.128";
        validMask = ValidateMask(mask);
        network = ResolveNetwork(pc.IP);
        Debug.Log(pc.GetID() + ": Network is: " + network);
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool CheckNetwork(string destIP)
    {
        string destNetwork = ResolveNetwork(destIP);
        if(destNetwork == network)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ValidateMask(string mask)
    {
        //split string up into octets
        string[] maskChunks = mask.Split('.');

        //for integer and binary operations
        int[] maskBitchunks = new int[4];
        int[] invMaskbitChunks = new int[4];

        for(int i=0;i<4;i++)
        {
            //convert to integers for binary ops
            maskBitchunks[i] = int.Parse(maskChunks[i]);

            //TWO's COMPLEMENT
            //get inverse of mask
            invMaskbitChunks[i] = ~maskBitchunks[i];
            // add one to inverse
            int invPlus = invMaskbitChunks[i] + 1;
            //valid only if inv & invplus == 0
            int valid = invPlus & invMaskbitChunks[i];

            //clean up binary to 1 byte 
            string val = Convert.ToString(valid, 2);
            val = val.Substring(Math.Max(val.Length - 8, 0)).PadLeft(8, '0');

            //check if all zeroes
            if(val.Contains("1"))
            {
                return false;
            }
        }
        return true;
    }

    private string ResolveNetwork(string dec)
    {

        string net = "";
        string[] ipChunks = dec.Split('.');
        string[] maskChunks = mask.Split('.');

        int[] ipBitChunks = new int[4];
        int[] maskBitChunks = new int[4];
        int[] netChunks = new int[4];

        for (int i = 0; i < 4; i++)
        {
            ipBitChunks[i] = int.Parse(ipChunks[i]);
            maskBitChunks[i] = int.Parse(maskChunks[i]);

            netChunks[i] = ipBitChunks[i] & maskBitChunks[i];

            if (i != 3)
            {
                net += netChunks[i] + ".";
                defaultGateway += net;
            }
            else
            {
                net += netChunks[i];
                //just adding one to the network to define gateway
                defaultGateway += (netChunks[i] + 1);
            }

        }

        return net;
    }

    private string ResolveBroadcast(string network)
    {

        return "";
    }


}
