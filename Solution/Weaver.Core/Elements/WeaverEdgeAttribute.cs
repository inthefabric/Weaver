using System;

namespace Weaver.Core.Elements {

	/*================================================================================================*/
	[AttributeUsage(AttributeTargets.Class)]
	public class WeaverEdgeAttribute : WeaverElementAttribute {

		public string DbName { get; private set; }
		public Type OutVertex { get; private set; }
		public Type InVertex { get; private set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverEdgeAttribute(string DbName, Type OutVertex, Type InVertex) {
			this.DbName = DbName;
			this.OutVertex = OutVertex;
			this.InVertex = InVertex;
		}

	}

}