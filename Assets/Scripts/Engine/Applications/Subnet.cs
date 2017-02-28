using System.Text.RegularExpressions;
using System;
using UnityEngine;

public class Subnet : MonoBehaviour {

    //this states the device's subnet
    public string mask;
    public string network;
    public string broadcast;
    public string defaultGateway;
    public bool validMask;
    public int CIDR;
    
	// Use this for initialization
	void Start ()
    {
        mask = "255.255.255.248";
        validMask = ValidateMask(mask);
        if (!GetComponent<PC>().Equals(null))
        {
            network = ResolveNetwork(GetComponent<PC>().IP);
            broadcast = ResolveBroadcast(GetComponent<PC>().IP);
            Debug.Log(GetComponent<PC>().GetID() + ": Network is: " + network);
        }
        CalculateCIDR();
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
                defaultGateway += netChunks[i] + ".";
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

    private string ResolveBroadcast(string ip)
    {
        string[] ipChunks = ip.Split('.');
        string[] maskChunks = mask.Split('.');

        int[] maskBitChunks = new int[4];
        int[] ipBitChunks = new int[4];
        int[] broadChunks = new int[4];

        string broadcast = "";

        for(int i=0; i<4; i++)
        {
            maskBitChunks[i] = int.Parse(maskChunks[i]);
            ipBitChunks[i] = int.Parse(ipChunks[i]);

            //invert mask
            maskBitChunks[i] = ~maskBitChunks[i];
            //do OR operation on ip with inverse mask
            int broad = ipBitChunks[i] | (maskBitChunks[i]);

            string val = Convert.ToString(broad, 2);
            val = val.Substring(Math.Max(val.Length - 8, 0)).PadLeft(8, '0');
            broadChunks[i] = Convert.ToInt32(val, 2);
            if (i != 3)
            {
                broadcast += broadChunks[i] + ".";

            } else
            {
                broadcast += broadChunks[i];
            }
        }
        return broadcast;
    }

    void CalculateCIDR()
    {
        string[] bitString = mask.Split('.');
        int[] bits = new int[4];

        for(int i=0; i<4; i++)
        {
            bits[i] = int.Parse(bitString[i]);
            string octet = Convert.ToString(bits[i],2);

            //count the number of 1's in the mask
            foreach (char c in octet)
                if (c == '1') CIDR++;
        }
    }

}
