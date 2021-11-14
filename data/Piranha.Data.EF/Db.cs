/*
 * Copyright (c) .NET Foundation and Contributors
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 *
 * http://github.com/piranhacms/piranha
 *
 */

using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Piranha
{
    public abstract class Db<T> : DbContext, IDb where T : Db<T>
    {
        /// <summary>
        /// Gets/sets whether the db context as been initialized. This
        /// is only performed once in the application lifecycle.
        /// </summary>
        private volatile bool IsInitialized = false;

        /// <summary>
        /// The object mutex used for initializing the context.
        /// </summary>
        private static readonly object Mutex = new object();

        /// <summary>
        /// Gets/sets the alias set.
        /// </summary>
        public DbSet<Data.Alias> Aliases { get; set; }

        /// <summary>
        /// Gets/sets the block set.
        /// </summary>
        public DbSet<Data.Block> Blocks { get; set; }

        /// <summary>
        /// Gets/sets the block field set.
        /// </summary>
        public DbSet<Data.BlockField> BlockFields { get; set; }

        /// <summary>
        /// Gets/sets the category set.
        /// </summary>
        public DbSet<Data.Category> Categories { get; set; }

        /// <summary>
        /// Gets/sets the content set.
        /// </summary>
        public DbSet<Data.Content> Content { get; set; }

        /// <summary>
        /// Gets/sets the content block set.
        /// </summary>
        public DbSet<Data.ContentBlock> ContentBlocks { get; set; }

        /// <summary>
        /// Gets/sets the content block field set.
        /// </summary>
        public DbSet<Data.ContentBlockField> ContentBlockFields { get; set; }

        /// <summary>
        /// Gets/sets the content block field translation set.
        /// </summary>
        public DbSet<Data.ContentBlockFieldTranslation> ContentBlockFieldTranslations { get; set; }

        /// <summary>
        /// Gets/sets the content field set.
        /// </summary>
        public DbSet<Data.ContentField> ContentFields { get; set; }

        /// <summary>
        /// Gets/sets the content field translation set.
        /// </summary>
        public DbSet<Data.ContentFieldTranslation> ContentFieldTranslations { get; set; }

        /// <summary>
        /// Gets/sets the content taxonomy set.
        /// </summary>
        public DbSet<Data.ContentTaxonomy> ContentTaxonomies { get; set; }

        /// <summary>
        /// Gets/sets the content translation set.
        /// </summary>
        public DbSet<Data.ContentTranslation> ContentTranslations { get; set; }

        /// <summary>
        /// Gets/sets the content group set.
        /// </summary>
        public DbSet<Data.ContentGroup> ContentGroups { get; set; }

        /// <summary>
        /// Gets/sets the content type set.
        /// </summary>
        public DbSet<Data.ContentType> ContentTypes { get; set; }

        /// <summary>
        /// Gets/sets the language set.
        /// </summary>
        public DbSet<Data.Language> Languages { get; set; }

        /// <summary>
        /// Gets/sets the media set.
        /// </summary>
        public DbSet<Data.Media> Media { get; set; }

        /// <summary>
        /// Gets/sets the media folder set.
        /// </summary>
        public DbSet<Data.MediaFolder> MediaFolders { get; set; }

        /// <summary>
        /// Gets/sets the media version set.
        /// </summary>
        public DbSet<Data.MediaVersion> MediaVersions { get; set; }

        /// <summary>
        /// Gets/sets the page set.
        /// </summary>
        public DbSet<Data.Page> Pages { get; set; }

        /// <summary>
        /// Gets/sets the page block set.
        /// </summary>
        public DbSet<Data.PageBlock> PageBlocks { get; set; }

        /// <summary>
        /// Gets/sets the page comments.
        /// </summary>
        public DbSet<Data.PageComment> PageComments { get; set; }

        /// <summary>
        /// Gets/sets the page field set.
        /// </summary>
        public DbSet<Data.PageField> PageFields { get; set; }

        /// <summary>
        /// Gets/sets the page permission set.
        /// </summary>
        public DbSet<Data.PagePermission> PagePermissions { get; set; }

        /// <summary>
        /// Gets/sets the page revision set.
        /// </summary>
        public DbSet<Data.PageRevision> PageRevisions { get; set; }

        /// <summary>
        /// Gets/sets the page type set.
        /// </summary>
        public DbSet<Data.PageType> PageTypes { get; set; }

        /// <summary>
        /// Gets/sets the param set.
        /// </summary>
        public DbSet<Data.Param> Params { get; set; }

        /// <summary>
        /// Gets/sets the post set.
        /// </summary>
        public DbSet<Data.Post> Posts { get; set; }

        /// <summary>
        /// Gets/sets the post block set.
        /// </summary>
        public DbSet<Data.PostBlock> PostBlocks { get; set; }

        /// <summary>
        /// Gets/sets the post comments.
        /// </summary>
        public DbSet<Data.PostComment> PostComments { get; set; }

        /// <summary>
        /// Gets/sets the post field set.
        /// </summary>
        public DbSet<Data.PostField> PostFields { get; set; }

        /// <summary>
        /// Gets/sets the post permission set.
        /// </summary>
        public DbSet<Data.PostPermission> PostPermissions { get; set; }

        /// <summary>
        /// Gets/sets the post revision set.
        /// </summary>
        public DbSet<Data.PostRevision> PostRevisions { get; set; }

        /// <summary>
        /// Gets/sets the post tag set.
        /// </summary>
        public DbSet<Data.PostTag> PostTags { get; set; }

        /// <summary>
        /// Gets/sets the post type set.
        /// </summary>
        public DbSet<Data.PostType> PostTypes { get; set; }

        /// <summary>
        /// Gets/sets the site set.
        /// </summary>
        public DbSet<Data.Site> Sites { get; set; }

        /// <summary>
        /// Gets/sets the site field set.
        /// </summary>
        public DbSet<Data.SiteField> SiteFields { get; set; }

        /// <summary>
        /// Gets/sets the site type set.
        /// </summary>
        public DbSet<Data.SiteType> SiteTypes { get; set; }

        /// <summary>
        /// Gets/sets the tag set.
        /// </summary>
        public DbSet<Data.Tag> Tags { get; set; }

        /// <summary>
        /// Gets/sets the taxonomy set.
        /// </summary>
        public DbSet<Data.Taxonomy> Taxonomies { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="options">Configuration options</param>
        protected Db(DbContextOptions<T> options) : base(options)
        {
            if (!IsInitialized)
            {
                lock (Mutex)
                {
                    if (!IsInitialized)
                    {
                        // Migrate database
                        Database.Migrate();
                        // Seed
                        Seed();

                        IsInitialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Creates and configures the data model.
        /// </summary>
        /// <param name="modelBuilder">The current model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Data.Alias>().ToTable("Piranha_Aliases");
            modelBuilder.Entity<Data.Alias>().Property(a => a.AliasUrl).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Data.Alias>().Property(a => a.RedirectUrl).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Data.Alias>().HasIndex(a => new { a.SiteId, a.AliasUrl }).IsUnique();

            modelBuilder.Entity<Data.Block>().ToTable("Piranha_Blocks");
            modelBuilder.Entity<Data.Block>().Property(b => b.CLRType).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Data.Block>().Property(b => b.Title).HasMaxLength(128);

            modelBuilder.Entity<Data.BlockField>().ToTable("Piranha_BlockFields");
            modelBuilder.Entity<Data.BlockField>().Property(f => f.FieldId).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.BlockField>().Property(f => f.CLRType).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Data.BlockField>().HasIndex(f => new { f.BlockId, f.FieldId, f.SortOrder }).IsUnique();

            modelBuilder.Entity<Data.Category>().ToTable("Piranha_Categories");
            modelBuilder.Entity<Data.Category>().Property(c => c.Title).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.Category>().Property(c => c.Slug).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.Category>().HasIndex(c => new { c.BlogId, c.Slug }).IsUnique();

            modelBuilder.Entity<Data.Content>().ToTable("Piranha_Content");
            modelBuilder.Entity<Data.Content>().Property(p => p.TypeId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.Content>().Ignore(p => p.SelectedLanguageId);

            modelBuilder.Entity<Data.ContentBlock>().ToTable("Piranha_ContentBlocks");
            modelBuilder.Entity<Data.ContentBlock>().Property(b => b.CLRType).IsRequired().HasMaxLength(256);

            modelBuilder.Entity<Data.ContentBlockField>().ToTable("Piranha_ContentBlockFields");
            modelBuilder.Entity<Data.ContentBlockField>().Property(f => f.FieldId).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.ContentBlockField>().Property(f => f.CLRType).IsRequired().HasMaxLength(256);
            modelBuilder.Entity<Data.ContentBlockField>().HasIndex(f => new { f.BlockId, f.FieldId, f.SortOrder }).IsUnique();

            modelBuilder.Entity<Data.ContentBlockFieldTranslation>().ToTable("Piranha_ContentBlockFieldTranslations");
            modelBuilder.Entity<Data.ContentBlockFieldTranslation>().HasKey(t => new { t.FieldId, t.LanguageId });

            modelBuilder.Entity<Data.ContentField>().ToTable("Piranha_ContentFields");
            modelBuilder.Entity<Data.ContentField>().Property(f => f.RegionId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.ContentField>().Property(f => f.FieldId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.ContentField>().Property(f => f.CLRType).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<Data.ContentField>().HasIndex(f => new { f.ContentId, f.RegionId, f.FieldId, f.SortOrder });

            modelBuilder.Entity<Data.ContentFieldTranslation>().ToTable("Piranha_ContentFieldTranslations");
            modelBuilder.Entity<Data.ContentFieldTranslation>().HasKey(t => new { t.FieldId, t.LanguageId });

            modelBuilder.Entity<Data.ContentGroup>().ToTable("Piranha_ContentGroups");
            modelBuilder.Entity<Data.ContentGroup>().Property(t => t.Id).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.ContentGroup>().Property(t => t.CLRType).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Data.ContentGroup>().Property(t => t.Title).IsRequired().HasMaxLength(128);

            modelBuilder.Entity<Data.ContentTaxonomy>().ToTable("Piranha_ContentTaxonomies");
            modelBuilder.Entity<Data.ContentTaxonomy>().HasKey(t => new { t.ContentId, t.TaxonomyId });
            modelBuilder.Entity<Data.ContentTaxonomy>().HasOne(t => t.Taxonomy).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Data.ContentTranslation>().ToTable("Piranha_ContentTranslations");
            modelBuilder.Entity<Data.ContentTranslation>().HasKey(t => new { t.ContentId, t.LanguageId });
            modelBuilder.Entity<Data.ContentTranslation>().Property(p => p.Title).HasMaxLength(128).IsRequired();

            modelBuilder.Entity<Data.ContentType>().ToTable("Piranha_ContentTypes");
            modelBuilder.Entity<Data.ContentType>().Property(t => t.Id).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.ContentType>().Property(t => t.Group).IsRequired().HasMaxLength(64);

            modelBuilder.Entity<Data.Language>().ToTable("Piranha_Languages");
            modelBuilder.Entity<Data.Language>().Property(l => l.Title).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.Language>().Property(l => l.Culture).HasMaxLength(6);

            modelBuilder.Entity<Data.Media>().ToTable("Piranha_Media");
            modelBuilder.Entity<Data.Media>().Property(m => m.Filename).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Data.Media>().Property(m => m.ContentType).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<Data.Media>().Property(m => m.Title).HasMaxLength(128);
            modelBuilder.Entity<Data.Media>().Property(m => m.AltText).HasMaxLength(128);
            modelBuilder.Entity<Data.Media>().Property(m => m.Description).HasMaxLength(512);

            modelBuilder.Entity<Data.MediaFolder>().ToTable("Piranha_MediaFolders");
            modelBuilder.Entity<Data.MediaFolder>().Property(f => f.Name).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Data.MediaFolder>().Property(f => f.Description).HasMaxLength(512);

            modelBuilder.Entity<Data.MediaVersion>().ToTable("Piranha_MediaVersions");
            modelBuilder.Entity<Data.MediaVersion>().Property(v => v.FileExtension).HasMaxLength(8);
            modelBuilder.Entity<Data.MediaVersion>().HasIndex(v => new { v.MediaId, v.Width, v.Height }).IsUnique();

            modelBuilder.Entity<Data.Page>().ToTable("Piranha_Pages");
            modelBuilder.Entity<Data.Page>().Property(p => p.PageTypeId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.Page>().Property(p => p.ContentType).HasMaxLength(255).IsRequired().HasDefaultValue("Page");
            modelBuilder.Entity<Data.Page>().Property(p => p.Title).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Data.Page>().Property(p => p.NavigationTitle).HasMaxLength(128);
            modelBuilder.Entity<Data.Page>().Property(p => p.Slug).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Data.Page>().Property(p => p.MetaTitle).HasMaxLength(128);
            modelBuilder.Entity<Data.Page>().Property(p => p.MetaKeywords).HasMaxLength(128);
            modelBuilder.Entity<Data.Page>().Property(p => p.MetaDescription).HasMaxLength(256);
            modelBuilder.Entity<Data.Page>().Property(p => p.MetaIndex).HasDefaultValue(true);
            modelBuilder.Entity<Data.Page>().Property(p => p.MetaFollow).HasDefaultValue(true);
            modelBuilder.Entity<Data.Page>().Property(p => p.MetaPriority).HasDefaultValue(0.5);
            modelBuilder.Entity<Data.Page>().Property(p => p.OgTitle).HasMaxLength(128);
            modelBuilder.Entity<Data.Page>().Property(p => p.OgDescription).HasMaxLength(256);
            modelBuilder.Entity<Data.Page>().Property(p => p.Route).HasMaxLength(256);
            modelBuilder.Entity<Data.Page>().Property(p => p.RedirectUrl).HasMaxLength(256);
            modelBuilder.Entity<Data.Page>().Property(p => p.EnableComments).HasDefaultValue(false);
            modelBuilder.Entity<Data.Page>().HasIndex(p => new { p.SiteId, p.Slug }).IsUnique();

            modelBuilder.Entity<Data.PageBlock>().ToTable("Piranha_PageBlocks");
            modelBuilder.Entity<Data.PageBlock>().HasIndex(b => new { b.PageId, b.SortOrder }).IsUnique();

            modelBuilder.Entity<Data.PageComment>().ToTable("Piranha_PageComments");
            modelBuilder.Entity<Data.PostComment>().Property(c => c.UserId).HasMaxLength(128);
            modelBuilder.Entity<Data.PageComment>().Property(c => c.Author).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Data.PageComment>().Property(c => c.Email).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Data.PageComment>().Property(c => c.Url).HasMaxLength(256);

            modelBuilder.Entity<Data.PageField>().ToTable("Piranha_PageFields");
            modelBuilder.Entity<Data.PageField>().Property(f => f.RegionId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.PageField>().Property(f => f.FieldId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.PageField>().Property(f => f.CLRType).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<Data.PageField>().HasIndex(f => new { f.PageId, f.RegionId, f.FieldId, f.SortOrder });

            modelBuilder.Entity<Data.PagePermission>().ToTable("Piranha_PagePermissions");
            modelBuilder.Entity<Data.PagePermission>().HasKey(p => new { p.PageId, p.Permission });

            modelBuilder.Entity<Data.PageRevision>().ToTable("Piranha_PageRevisions");

            modelBuilder.Entity<Data.PageType>().ToTable("Piranha_PageTypes");
            modelBuilder.Entity<Data.PageType>().Property(p => p.Id).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.PageType>().Property(p => p.CLRType).HasMaxLength(256);

            modelBuilder.Entity<Data.Param>().ToTable("Piranha_Params");
            modelBuilder.Entity<Data.Param>().Property(p => p.Key).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.Param>().Property(p => p.Description).HasMaxLength(256);
            modelBuilder.Entity<Data.Param>().HasIndex(p => p.Key).IsUnique();

            modelBuilder.Entity<Data.Post>().ToTable("Piranha_Posts");
            modelBuilder.Entity<Data.Post>().Property(p => p.PostTypeId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.Post>().Property(p => p.Title).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Data.Post>().Property(p => p.Slug).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Data.Post>().Property(p => p.MetaTitle).HasMaxLength(128);
            modelBuilder.Entity<Data.Post>().Property(p => p.MetaKeywords).HasMaxLength(128);
            modelBuilder.Entity<Data.Post>().Property(p => p.MetaDescription).HasMaxLength(256);
            modelBuilder.Entity<Data.Post>().Property(p => p.MetaIndex).HasDefaultValue(true);
            modelBuilder.Entity<Data.Post>().Property(p => p.MetaFollow).HasDefaultValue(true);
            modelBuilder.Entity<Data.Post>().Property(p => p.MetaPriority).HasDefaultValue(0.5);
            modelBuilder.Entity<Data.Post>().Property(p => p.OgTitle).HasMaxLength(128);
            modelBuilder.Entity<Data.Post>().Property(p => p.OgDescription).HasMaxLength(256);
            modelBuilder.Entity<Data.Post>().Property(p => p.Route).HasMaxLength(256);
            modelBuilder.Entity<Data.Post>().Property(p => p.RedirectUrl).HasMaxLength(256);
            modelBuilder.Entity<Data.Post>().Property(p => p.EnableComments).HasDefaultValue(false);
            modelBuilder.Entity<Data.Post>().HasOne(p => p.Category).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Data.Post>().HasIndex(p => new { p.BlogId, p.Slug }).IsUnique();

            modelBuilder.Entity<Data.PostBlock>().ToTable("Piranha_PostBlocks");
            modelBuilder.Entity<Data.PostBlock>().HasIndex(b => new { b.PostId, b.SortOrder }).IsUnique();

            modelBuilder.Entity<Data.PostComment>().ToTable("Piranha_PostComments");
            modelBuilder.Entity<Data.PostComment>().Property(c => c.UserId).HasMaxLength(128);
            modelBuilder.Entity<Data.PostComment>().Property(c => c.Author).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Data.PostComment>().Property(c => c.Email).HasMaxLength(128).IsRequired();
            modelBuilder.Entity<Data.PostComment>().Property(c => c.Url).HasMaxLength(256);

            modelBuilder.Entity<Data.PostField>().ToTable("Piranha_PostFields");
            modelBuilder.Entity<Data.PostField>().Property(f => f.RegionId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.PostField>().Property(f => f.FieldId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.PostField>().Property(f => f.CLRType).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<Data.PostField>().HasIndex(f => new { f.PostId, f.RegionId, f.FieldId, f.SortOrder });

            modelBuilder.Entity<Data.PostPermission>().ToTable("Piranha_PostPermissions");
            modelBuilder.Entity<Data.PostPermission>().HasKey(p => new { p.PostId, p.Permission });

            modelBuilder.Entity<Data.PostRevision>().ToTable("Piranha_PostRevisions");

            modelBuilder.Entity<Data.PostTag>().ToTable("Piranha_PostTags");
            modelBuilder.Entity<Data.PostTag>().HasKey(t => new { t.PostId, t.TagId });
            modelBuilder.Entity<Data.PostTag>().HasOne(t => t.Tag).WithMany().IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Data.PostType>().ToTable("Piranha_PostTypes");
            modelBuilder.Entity<Data.PostType>().Property(p => p.Id).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.PostType>().Property(p => p.CLRType).HasMaxLength(256);

            modelBuilder.Entity<Data.Site>().ToTable("Piranha_Sites");
            modelBuilder.Entity<Data.Site>().Property(s => s.SiteTypeId).HasMaxLength(64);
            modelBuilder.Entity<Data.Site>().Property(s => s.InternalId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.Site>().Property(s => s.Title).HasMaxLength(128);
            modelBuilder.Entity<Data.Site>().Property(s => s.Description).HasMaxLength(256);
            modelBuilder.Entity<Data.Site>().Property(s => s.Hostnames).HasMaxLength(256);
            modelBuilder.Entity<Data.Site>().Property(s => s.Culture).HasMaxLength(6);
            modelBuilder.Entity<Data.Site>().HasOne(s => s.Language).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Data.Site>().HasIndex(s => s.InternalId).IsUnique();


            modelBuilder.Entity<Data.SiteField>().ToTable("Piranha_SiteFields");
            modelBuilder.Entity<Data.SiteField>().Property(f => f.RegionId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.SiteField>().Property(f => f.FieldId).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.SiteField>().Property(f => f.CLRType).HasMaxLength(256).IsRequired();
            modelBuilder.Entity<Data.SiteField>().HasIndex(f => new { f.SiteId, f.RegionId, f.FieldId, f.SortOrder });

            modelBuilder.Entity<Data.SiteType>().ToTable("Piranha_SiteTypes");
            modelBuilder.Entity<Data.SiteType>().Property(s => s.Id).HasMaxLength(64).IsRequired();
            modelBuilder.Entity<Data.SiteType>().Property(s => s.CLRType).HasMaxLength(256);

            modelBuilder.Entity<Data.Tag>().ToTable("Piranha_Tags");
            modelBuilder.Entity<Data.Tag>().Property(t => t.Title).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.Tag>().Property(t => t.Slug).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.Tag>().HasIndex(t => new { t.BlogId, t.Slug }).IsUnique();

            modelBuilder.Entity<Data.Taxonomy>().ToTable("Piranha_Taxonomies");
            modelBuilder.Entity<Data.Taxonomy>().Property(t => t.GroupId).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.Taxonomy>().Property(t => t.Title).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.Taxonomy>().Property(t => t.Slug).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Data.Taxonomy>().HasIndex(t => new { t.GroupId, t.Type, t.Slug }).IsUnique();
        }

        /// <summary>
        /// Seeds the default data.
        /// </summary>
        private void Seed()
        {
            SaveChanges();

            //
            // Params
            //
            var param = Params.FirstOrDefault(p => p.Key == Config.ARCHIVE_PAGE_SIZE);
            if (param == null)
                Params.Add(new Data.Param
                {
                    Id = Guid.NewGuid(),
                    Key = Config.ARCHIVE_PAGE_SIZE,
                    Value = 5.ToString(),
                    Created = DateTime.Now,
                    LastModified = DateTime.Now
                });

            param = Params.FirstOrDefault(p => p.Key == Config.CACHE_EXPIRES_PAGES);
            if (param == null)
                Params.Add(new Data.Param
                {
                    Id = Guid.NewGuid(),
                    Key = Config.CACHE_EXPIRES_PAGES,
                    Value = 0.ToString(),
                    Created = DateTime.Now,
                    LastModified = DateTime.Now
                });

            param = Params.FirstOrDefault(p => p.Key == Config.CACHE_EXPIRES_POSTS);
            if (param == null)
                Params.Add(new Data.Param
                {
                    Id = Guid.NewGuid(),
                    Key = Config.CACHE_EXPIRES_POSTS,
                    Value = 0.ToString(),
                    Created = DateTime.Now,
                    LastModified = DateTime.Now
                });

            param = Params.FirstOrDefault(p => p.Key == Config.PAGES_HIERARCHICAL_SLUGS);
            if (param == null)
                Params.Add(new Data.Param
                {
                    Id = Guid.NewGuid(),
                    Key = Config.PAGES_HIERARCHICAL_SLUGS,
                    Value = true.ToString(),
                    Created = DateTime.Now,
                    LastModified = DateTime.Now
                });

            param = Params.FirstOrDefault(p => p.Key == Config.MANAGER_EXPANDED_SITEMAP_LEVELS);
            if (param == null)
                Params.Add(new Data.Param
                {
                    Id = Guid.NewGuid(),
                    Key = Config.MANAGER_EXPANDED_SITEMAP_LEVELS,
                    Value = 1.ToString(),
                    Created = DateTime.Now,
                    LastModified = DateTime.Now
                });

            param = Params.FirstOrDefault(p => p.Key == Config.MEDIA_CDN_URL);
            if (param == null)
                Params.Add(new Data.Param
                {
                    Id = Guid.NewGuid(),
                    Key = Config.MEDIA_CDN_URL,
                    Created = DateTime.Now,
                    LastModified = DateTime.Now
                });

            //
            // Default language
            //
            var langId = Guid.NewGuid();

            if (!Languages.Any())
            {
                Languages.Add(new Data.Language
                {
                    Id = langId,
                    Title = "Default",
                    Culture = "en-US",
                    IsDefault = true
                });
            }
            else
            {
                langId = Languages.FirstOrDefault(l => l.IsDefault).Id;
            }

            //
            // Default site
            //
            if (Sites.Count() == 0)
            {
                Sites.Add(new Data.Site
                {
                    Id = Guid.NewGuid(),
                    LanguageId = langId,
                    InternalId = "Default",
                    IsDefault = true,
                    Title = "Default Site",
                    Created = DateTime.Now,
                    LastModified = DateTime.Now
                });
            }
            else
            {
                // When upgrading, make sure we assign the default language id
                // to already created sites.
                foreach (var site in Sites.Where(s => s.LanguageId == Guid.Empty))
                {
                    site.LanguageId = langId;
                }
            }

            //
            // Make sure we don't have NULL values in Piranha_MediaVersions.FileExtension
            //
            var versions = MediaVersions
                .Where(m => m.FileExtension == null)
                .ToList();
            foreach (var version in versions)
                version.FileExtension = ".jpg";

            var pageBlocks = PageBlocks
                .Where(b => b.ParentId.HasValue)
                .ToList();
            var pageBlocksId = pageBlocks.Select(b => b.BlockId).ToList();
            var blocks = Blocks
                .Where(b => pageBlocksId.Contains(b.Id))
                .ToList();
            foreach (var block in blocks)
            {
                var pageBlock = pageBlocks.Single(b => b.BlockId == block.Id);
                block.ParentId = pageBlock.ParentId;
                pageBlock.ParentId = null;
            }
            var postBlocks = PostBlocks
                .Where(b => b.ParentId.HasValue)
                .ToList();
            var postBlocksId = postBlocks.Select(b => b.BlockId).ToList();
            blocks = Blocks
                .Where(b => postBlocksId.Contains(b.Id))
                .ToList();
            foreach (var block in blocks)
            {
                var postBlock = postBlocks.Single(b => b.BlockId == block.Id);
                block.ParentId = postBlock.ParentId;
                postBlock.ParentId = null;
            }

            SaveChanges();
        }
    }
}
