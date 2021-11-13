/*
 * Copyright (c) .NET Foundation and Contributors
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *
 * https://github.com/piranhacms/piranha.core
 *
 */

using System;
using System.Collections.Generic;

namespace Piranha.Models
{
    [Serializable]
    public sealed class ContentTypeField
    {
        /// <summary>
        /// Gets/sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets/sets the optional title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets/sets the value type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets/sets the options.
        /// </summary>
        public FieldOptions Options { get; set; }

        /// <summary>
        /// Gets/sets the optional placeholder for
        /// text based fields.
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// Gets/sets the optional description to be shown in
        /// the manager interface.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets/sets the available field settings.
        /// </summary>
        private IDictionary<string, object> settings = new Dictionary<string, object>();
        public IDictionary<string, object> Settings
        {
            get { return settings; }
            set { settings = value; }
        }
    }
}
