using System.Reflection;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverQuery : IWeaverQuery {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string AddNodeGremlin<T>(T pNode) {
			string grem = "g.addVertex([";
			int i = 0;

			foreach ( PropertyInfo prop in pNode.GetType().GetProperties() ) {
				object[] propAtts = prop.GetCustomAttributes(typeof(WeaverItemPropertyAttribute), true);
				if ( propAtts.Length == 0 ) { continue; }
				object val = prop.GetValue(pNode, null);
				if ( val == null ) { continue; }
				if ( val is string ) { val = "'"+val+"'"; }
				if ( i++ > 0 ) { grem += ","; }
				grem += prop.Name+":"+val;
			}

			return grem+"]);";
		}


	}

}