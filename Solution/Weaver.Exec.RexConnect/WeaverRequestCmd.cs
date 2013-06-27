using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RexConnectClient.Core;
using RexConnectClient.Core.Transfer;
using Weaver.Core.Query;

namespace Weaver.Exec.RexConnect {

	/*================================================================================================*/
	internal static class WeaverRequestCmd {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static RequestCmd CreateQueryCommand(string pScript,
														IDictionary<string, IWeaverQueryVal> pParams) {
			string q = JsonUnquote(pScript);

			if ( pParams == null || pParams.Keys.Count == 0 ) {
				return new RequestCmd(RexConn.Command.Query.ToString().ToLower(), q);
			}

			const string end = @"(?=$|[^\d])";
			var sb = new StringBuilder();

			foreach ( string key in pParams.Keys ) {
				sb.Append((sb.Length > 0 ? "," : "")+"\""+JsonUnquote(key)+"\":");
				IWeaverQueryVal qv = pParams[key];

				if ( qv.IsString ) {
					sb.Append("\""+JsonUnquote(qv.FixedText)+"\"");
					continue;
				}

				sb.Append(qv.FixedText);

				//Explicitly cast certain parameter types
				//See: https://github.com/tinkerpop/rexster/issues/295

				if ( qv.Original is int ) {
					q = Regex.Replace(q, key+end, key+".toInteger()");
				}
				else if ( qv.Original is byte ) {
					q = Regex.Replace(q, key+end, key+".byteValue()");
				}
				else if ( qv.Original is float ) {
					q = Regex.Replace(q, key+end, key+".toFloat()");
				}
			}

			return new RequestCmd(RexConn.Command.Query.ToString().ToLower(), q, "{"+sb+"}");
		}

		/*--------------------------------------------------------------------------------------------*/
		private static string JsonUnquote(string pText) {
			return pText.Replace("\"", "\\\"");
		}

	}

}