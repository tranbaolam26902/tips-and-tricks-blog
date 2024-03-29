﻿using Mapster;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.WebApi.Models.Authors;
using TipsAndTricks.WebApi.Models.Categories;
using TipsAndTricks.WebApi.Models.Posts;
using TipsAndTricks.WebApi.Models.Tags;

namespace TipsAndTricks.WebApi.Mapsters {
    public class MapsterConfiguration : IRegister {
        public void Register(TypeAdapterConfig config) {
            config.NewConfig<Author, AuthorDTO>();
            config.NewConfig<Author, AuthorItem>().Map(dest => dest.PostCount, src => src.Posts == null ? 0 : src.Posts.Count);
            config.NewConfig<AuthorEditModel, Author>();
            config.NewConfig<Category, CategoryDTO>();
            config.NewConfig<Category, CategoryItem>().Map(dest => dest.PostCount, src => src.Posts == null ? 0 : src.Posts.Count);
            config.NewConfig<Tag, TagDTO>();
            config.NewConfig<Tag, TagItem>().Map(dest => dest.PostCount, src => src.Posts == null ? 0 : src.Posts.Count);
            config.NewConfig<Post, PostDTO>();
            config.NewConfig<Post, PostDetail>();
        }
    }
}
