using System;


public class TransLayer : Layer
{
	public TransLayer (string protocol)
	{
		setProtocol (protocol);
		setup ();
	}

	private void setup(){
		if (getProtocol().Equals ("TCP")) {
			//add something to packet
			setActive(true);
		} else {
			//TODO for other protocols (UDP?)
			setActive(false);
		}
	}
}


