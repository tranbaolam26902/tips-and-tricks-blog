using Mapster;
using TipsAndTricks.Core.DTO;
using TipsAndTricks.Core.Entities;
using TipsAndTricks.Services;
using TipsAndTricks.WebApp.Areas.Admin.Models;

namespace TipsAndTricks.WebApp.Mapster {
    public class MapsterConfiguration : IRegister {
        public void Register(TypeAdapterConfig config) {
            config.NewConfig<Post, PostItem>()
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.Tags, src => src.Tags.Select(x => x.Name));

            config.NewConfig<PostFilterModel, PostQuery>()
                .Map(dest => dest.Published, src => false);
        }
    }
}
