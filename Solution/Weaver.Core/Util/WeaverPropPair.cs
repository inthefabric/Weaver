using System.Reflection;
using Weaver.Core.Elements;

namespace Weaver.Core.Util {

	/*================================================================================================*/
	internal class WeaverPropPair {

		public WeaverPropertyAttribute Attrib { get; private set; }
		public PropertyInfo Info { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal WeaverPropPair(WeaverPropertyAttribute pAttrib, PropertyInfo pInfo) {
			Attrib = pAttrib;
			Info = pInfo;
		}

	}

}