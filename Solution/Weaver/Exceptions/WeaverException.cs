﻿using System;

namespace Weaver.Exceptions {

	/*================================================================================================*/
	public class WeaverException : Exception {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverException(string pMessage) : base(pMessage) {}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverException(string pName, string pItem, string pMessage) :
																base(pName+" '"+pItem+"': "+pMessage) {}

	}

}