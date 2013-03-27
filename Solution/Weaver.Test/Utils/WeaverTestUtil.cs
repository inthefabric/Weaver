using System;
using System.Collections.Generic;
using NUnit.Framework;
using Weaver.Interfaces;

namespace Weaver.Test.Utils {

	/*================================================================================================*/
	public static class WeaverTestUtil {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Exception CheckThrows<TEx>(bool pThrows, TestDelegate pFunc) where TEx :Exception{
			if ( pThrows ) {
				return Assert.Throws(typeof(TEx), pFunc);
			}

			Assert.DoesNotThrow(pFunc);
			return null;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Dictionary<string, string> GetPropListDictionary(string pPropList) {
			string[] valPairs = pPropList.Split(',');
			var map = new Dictionary<string, string>();

			foreach ( string pair in valPairs ) {
				string[] parts = pair.Split(':');
				map.Add(parts[0], parts[1]);
			}

			return map;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static void CheckQueryParamsOriginalVal(IWeaverQuery pQuery, 
														Dictionary<string, IWeaverQueryVal> pExpect) {
			Assert.NotNull(pQuery.Params, "Query.Params should not be null.");
			Assert.AreEqual(pExpect.Keys.Count, pQuery.Params.Keys.Count,
				"Incorrect Query.Params count.");

			foreach ( string key in pExpect.Keys ) {
				Assert.True(pQuery.Params.ContainsKey(key), "Missing Query.Params["+key+"].");
				Assert.AreEqual(pExpect[key].Original, pQuery.Params[key].Original,
					"Incorrect value for Query.Params["+key+"].Original.");
			}
		}

	}

}