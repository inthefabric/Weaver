using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Fabric.Test.Utils {

	/*================================================================================================*/
	public static class WeaverTestUtils {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Exception CheckThrows(Type pExType, bool pThrows, TestDelegate pFunc) {
			if ( pThrows ) { return Assert.Throws(pExType, pFunc); }
			Assert.DoesNotThrow(pFunc);
			return null;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void CheckListProps<TL>(IList<TL> pList, IList<uint> pExpectIds,
																			Func<TL, object> pGetProp) {
			int n = pList.Count;
			string name = typeof(TL).Name;

			Assert.NotNull(pList,name+" list should be filled.");
			Assert.AreEqual(pExpectIds.Count, n, "Incorrect "+name+" list length.");

			for ( int i = 0 ; i < n ; ++i ) {
				Assert.AreEqual(pExpectIds[i], pGetProp(pList[i]), "Invalid "+name+".Id at index "+i);
			}
		}

	}

}