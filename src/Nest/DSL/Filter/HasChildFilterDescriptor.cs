﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nest.Resolvers.Converters;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Nest.Resolvers;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeConverter<HasChildFilterDescriptor<object>>))]
	public interface IHasChildFilter : IFilterBase
	{
		[JsonProperty("type")]
		TypeNameMarker Type { get; set; }

		[JsonProperty("_scope")]
		string Scope { get; set; }

		[JsonProperty("query")]
		IQueryDescriptor Query { get; set; }
	}

	public class HasChildFilterDescriptor<T> : FilterBase, IHasChildFilter where T : class
	{
		bool IFilterBase.IsConditionless
		{
			get
			{
				var hf = ((IHasChildFilter)this);
				return hf.Query == null || hf.Query.IsConditionless || hf.Type.IsNullOrEmpty();
			}
		}

		TypeNameMarker IHasChildFilter.Type { get; set; }

		string IHasChildFilter.Scope { get; set;}
		
		IQueryDescriptor IHasChildFilter.Query { get; set; }

		public HasChildFilterDescriptor()
		{
			((IHasChildFilter)this).Type = TypeNameMarker.Create<T>();
		}

		public HasChildFilterDescriptor<T> Query(Func<QueryDescriptor<T>, BaseQuery> querySelector)
		{
			var q = new QueryDescriptor<T>();
			((IHasChildFilter)this).Query = querySelector(q);
			return this;
		}
		
		public HasChildFilterDescriptor<T> Scope(string scope)
		{
			((IHasChildFilter)this).Scope = scope;
			return this;
		}

		public HasChildFilterDescriptor<T> Type(string type)
		{
			((IHasChildFilter)this).Type = type;
			return this;
		}
	}
}
