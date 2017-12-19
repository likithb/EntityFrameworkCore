// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Remotion.Linq.Clauses;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class SelectExpression : FromExpression
    {
        private ExpressionEqualityComparer _expressionEqualityComparer = new ExpressionEqualityComparer();
        public List<Expression> SelectList { get; } = new List<Expression>();
        public List<FromExpression> FromSpecifications { get; } = new List<FromExpression>();
        public Expression FilterCondition { get => _filterCondition; set => _filterCondition = value; }

        private Expression _filterCondition;
        //private List<Ordering> _sortSpecifications = new List<Ordering>();

        public SelectExpression(IQuerySource querySource, string alias)
            : base(querySource, alias)
        {
        }

        public void AddFromSpecification(FromExpression fromExpression)
        {
            FromSpecifications.Add(fromExpression);
        }

        public Expression BindProperty(
            IProperty property,
            IQuerySource querySource)
        {
            if (FromSpecifications.Where(f => f.QuerySource == querySource)
                .FirstOrDefault() is CollectionExpression fromExpression)
            {
                return new ColumnExpression(property.Name, property, fromExpression);
            }
            return null;
        }

        public int AddToSelectList(IProperty property, IQuerySource querySource)
        {
            var boundProperty = BindProperty(property, querySource);
            var projectionIndex = SelectList.FindIndex(e => _expressionEqualityComparer.Equals(e, boundProperty));
            if (projectionIndex != -1)
            {
                return projectionIndex;
            }

            SelectList.Add(boundProperty);

            return SelectList.Count - 1;
        }

        public void AddFilterCondition(Expression filterCondition)
        {
            if (FilterCondition == null)
            {
                FilterCondition = filterCondition;
            }
            else
            {
                FilterCondition = AndAlso(FilterCondition, filterCondition);
            }
        }

        public override bool HandlesQuerySource(IQuerySource querySource)
        {
            return FromSpecifications.Any(f => f.QuerySource == querySource || f.HandlesQuerySource(querySource));
        }

        public QuerySqlGenerator GetSqlGenerator()
        {
            return new QuerySqlGenerator(this);
        }

        public IEnumerable<IProperty> GetProjectedProperties()
        {
            return SelectList.Select(e => e is ColumnExpression ce ? ce.Property : null);
        }

        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return GetSqlGenerator().GenerateSql(new Dictionary<string, object>());
        }
    }
}
