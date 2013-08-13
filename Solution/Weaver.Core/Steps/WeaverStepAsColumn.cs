using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepAsColumn<T> : WeaverStepAs<T>, IWeaverStepAsColumn<T>
																			where T : IWeaverElement {
		
		public string PropName { get; protected set; }
		public string AppendScript { get; set; }
		public string ReplaceScript { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepAsColumn(T pElem, IWeaverConfig pConfig, string pLabel,
											Expression<Func<T, object>> pProperty=null) : base(pElem) {
			Label = pLabel;
			PropName = (pProperty == null ? null : pConfig.GetPropertyDbName(pProperty));
			AppendScript = "";
			ReplaceScript = null;
		}

	}

}