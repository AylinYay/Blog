using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess;

namespace Business.Services
{
    public interface ITagService : IService<TagModel>
    {
    }

    public class TagService : ITagService
    {
        private readonly RepoBase<Tag> _tagRepo;

        public TagService(RepoBase<Tag> tagRepo)
        {
            _tagRepo = tagRepo;
        }

        public Result Add(TagModel model)
        {
            Tag tagEntity = new Tag()
            {
                Name = model.Name,
                IsPopular = model.IsPopular,               
            };

            _tagRepo.Add(tagEntity);

            return new SuccessResult("Tag added successfully.");
        }

        public Result Delete(int id)
        {
            _tagRepo.Delete(id);
            return new SuccessResult("Tag deleted successfully.");
        }

        public void Dispose()
        {
            _tagRepo.Dispose();
        }

        public IQueryable<TagModel> Query()
        {
            return _tagRepo.Query().OrderBy(t => t.Name).Select(t => new TagModel()
            {
                Guid = t.Guid,              
                Id = t.Id,
                IsPopular = t.IsPopular,
                Name = t.Name + (t.IsPopular ? " *" : ""),
                IsPopularDisplay = t.IsPopular ? "Yes" : "No",
                BlogCountDisplay = t.BlogTags.Count
            });
        }

        public Result Update(TagModel model)
        {
            _tagRepo.Delete<BlogTag>(bt => bt.TagId == model.Id);

            var entity = new Tag()
            {
                Name = model.Name,
                IsPopular = model.IsPopular,
                Guid = model.Guid,
                Id = model.Id
            };

            _tagRepo.Update(entity);

            return new SuccessResult("Tag updated successfully.");
        }
    }
}
