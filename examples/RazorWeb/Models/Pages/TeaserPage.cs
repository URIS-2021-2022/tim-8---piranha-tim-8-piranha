/*
 * Copyright (c) .NET Foundation and Contributors
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *
 * http://github.com/tidyui/coreweb
 *
 */

using System.Collections.Generic;
using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Models;

namespace RazorWeb.Models
{
    /// <summary>
    /// Basic page with main content in markdown.
    /// </summary>
    [PageType(Title = "Teaser Page")]
    [ContentTypeRoute(Title = "Default", Route = "/teaserpage")]
    public class TeaserPage : Page<TeaserPage>
    {
        /// <summary>
        /// Gets/sets the page header.
        /// </summary>
        [Region(Description = "The Hero is shown on the top of your page")]
        public Regions.Hero Hero { get; set; }

        [Region(
            Title = "All fields",
            Description = "Vestibulum id ligula porta felis euismod <strong>semper</strong>. Curabitur blandit tempus porttitor."
        )]
        public Regions.AllFields AllFields { get; set; }

        [Region(Title = "Images", ListTitle = "Title", ListPlaceholder = "New image", Icon = "fas fa-images", ListExpand = false)]
        public IList<ImageField> Images { get; set; }

        /// <summary>
        /// Gets/sets the available teasers.
        /// </summary>
        [Region(ListTitle = "Title", ListPlaceholder = "New Teaser", Icon = "fas fa-bookmark")]
        public IList<Regions.Teaser> Teasers { get; set; } = new List<Regions.Teaser>();

        /// <summary>
        /// Gets/sets the available teasers.
        /// </summary>
        [Region(ListTitle = "Title", ListPlaceholder = "New Quote", Icon = "fas fa-quote-right")]
        public IList<Regions.Teaser> Quotes { get; set; } = new List<Regions.Teaser>();

        /// <summary>
        /// Gets/sets the latest post.
        /// </summary>
        public PostInfo LatestPost { get; set; }
    }
}
