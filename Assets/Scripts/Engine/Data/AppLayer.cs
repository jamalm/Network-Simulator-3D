﻿using System;

public class AppLayer : Layer
{
	private string protocol;

	public AppLayer (string protocol) {
		setProtocol(protocol);
	}

	private void setup(){
		//TODO for application features in the future!
		setActive(false);
	}
}
