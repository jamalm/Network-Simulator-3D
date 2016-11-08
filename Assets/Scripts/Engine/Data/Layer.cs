using UnityEngine;
using System.Collections;

public abstract class Layer
{
	private bool active;
	private string protocol;

	public Layer(){

	}

	public Layer (string protocol) {
		this.protocol = protocol;
	}

	public virtual string getMAC(string type){ return "";}
	public virtual void setMAC(string MAC, string type){}
	public virtual string getIP(string type){ return "";}
	public virtual void setIP(string IP, string type){}

	public string getProtocol(){
		return protocol;
	}
	public void setProtocol(string _protocol){
		protocol = _protocol;
	}

	public bool isActive(){
		return active;
	}
	public void setActive(bool set){
		active = set;
	}
}

