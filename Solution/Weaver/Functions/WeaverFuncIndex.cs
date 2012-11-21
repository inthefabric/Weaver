using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncIndex : WeaverFunc {

		public bool ValueIsString { get; private set; }

		private readonly string vIndexName;
		private readonly string vValue;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncIndex(string pIndexName, string pValue, bool pValueIsString) {
			vIndexName = pIndexName;
			vValue = pValue;
			ValueIsString = pValueIsString;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string IndexName {
			get { return vIndexName+""; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public string Value {
			get { return vValue+""; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get {
				string val = vValue;
				if ( ValueIsString ) { val = "'"+val+"'"; }
				return "index.get('"+vIndexName+"', "+val+")";
			}
		}

	}

}