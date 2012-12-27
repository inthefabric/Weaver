﻿namespace Weaver.Interfaces {
	
	/*================================================================================================*/
	public interface IWeaverQueryVal {

		object Original { get; }
		bool AllowQuote { get; }

		string RawText { get; }
		string FixedText { get; }
		bool IsString { get; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		string GetQuoted();
		string GetQuotedForce();

	}

}