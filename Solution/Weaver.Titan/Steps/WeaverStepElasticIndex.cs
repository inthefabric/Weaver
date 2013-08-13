using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Titan.Steps.Parameters;

namespace Weaver.Titan.Steps {
	
	/*================================================================================================*/
	public class WeaverStepElasticIndex<T> : WeaverStep where T : IWeaverElement {

		private readonly IWeaverParamElastic<T>[] vParams;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepElasticIndex(params IWeaverParamElastic<T>[] pParams) {
			vParams = pParams;
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepElasticIndex(Expression<Func<T, object>> pProperty,
																		string pSpaceDelimitedText) {
			string[] tokens = pSpaceDelimitedText.Trim().Split(' ');
			var list = new List<IWeaverParamElastic<T>>();

			foreach ( string t in tokens ) {
				list.Add(new WeaverParamElastic<T>(pProperty, WeaverParamElasticOp.Contains, t));
			}

			vParams = list.ToArray();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			if ( vParams.Length == 0 ) {
				throw new WeaverStepException(this, "ElasticIndex must have at least one parameter.");
			}

			var sb = new StringBuilder();

			foreach ( IWeaverParamElastic<T> pe in vParams ) {
				var prop = pe.Property;
				string propName = WrapException(() => Path.Config.GetPropertyDbName(prop));
				string valParam = Path.Query.AddParam(new WeaverQueryVal(pe.Value));

				//TODO: WeaverStepElasticIndex: use query parameter (for PropertyName);

				sb.Append(
					(sb.Length > 0 ? "." : "")+
					"has('"+propName+"',"+pe.GetOperationScript()+","+valParam+")"
				);
			}

			return sb.ToString();
		}

	}

}