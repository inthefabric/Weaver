using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Items;
using Weaver.Schema;

namespace Weaver {
	
	/*================================================================================================*/
	public class WeaverConfig : IWeaverConfig {

		internal const char Delim = '.';

		public IList<WeaverNodeSchema> Nodes { get; private set; }
		public IList<WeaverRelSchema> Rels { get; private set; }

		public IDictionary<string, WeaverItemSchema> ItemNameMap { get; private set; }
		public IDictionary<string, WeaverPropSchema> ItemPropNameMap { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverConfig(IList<WeaverNodeSchema> pNodes, IList<WeaverRelSchema> pRels) {
			Nodes = pNodes;
			Rels = pRels;

			ItemNameMap = new Dictionary<string, WeaverItemSchema>();
			ItemPropNameMap = new Dictionary<string, WeaverPropSchema>();

			foreach ( WeaverNodeSchema ns in Nodes ) {
				ItemNameMap.Add(ns.Name, ns);
				Console.WriteLine("WcNode: "+ns.Name);

				foreach ( WeaverPropSchema ps in ns.Props ) {
					ItemPropNameMap.Add(ns.Name+Delim+ps.Name, ps);
					Console.WriteLine(" - "+ps.Name);
				}
			}

			foreach ( WeaverRelSchema rs in Rels ) {
				ItemNameMap.Add(rs.Name, rs);
				Console.WriteLine("WcRel: "+rs.Name);

				foreach ( WeaverPropSchema ps in rs.Props ) {
					ItemPropNameMap.Add(rs.Name+Delim+ps.Name, ps);
					Console.WriteLine(" - "+ps.Name);
				}
			}

			Console.WriteLine("==============================");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string GetPropertyName<T>(IWeaverFunc pFunc, Expression<Func<T, object>> pExp)
																		where T : IWeaverItemIndexable {
			return WeaverUtil.GetPropertyName(this, pFunc, pExp);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetPropertyName<T>(Expression<Func<T, object>> pExp)
																		where T : IWeaverItemIndexable {
			return WeaverUtil.GetPropertyName(this, pExp);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetPropertyName<T>(string pProp) where T : IWeaverItemIndexable {
			Type t = typeof(T);
			string dbName;
			WeaverException firstEx = null;

			while ( true ) {
				if ( t == typeof(Object) ) {
					if ( firstEx == null ) {
						throw new WeaverException("Unknown property "+typeof(T).Name+"."+pProp);
					}

					throw firstEx;
				}

				Console.WriteLine("GetPropName: "+t.Name+" / "+pProp);

				if ( !ItemNameMap.ContainsKey(t.Name) ) {
					firstEx = new WeaverException("Unknown item type: "+t.Name);
					t = t.BaseType;
					continue;
				}

				WeaverItemSchema wis = ItemNameMap[t.Name];
				string propKey = wis.Name+Delim+pProp;

				if ( !ItemPropNameMap.ContainsKey(propKey) ) {
					firstEx = new WeaverException("Unknown property "+propKey);
					t = t.BaseType;
					continue;
				}

				Console.WriteLine(" - Item: "+pProp);
				WeaverPropSchema ps = ItemPropNameMap[propKey];
				dbName = ps.DbName;
				break;
			}

			return dbName;
		}

	}

}