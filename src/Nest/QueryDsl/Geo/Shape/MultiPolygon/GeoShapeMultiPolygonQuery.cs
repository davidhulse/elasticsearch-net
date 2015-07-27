﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nest.Resolvers;
using Newtonsoft.Json;
using Elasticsearch.Net;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public interface IGeoShapeMultiPolygonQuery : IGeoShapeQuery
	{
		[JsonProperty("shape")]
		IMultiPolygonGeoShape Shape { get; set; }
	}

	public class GeoShapeMultiPolygonQuery : FieldNameQuery, IGeoShapeMultiPolygonQuery
	{
		bool IQuery.Conditionless => IsConditionless(this);
		public IMultiPolygonGeoShape Shape { get; set; }

		protected override void WrapInContainer(IQueryContainer c) => c.GeoShape = this;
		internal static bool IsConditionless(IGeoShapeMultiPolygonQuery q) => q.Field.IsConditionless() || q.Shape == null || !q.Shape.Coordinates.HasAny();
	}

	public class GeoShapeMultiPolygonQueryDescriptor<T> 
		: FieldNameQueryDescriptor<GeoShapeMultiPolygonQueryDescriptor<T>, IGeoShapeMultiPolygonQuery, T>
		, IGeoShapeMultiPolygonQuery where T : class
	{
		private IGeoShapeMultiPolygonQuery Self { get { return this; }}
		bool IQuery.Conditionless => GeoShapeMultiPolygonQuery.IsConditionless(this);
		IMultiPolygonGeoShape IGeoShapeMultiPolygonQuery.Shape { get; set; }

		public GeoShapeMultiPolygonQueryDescriptor<T> Coordinates(IEnumerable<IEnumerable<IEnumerable<IEnumerable<double>>>> coordinates)
		{
			if (Self.Shape == null)
				Self.Shape = new MultiPolygonGeoShape();
			Self.Shape.Coordinates = coordinates;
			return this;
		}
	}
}