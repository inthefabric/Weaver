using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Schema;
using Weaver.Core.Steps;
using Weaver.Core.Util;

namespace Weaver.Core {
	
	/*================================================================================================*/
	public class WeaverConfig : IWeaverConfig {

		internal const char Delim = '.';

		public IList<WeaverVertexSchema> VertexSchemas { get; private set; }
		public IList<WeaverEdgeSchema> EdgeSchemas { get; private set; }

		private readonly IDictionary<string, WeaverItemSchema> vItemNameMap;
		private readonly IDictionary<string, WeaverPropSchema> vItemPropNameMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverConfig(IList<WeaverVertexSchema> pVertexSchemas,
																IList<WeaverEdgeSchema> pEdgeSchemas) {
			VertexSchemas = pVertexSchemas;
			EdgeSchemas = pEdgeSchemas;

			vItemNameMap = new Dictionary<string, WeaverItemSchema>();
			vItemPropNameMap = new Dictionary<string, WeaverPropSchema>();
			string key;

			foreach ( WeaverVertexSchema ns in VertexSchemas ) {
				key = CheckItemKey(ns.Name);
				vItemNameMap.Add(key, ns);
				//Console.WriteLine("WcNode: "+ns.Name+" / "+ns.DbName);

				foreach ( WeaverPropSchema ps in ns.Props ) {
					key = CheckItemPropKey(ns.Name, ps.Name);
					vItemPropNameMap.Add(key, ps);
					//Console.WriteLine(" - "+ps.Name+" / "+ps.DbName);
				}
			}

			foreach ( WeaverEdgeSchema rs in EdgeSchemas ) {
				key = CheckItemKey(rs.Name);
				vItemNameMap.Add(key, rs);
				//Console.WriteLine("WcRel: "+rs.Name+" / "+rs.DbName+" ... "+key);

				foreach ( WeaverPropSchema ps in rs.Props ) {
					key = CheckItemPropKey(rs.Name, ps.Name);
					vItemPropNameMap.Add(key, ps);
					//Console.WriteLine(" - "+ps.Name+" / "+ps.DbName+" ... "+key);
				}
			}

			//Console.WriteLine("==============================");
		}

		/*--------------------------------------------------------------------------------------------*/
		private string CheckItemKey(string pName) {
			if ( vItemNameMap.ContainsKey(pName) ) {
				throw new WeaverException("An item with name '"+pName+"' has already been added.");
			}

			return pName;
		}

		/*--------------------------------------------------------------------------------------------*/
		private string CheckItemPropKey(string pItemName, string pPropName) {
			string key = pItemName+Delim+pPropName;

			if ( vItemPropNameMap.ContainsKey(key) ) {
				throw new WeaverException("An item property with full name '"+pItemName+"."+pPropName+
					"' has already been added.");
			}

			return key;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string GetItemDbName<T>(T pItem) where T : IWeaverElement {
			return GetItemDbName<T>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public string GetItemDbName<T>() where T : IWeaverElement {
			return GetItemDbName(typeof(T).Name);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetItemDbName(string pItemName) {
			if ( !vItemNameMap.ContainsKey(pItemName) ) {
				throw new KeyNotFoundException(pItemName);
			}

			return vItemNameMap[pItemName].DbName;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string GetPropertyDbName<T>(IWeaverStep pStep, Expression<Func<T, object>> pExp)
																			where T : IWeaverElement {
			return WeaverUtil.GetPropertyName(this, pStep, pExp);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetPropertyDbName<T>(Expression<Func<T, object>> pExp) where T : IWeaverElement {
			return WeaverUtil.GetPropertyName(this, pExp);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetPropertyDbName<T>(string pProp) where T : IWeaverElement {
			Type t = typeof(T);
			string dbName;
			string exList = "";

			while ( true ) {
				if ( t == null || t == typeof(Object) ) {
					throw new WeaverException(
						"Could not find property DB name:\n"+exList);
				}

				string typeName = t.Name.Replace("`1", "");
				//Console.WriteLine("GetPropName: "+typeName+" / "+pProp);

				if ( !vItemNameMap.ContainsKey(typeName) ) {
					exList += " - Unknown item type: "+typeName+".\n";
					t = t.BaseType;
					continue;
				}

				WeaverItemSchema wis = vItemNameMap[typeName];
				string propKey = wis.Name+Delim+pProp;

				if ( !vItemPropNameMap.ContainsKey(propKey) ) {
					exList += " - Unknown property: "+propKey+".\n";
					t = t.BaseType;
					continue;
				}

				WeaverPropSchema ps = vItemPropNameMap[propKey];
				dbName = ps.DbName;
				break;
			}

			return dbName;
		}

	}

}