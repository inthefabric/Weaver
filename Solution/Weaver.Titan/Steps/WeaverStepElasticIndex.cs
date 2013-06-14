﻿using System.Text;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Titan.Steps.Parameters;

namespace Weaver.Titan.Steps {
	
	/*================================================================================================*/
	public class WeaverStepElasticIndex<T> : WeaverStep where T : IWeaverElement {

		private readonly bool vVertMode;
		private readonly IWeaverParamElastic<T>[] vParams;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepElasticIndex(bool pVertMode, params IWeaverParamElastic<T>[] pParams) {
			vVertMode = pVertMode;
			vParams = pParams;
		}

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
				
				sb.Append(
					(sb.Length > 0 ? "." : "")+
					"has('"+propName+"',"+pe.GetOperationScript()+","+valParam+")"
				);
			}

			return sb+"."+(vVertMode ? "vertices" : "edges")+"()";
		}

	}

}