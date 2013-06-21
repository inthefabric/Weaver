using System;
using System.Collections.Generic;
using Weaver.Core.Exceptions;
using Weaver.Core.Schema;
using Weaver.Titan.Elements;

namespace Weaver.Titan.Schema {

	/*================================================================================================*/
	public class WeaverTitanPropSchema : WeaverPropSchema {
		
		public bool? TitanIndex { get; set; }
		public bool? TitanElasticIndex { get; set; }

		private readonly IDictionary<string, WeaverEdgeSchema> vVertCent;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverTitanPropSchema(string pName, string pDbName, Type pType) :	
																		base(pName, pDbName, pType) {
			vVertCent = new Dictionary<string, WeaverEdgeSchema>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public int AddTitanVertexCentricIndex(WeaverEdgeSchema pEdge) {
			if ( vVertCent.ContainsKey(pEdge.DbName) ) {
				throw new WeaverException(GetType().Name, DbName, "A vertex-centric index was already "+
					"added for the edge named '"+pEdge.DbName+"'.");
			}

			vVertCent.Add(pEdge.DbName, pEdge);
			return vVertCent.Keys.Count;
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool HasTitanVertexCentricIndex(WeaverEdgeSchema pEdge) {
			return HasTitanVertexCentricIndex(pEdge.DbName);
		}

		/*--------------------------------------------------------------------------------------------*/
		public bool HasTitanVertexCentricIndex(string pEdgeDbName) {
			return vVertCent.ContainsKey(pEdgeDbName);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string GetTitanTypeName() {
			return WeaverTitanPropertyAttribute.GetTitanTypeName(Type);
		}

	}

}