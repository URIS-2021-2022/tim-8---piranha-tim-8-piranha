/*
 * Copyright (c) .NET Foundation and Contributors
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *
 * http://github.com/tidyui/coreweb
 *
 */

using System;
using Piranha.Extend;
using Piranha.Extend.Fields;
using Piranha.Extend.Fields.Settings;
using Piranha.Models;

namespace RazorWeb.Models.Regions
{
    /// <summary>
    /// Simple hero region.
    /// </summary>
    public class Hero
    {
        /// <summary>
        /// Gets/sets the optional subtitle.
        /// </summary>
        [Field(Options = FieldOptions.HalfWidth)]
        [StringFieldSettings(MaxLength = 32, DefaultValue = "Olle")]
        public StringField Subtitle { get; set; }

        /// <summary>
        /// Gets/sets the optional primary image.
        /// </summary>
        [Field(Title = "Primary Image", Options = FieldOptions.HalfWidth)]
        public ImageField PrimaryImage { get; set; }

        /// <summary>
        /// Gets/sets the optional ingress.
        /// </summary>
        [Field(Description = "Optional text that is shown on top of the background image")]
        public HtmlField Ingress { get; set; }
    }
}
