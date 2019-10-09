﻿namespace UglyToad.PdfPig.Content
{
    using System;
    using System.Collections.Generic;
    using Tokens;
    using Util.JetBrains.Annotations;

    /// <summary>
    /// A node in the PDF document's page tree.
    /// Nodes may either be of type 'Page' - a single page, or 'Pages' - a container for multiple child Page
    /// or Pages nodes.
    /// </summary>
    public class PageTreeNode
    {
        /// <summary>
        /// The dictionary for this node in the page tree.
        /// </summary>
        [NotNull]
        public DictionaryToken NodeDictionary { get; }

        /// <summary>
        /// The indirect reference for this node in the page tree.
        /// </summary>
        public IndirectReference Reference { get; }

        /// <summary>
        /// Whether this node is a page or not. If not it must be a /Pages container.
        /// </summary>
        public bool IsPage { get; }

        /// <summary>
        /// The number of this page if <see cref="IsPage"/> is <see langword="true"/>.
        /// </summary>
        public int? PageNumber { get; }

        /// <summary>
        /// The child nodes of this node if <see cref="IsPage"/> is <see langword="false" />
        /// </summary>
        [NotNull]
        public IReadOnlyList<PageTreeNode> Children { get; }

        /// <summary>
        /// The parent node of this node, unless it is the root node.
        /// </summary>
        [CanBeNull]
        public PageTreeNode Parent { get; private set; }

        /// <summary>
        /// Whether this node is the root node.
        /// </summary>
        public bool IsRoot => Parent == null;

        /// <summary>
        /// Create a new <see cref="PageTreeNode"/>.
        /// </summary>
        internal PageTreeNode(DictionaryToken nodeDictionary, IndirectReference reference,
            bool isPage, 
            int? pageNumber, 
            IReadOnlyList<PageTreeNode> children)
        {
            NodeDictionary = nodeDictionary ?? throw new ArgumentNullException(nameof(nodeDictionary));
            Reference = reference;
            IsPage = isPage;
            PageNumber = pageNumber;
            Children = children ?? throw new ArgumentNullException(nameof(children));

            if (IsPage && Children.Count > 0)
            {
                throw new ArgumentException("Cannot define children on a page node.", nameof(children));   
            }

            if (!IsPage && pageNumber.HasValue)
            {
                throw new ArgumentException("Cannot define page number for a pages node.", nameof(pageNumber));
            }

            foreach (var child in Children)
            {
                child.Parent = this;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (IsPage)
            {
                return $"Page #{PageNumber}: {NodeDictionary}.";
            }

            return $"Pages ({Children.Count} children): {NodeDictionary}";
        }
    }
}