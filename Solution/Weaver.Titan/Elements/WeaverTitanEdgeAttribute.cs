using System;
using Weaver.Core.Elements;

namespace Weaver.Titan.Elements {

	/*================================================================================================*/
	[AttributeUsage(AttributeTargets.Class)]
	public class WeaverTitanEdgeAttribute : WeaverEdgeAttribute {

		public WeaverEdgeConn OutConn { get; set; }
		public WeaverEdgeConn InConn { get; set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverTitanEdgeAttribute(string DbName, WeaverEdgeConn OutConn, Type OutVertex,
							WeaverEdgeConn InConn, Type InVertex) : base(DbName, OutVertex, InVertex) {
			this.OutConn = OutConn;
			this.InConn = InConn;
		}

	}

}