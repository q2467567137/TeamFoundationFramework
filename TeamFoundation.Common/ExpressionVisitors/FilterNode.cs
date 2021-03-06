﻿
namespace TeamFoundation.Common.ExpressionVisitors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public enum FilterNodeRelationship
    {
        And,
        Or
    }

    public enum FilterExpressionType
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        NotContains
    }

    public class FilterNode : IEnumerable<FilterNode>
    {
        private FilterNode nextNode;

        public string Key { get; set; }

        public string Value { get; set; }

        public FilterExpressionType Sign { get; set; }

        public FilterNodeRelationship NodeRelationship { get; set; }

        public FilterNode NextNode
        {
            get
            {
                return this.nextNode;
            }
        }

        public static FilterExpressionType ParseFilterExpressionType(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Equal:
                    return FilterExpressionType.Equal;
                case ExpressionType.NotEqual:
                    return FilterExpressionType.NotEqual;
                case ExpressionType.GreaterThan:
                    return FilterExpressionType.GreaterThan;
                case ExpressionType.LessThan:
                    return FilterExpressionType.LessThan;
                case ExpressionType.GreaterThanOrEqual:
                    return FilterExpressionType.GreaterThanOrEqual;
                case ExpressionType.LessThanOrEqual:
                    return FilterExpressionType.LessThanOrEqual;

                default:
                    throw new NotSupportedException("Custom filtering only available with equal, not equal, greater than, less than, greater than or equal, less than or equal operators");
            }
        }

        public void AddNode(FilterNode node)
        {
            var lastItem = this.Last();
            lastItem.nextNode = node;
        }

        public IEnumerator<FilterNode> GetEnumerator()
        {
            return new FilterNodeEnumerator(this);
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new FilterNodeEnumerator(this);
        }

        public FilterNode DeepClone()
        {
            var rootNode = default(FilterNode);
            foreach (var node in this)
            {
                var newNode = new FilterNode() { Key = node.Key, NodeRelationship = node.NodeRelationship, Sign = node.Sign, Value = node.Value };
                if (rootNode != null)
                {
                    rootNode.AddNode(newNode);
                }
                else
                {
                    rootNode = newNode;
                }
            }

            return rootNode;
        }
    }
}
