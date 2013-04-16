using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Schema;

namespace Weaver {
	
	/*================================================================================================*/
	public class WeaverConfig : IWeaverConfig {

		internal const char Delim = '.';

		public IList<WeaverNodeSchema> Nodes { get; private set; }
		public IList<WeaverRelSchema> Rels { get; private set; }

		private readonly IDictionary<string, WeaverItemSchema> vItemNameMap;
		private readonly IDictionary<string, WeaverPropSchema> vItemPropNameMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverConfig(IList<WeaverNodeSchema> pNodes, IList<WeaverRelSchema> pRels) {
			Nodes = pNodes;
			Rels = pRels;

			vItemNameMap = new Dictionary<string, WeaverItemSchema>();
			vItemPropNameMap = new Dictionary<string, WeaverPropSchema>();
			string key;

			foreach ( WeaverNodeSchema ns in Nodes ) {
				key = CheckItemKey(ns.Name);
				vItemNameMap.Add(key, ns);
				//Console.WriteLine("WcNode: "+ns.Name+" / "+ns.DbName);

				foreach ( WeaverPropSchema ps in ns.Props ) {
					key = CheckItemPropKey(ns.Name, ps.Name);
					vItemPropNameMap.Add(key, ps);
					//Console.WriteLine(" - "+ps.Name+" / "+ps.DbName);
				}
			}

			foreach ( WeaverRelSchema rs in Rels ) {
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
		public string GetItemDbName<T>(T pItem) where T : IWeaverItemIndexable {
			return GetItemDbName<T>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public string GetItemDbName<T>() where T : IWeaverItemIndexable {
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
		public string GetPropertyDbName<T>(IWeaverFunc pFunc, Expression<Func<T, object>> pExp)
																		where T : IWeaverItemIndexable {
			return WeaverUtil.GetPropertyName(this, pFunc, pExp);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetPropertyDbName<T>(Expression<Func<T, object>> pExp)
																		where T : IWeaverItemIndexable {
			return WeaverUtil.GetPropertyName(this, pExp);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetPropertyDbName<T>(string pProp) where T : IWeaverItemIndexable {
			Type t = typeof(T);
			string dbName;
			WeaverException firstEx = null;

			while ( true ) {
				if ( t == null || t == typeof(Object) ) {
					if ( firstEx == null ) {
						throw new WeaverException("Unknown property "+typeof(T).Name+"."+pProp);
					}

					throw firstEx;
				}

				//Console.WriteLine("GetPropName: "+t.Name+" / "+pProp);

				if ( !vItemNameMap.ContainsKey(t.Name) ) {
					firstEx = new WeaverException("Unknown item type '"+t.Name+"' while looking for "+
						"property "+typeof(T).Name+"."+pProp);
					t = t.BaseType;
					continue;
				}

				WeaverItemSchema wis = vItemNameMap[t.Name];
				string propKey = wis.Name+Delim+pProp;

				if ( !vItemPropNameMap.ContainsKey(propKey) ) {
					firstEx = new WeaverException("Unknown property "+propKey);
					t = t.BaseType;
					continue;
				}

				//Console.WriteLine(" - Item: "+pProp);
				WeaverPropSchema ps = vItemPropNameMap[propKey];
				dbName = ps.DbName;
				break;
			}

			return dbName;
		}

	}

}