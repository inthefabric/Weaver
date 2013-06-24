using System;
using System.Linq;
using Weaver.Core.Elements;

namespace Weaver.Titan.Elements {

	/*================================================================================================*/
	[AttributeUsage(AttributeTargets.Property)]
	public class WeaverTitanPropertyAttribute : WeaverPropertyAttribute {

		public bool TitanIndex { get; set; }
		public bool TitanElasticIndex { get; set; }
		public Type[] EdgesForVertexCentricIndexing { get; set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverTitanPropertyAttribute(string DbName) : base(DbName) {}

		/*--------------------------------------------------------------------------------------------*/
		public bool HasTitanVertexCentricIndex(Type pEdgeType) {
			return (EdgesForVertexCentricIndexing != null &&
				EdgesForVertexCentricIndexing.Any(t => (t == pEdgeType)));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string GetTitanTypeName(Type pType) {
			string n;

			switch ( pType.Name ) {
				case "Boolean":
					n = "Boolean";
					break;

				case "Byte":
					n = "Byte";
					break;

				case "Int32":
					n = "Integer";
					break;

				case "Int64":
				case "DateTime":
					n = "Long";
					break;

				case "Single":
					n = "Float";
					break;

				case "Double":
					n = "Double";
					break;

				case "Nullable`1":
					n = GetTitanTypeName(pType.GetGenericArguments()[0]);
					break;

				default:
					n = pType.Name;
					break;
			}

			return n;
		}

	}

}