﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverTableColumns { //TEST: WeaverTableColumns

		private readonly IWeaverConfig vConfig;
		private readonly List<WeaverTableColumn> vCols;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal WeaverTableColumns(IWeaverConfig pConfig) {
			vConfig = pConfig;
			vCols = new List<WeaverTableColumn>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddObjectColumn<T>() where T : IWeaverItemIndexable {
			vCols.Add(WeaverTableColumn.Build<T>(false));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddNullColumn<T>() where T : IWeaverItemIndexable {
			vCols.Add(WeaverTableColumn.Build<T>(true));
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddPropertyColumn<T>(Expression<Func<T, object>> pItemProperty,
										string pPropertyScript=null) where T : IWeaverItemIndexable {
			vCols.Add(WeaverTableColumn.Build(vConfig, pItemProperty, pPropertyScript));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public int Count {
			get { return vCols.Count; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverTableColumn this[int pIndex] {
			get {
				if ( pIndex < 0 || pIndex >= vCols.Count ) {
					throw new WeaverException(
						"Index "+pIndex+" is out of bounds: [0,"+vCols.Count+"].");
				}

				return vCols[pIndex];
			}
		}

	}

}