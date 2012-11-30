﻿namespace Weaver {
	
	/*================================================================================================*/
	public class WeaverQueryVal {

		public object Original { get; private set; }
		public bool AllowQuote { get; private set; }

		public string RawText { get; private set; }
		public string FixedText { get; private set; }
		public bool IsString { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQueryVal(object pValue, bool pAllowQuote=true) {
			Original = pValue;
			AllowQuote = pAllowQuote;

			RawText = pValue+"";
			FixedText = RawText;
			IsString = (pValue is string);

			if ( Original is bool ) {
				FixedText = RawText.ToLower();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetQuoted() {
			if ( IsString && AllowQuote ) {
				return "'"+FixedText+"'";
			}

			return FixedText;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetQuotedForce() {
			return "'"+FixedText+"'";
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string QuoteIfString(object pValue) {
			return new WeaverQueryVal(pValue).GetQuoted();
		}

	}

}