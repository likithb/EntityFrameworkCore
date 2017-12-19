// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.TestUtilities;

namespace Microsoft.EntityFrameworkCore.Query
{
    public partial class SimpleQueryDocumentDbTest : SimpleQueryTestBase<NorthwindQueryDocumentDbFixture<NoopModelCustomizer>>
    {
        public override void Where_simple()
        {
            base.Where_simple();

            AssertSql(
                @"SELECT c.CustomerID, c.Address, c.City, c.CompanyName, c.ContactName, c.ContactTitle, c.Country, c.Fax, c.Phone, c.PostalCode, c.Region
FROM Customers c
WHERE c.City = ""London""");
        }

        public override void Where_simple_closure()
        {
            base.Where_simple_closure();

            AssertSql(
                @"SELECT c.CustomerID, c.Address, c.City, c.CompanyName, c.ContactName, c.ContactTitle, c.Country, c.Fax, c.Phone, c.PostalCode, c.Region
FROM Customers c
WHERE c.City = @__city_0");
        }

        public override void Where_indexer_closure()
        {
            base.Where_indexer_closure();

            //AssertSql(" ");
        }

        public override void Where_dictionary_key_access_closure()
        {
            base.Where_dictionary_key_access_closure();
        }

        public override void Where_tuple_item_closure()
        {
            base.Where_tuple_item_closure();
        }

        public override void Where_named_tuple_item_closure()
        {
            base.Where_named_tuple_item_closure();
        }

        public override void Where_simple_closure_constant()
        {
            base.Where_simple_closure_constant();
        }

        public override void Where_simple_closure_via_query_cache()
        {
            base.Where_simple_closure_via_query_cache();
        }

        public override void Where_method_call_nullable_type_closure_via_query_cache()
        {
            base.Where_method_call_nullable_type_closure_via_query_cache();
        }

        public override void Where_method_call_nullable_type_reverse_closure_via_query_cache()
        {
            base.Where_method_call_nullable_type_reverse_closure_via_query_cache();
        }

        public override void Where_method_call_closure_via_query_cache()
        {
            base.Where_method_call_closure_via_query_cache();
        }

        public override void Where_field_access_closure_via_query_cache()
        {
            base.Where_field_access_closure_via_query_cache();
        }

        public override void Where_property_access_closure_via_query_cache()
        {
            base.Where_property_access_closure_via_query_cache();
        }

        public override void Where_static_field_access_closure_via_query_cache()
        {
            base.Where_static_field_access_closure_via_query_cache();
        }

        public override void Where_static_property_access_closure_via_query_cache()
        {
            base.Where_static_property_access_closure_via_query_cache();
        }

        public override void Where_nested_field_access_closure_via_query_cache()
        {
            base.Where_nested_field_access_closure_via_query_cache();
        }

        public override void Where_nested_property_access_closure_via_query_cache()
        {
            base.Where_nested_property_access_closure_via_query_cache();
        }

        public override void Where_nested_field_access_closure_via_query_cache_error_null()
        {
            base.Where_nested_field_access_closure_via_query_cache_error_null();
        }

        public override void Where_nested_field_access_closure_via_query_cache_error_method_null()
        {
            base.Where_nested_field_access_closure_via_query_cache_error_method_null();
        }

        public override void Where_new_instance_field_access_closure_via_query_cache()
        {
            base.Where_new_instance_field_access_closure_via_query_cache();
        }

        public override void Where_simple_closure_via_query_cache_nullable_type()
        {
            base.Where_simple_closure_via_query_cache_nullable_type();
        }

        public override void Where_simple_closure_via_query_cache_nullable_type_reverse()
        {
            base.Where_simple_closure_via_query_cache_nullable_type_reverse();
        }

        public override void Where_subquery_closure_via_query_cache()
        {
            base.Where_subquery_closure_via_query_cache();
        }

        public override void Where_simple_shadow()
        {
            base.Where_simple_shadow();
        }

        public override void Where_simple_shadow_projection()
        {
            base.Where_simple_shadow_projection();
        }

        public override void Where_simple_shadow_projection_mixed()
        {
            base.Where_simple_shadow_projection_mixed();
        }

        public override void Where_simple_shadow_subquery()
        {
            base.Where_simple_shadow_subquery();
        }

        public override void Where_shadow_subquery_FirstOrDefault()
        {
            base.Where_shadow_subquery_FirstOrDefault();
        }

        public override void Where_client()
        {
            base.Where_client();
        }

        public override void Where_subquery_correlated()
        {
            base.Where_subquery_correlated();
        }

        public override void Where_comparison_nullable_type_null()
        {
            base.Where_comparison_nullable_type_null();
        }

        public override void Where_null_is_null()
        {
            base.Where_null_is_null();
        }

        public override void Where_comparison_nullable_type_not_null()
        {
            base.Where_comparison_nullable_type_not_null();
        }

        public override void Where_bool_member_compared_to_binary_expression()
        {
            base.Where_bool_member_compared_to_binary_expression();
        }

        public override void Where_comparison_to_nullable_bool()
        {
            base.Where_comparison_to_nullable_bool();
        }

        public override void Where_de_morgan_and_optimizated()
        {
            base.Where_de_morgan_and_optimizated();
        }

        public override void Where_not_in_optimization1()
        {
            base.Where_not_in_optimization1();
        }

        public override void Where_bool_member_negated_twice()
        {
            base.Where_bool_member_negated_twice();
        }

        public override void Where_bool_closure()
        {
            base.Where_bool_closure();
        }

        public override void Where_bool_member_in_complex_predicate()
        {
            base.Where_bool_member_in_complex_predicate();
        }

        public override void Where_ternary_boolean_condition_with_false_as_result_true()
        {
            base.Where_ternary_boolean_condition_with_false_as_result_true();
        }

        public override void Where_navigation_contains()
        {
            base.Where_navigation_contains();
        }
    }
}
