using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Util;

namespace Weaver.Core {
	
	/*================================================================================================*/
	public class WeaverConfig : IWeaverConfig {

		internal const char Delim = '.';

		public IList<Type> VertexTypes { get; private set; }
		public IList<Type> EdgeTypes { get; private set; }

		private readonly IDictionary<Type, string> vEdgeDbMap;
		private readonly IDictionary<string, WeaverPropPair> vPropDbMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverConfig(IList<Type> pVertexTypes, IList<Type> pEdgeTypes) {
			VertexTypes = pVertexTypes;
			EdgeTypes = pEdgeTypes;

			vEdgeDbMap = new Dictionary<Type, string>();
			vPropDbMap = new Dictionary<string, WeaverPropPair>();

			BuildMaps<WeaverVertexAttribute>(VertexTypes, (t, a) => {});
			BuildMaps<WeaverEdgeAttribute>(EdgeTypes, (t, a) => vEdgeDbMap.Add(t, a.DbName));
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildMaps<T>(IEnumerable<Type> pTypes, Action<Type, T> pFinish) 
																	where T : WeaverElementAttribute {
			foreach ( Type t in pTypes ) {
				BuildType(t, pFinish);

				IList<WeaverPropPair> props = WeaverUtil.GetElementPropertyAttributes(t);

				foreach ( WeaverPropPair p in props ) {
					BuildProp(p);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildType<T>(Type pType, Action<Type, T> pFinish) where T : WeaverElementAttribute{
			T att = WeaverUtil.GetElementAttribute<T>(pType);

			if ( att == null ) {
				throw new WeaverException("Type '"+pType.Name+"' must have a "+typeof(T).Name+".");
			}

			pFinish(pType, att);
		}

		/*--------------------------------------------------------------------------------------------*/
		private void BuildProp(WeaverPropPair pProp) {
			string dbName = pProp.Attrib.DbName;

			if ( vPropDbMap.ContainsKey(dbName) ) {
				WeaverPropPair wpp = vPropDbMap[dbName];

				//Duplicate DbNames are ignored if they come from the same declaring type. This occurs
				//for WeaverElement.Id, for example, or a common Vertex base class.
				
				Type t = wpp.Info.DeclaringType;
				Type expectT = pProp.Info.DeclaringType;

				if ( t == expectT ) {
					return;
				}

				//Sometimes, a property can be shared by a generic base class. For example: base class
				//EdgeBase<Person, Knows, T> contains a "MyNumber" property. The condition below will
				//prevent exceptions when multiple subclasses (like CandyEdge : EdgeBase<Candy>) 
				//all try to register the same shared "MyNumber" property.

				if ( t.Namespace == expectT.Namespace && t.Name == expectT.Name ) {
					return;
				}

				throw new WeaverException("Duplicate property DbName found: '"+dbName+"'.");
			}

			vPropDbMap.Add(dbName, pProp);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string GetEdgeDbName<T>(T pItem) where T : IWeaverEdge {
			return GetEdgeDbName(pItem.GetType());
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public string GetEdgeDbName<T>() where T : IWeaverEdge {
			return GetEdgeDbName(typeof(T));
		}

		/*--------------------------------------------------------------------------------------------*/
		private string GetEdgeDbName(Type pType) {
			if ( !vEdgeDbMap.ContainsKey(pType) ) {
				throw new KeyNotFoundException(pType.Name);
			}

			return vEdgeDbMap[pType];
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetPropertyDbName<T>(Expression<Func<T, object>> pExp) where T : IWeaverElement {
			return WeaverUtil.GetPropertyDbName(pExp);
		}

	}

}