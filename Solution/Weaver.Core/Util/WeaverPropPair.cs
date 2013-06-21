using System.Reflection;
using Weaver.Core.Elements;

namespace Weaver.Core.Util {

	/*================================================================================================*/
	public class WeaverPropPair {

		public WeaverPropertyAttribute Attrib { get; private set; }
		public PropertyInfo Info { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPropPair(WeaverPropertyAttribute pAttrib, PropertyInfo pInfo) {
			Attrib = pAttrib;
			Info = pInfo;
		}

	}

}