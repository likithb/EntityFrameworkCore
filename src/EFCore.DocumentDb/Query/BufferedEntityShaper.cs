// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class BufferedEntityShaper<TEntity> : Shaper, IShaper<TEntity>
    {
        public bool IsTrackingQuery { get; }
        public IKey Key { get; }
        private readonly Func<ValueBuffer, object> _materializer;

        public BufferedEntityShaper(bool trackingQuery, IKey key, Func<ValueBuffer, object> materializer)
        {
            IsTrackingQuery = trackingQuery;
            Key = key;
            _materializer = materializer;
        }

        public TEntity Shape(QueryContext queryContext, ValueBuffer valueBuffer)
        {
            var entity
                = (TEntity)queryContext.QueryBuffer
                    .GetEntity(
                        Key,
                        new EntityLoadInfo(valueBuffer, _materializer),
                        queryStateManager: IsTrackingQuery,
                        throwOnNullKey: false);

            return entity;
        }

        public override Type Type => typeof(TEntity);
    }
}
