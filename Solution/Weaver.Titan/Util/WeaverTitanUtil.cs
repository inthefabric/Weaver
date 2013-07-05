using System;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Titan.Elements;

namespace Weaver.Core.Util {

	/*================================================================================================*/
	internal static class WeaverTitanUtil {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T GetAndVerifyElementAttribute<T>(Type pType) where T : WeaverElementAttribute {
			T att = WeaverUtil.GetElementAttribute<T>(pType);
			
			if ( att == null ) {
				throw new WeaverException("Type '"+pType.Name+"' must have a "+typeof(T).Name+".");
			}
			
			return att;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static WeaverTitanPropertyAttribute GetAndVerifyTitanPropertyAttribute(
													WeaverPropPair pPair, bool pIgnoreNonTitan=false) {
			WeaverTitanPropertyAttribute a = (pPair.Attrib as WeaverTitanPropertyAttribute);
			
			if ( !pIgnoreNonTitan && a == null ) {
				throw new WeaverException("Type '"+pPair.Info.Name+"' must have a "+
					typeof(WeaverTitanPropertyAttribute).Name+".");
			}
			
			return a;
		}

	}

}