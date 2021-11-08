﻿/*
 * Copyright (c) 2016-2019 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *
 * https://github.com/piranhacms/piranha.core
 *
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Piranha.Models;

namespace Piranha.Repositories
{
    public interface IPageRepository
    {
        /// <summary>
        /// Gets all of the available pages for the current site.
        /// </summary>
        /// <param name="siteId">The site id</param>
        /// <returns>The pages</returns>
        Task<IEnumerable<Guid>> GetAll(Guid siteId);

        /// <summary>
        /// Gets the available blog pages for the current site.
        /// </summary>
        /// <param name="siteId">The site id</param>
        /// <returns>The pages</returns>
        Task<IEnumerable<Guid>> GetAllBlogs(Guid siteId);

        /// <summary>
        /// Gets all of the available revisions for the specified
        /// page order by created date.
        /// </summary>
        /// <param name="id">The unique page id</param>
        /// <returns>The available revisions</returns>
        Task<IEnumerable<Revision>> GetAllRevisions(Guid id);

        /// <summary>
        /// Gets the id of all pages that have a draft for
        /// the specified site.
        /// </summary>
        /// <param name="siteId">The unique site id</param>
        /// <returns>The pages that have a draft</returns>
        Task<IEnumerable<Guid>> GetAllDrafts(Guid siteId);

        /// <summary>
        /// Gets the site startpage.
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="siteId">The site id</param>
        /// <returns>The page model</returns>
        Task<T> GetStartpage<T>(Guid siteId) where T : PageBase;

        /// <summary>
        /// Gets the page model with the specified id.
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="id">The unique id</param>
        /// <returns>The page model</returns>
        Task<T> GetById<T>(Guid id) where T : PageBase;

        /// <summary>
        /// Gets the page model with the specified slug.
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="slug">The unique slug</param>
        /// <param name="siteId">The site id</param>
        /// <returns>The page model</returns>
        Task<T> GetBySlug<T>(string slug, Guid siteId) where T : PageBase;

        /// <summary>
        /// Gets the draft for the page model with the specified id.
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="id">The unique id</param>
        /// <returns>The draft, or null if no draft exists</returns>
        Task<T> GetDraftById<T>(Guid id) where T : PageBase;

        /// <summary>
        /// Moves the current page in the structure.
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="model">The page to move</param>
        /// <param name="parentId">The new parent id</param>
        /// <param name="sortOrder">The new sort order</param>
        /// <returns>The other pages that were affected by the move</returns>
        Task<IEnumerable<Guid>> Move<T>(T model, Guid? parentId, int sortOrder) where T : PageBase;

        /// <summary>
        /// Saves the given page model
        /// </summary>
        /// <param name="model">The page model</param>
        /// <returns>The other pages that were affected by the move</returns>
        Task<IEnumerable<Guid>> Save<T>(T model) where T : PageBase;

        /// <summary>
        /// Saves the given model as a draft revision.
        /// </summary>
        /// <param name="model">The page model</param>
        Task SaveDraft<T>(T model) where T : PageBase;

        /// <summary>
        /// Creates a revision from the current version
        /// of the page with the given id.
        /// </summary>
        /// <param name="id">The unique id</param>
        /// <param name="revisions">The maximum number of revisions that should be stored</param>
        Task CreateRevision(Guid id, int revisions);

        /// <summary>
        /// Deletes the model with the specified id.
        /// </summary>
        /// <param name="id">The unique id</param>
        /// <returns>The other pages that were affected by the move</returns>
        Task<IEnumerable<Guid>> Delete(Guid id);

        /// <summary>
        /// Deletes the current draft revision for the page
        /// with the given id.
        /// </summary>
        /// <param name="id">The unique id</param>
        Task DeleteDraft(Guid id);
    }
}
