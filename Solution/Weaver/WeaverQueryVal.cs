using Weaver.Interfaces;

namespace Weaver {
	
	/*================================================================================================*/
	public class WeaverQueryVal : IWeaverQueryVal {

		public object Original { get; private set; }
		public bool AllowQuote { get; private set; }

		public string RawText { get; private set; }
		public string FixedText { get; private set; }
		//public string NumericSuffixText { get; private set; }
		public bool IsString { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQueryVal(object pValue, bool pAllowQuote=true) {
			Original = pValue;
			AllowQuote = pAllowQuote;

			RawText = pValue+"";
			FixedText = RawText;
			IsString = (pValue is string);

			if ( Original == null ) {
				AllowQuote = false;
				FixedText = "null";
			}
			/*else if ( IsString ) {
				FixedText = FixedText.Replace("'", "\\'");
			}*/
			else if ( Original is bool ) {
				FixedText = RawText.ToLower();
			}
			else if ( Original is double ) {
				FixedText = RawText;
				//NumericSuffixText = RawText+"D";
			}
			else if ( Original is float ) {
				FixedText = RawText;
				//NumericSuffixText = RawText+"F";
			}
			else if ( Original is long ) {
				FixedText = RawText;
				//NumericSuffixText = RawText+"L";
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

	}

}