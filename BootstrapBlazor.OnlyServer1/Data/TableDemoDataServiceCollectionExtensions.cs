using BootstrapBlazor.Components;
using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazor.OnlyServer1.Data
{
    /// <summary>
    /// BootstrapBlazor
    /// </summary>
    public static class TableDemoDataServiceCollectionExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddTableDemoDataService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDataService<>), typeof(TableDemoDataService<>));
            return services;
        }
    }

    /// <summary>
    /// </summary>
    internal class TableDemoDataService<TModel>(IStringLocalizer<Foo> localizer) : DataServiceBase<TModel> where TModel : class, new()
    {
        private static readonly ConcurrentDictionary<Type, Func<IEnumerable<TModel>, string, SortOrder, IEnumerable<TModel>>> SortLambdaCache = new();

        [NotNull]
        private List<TModel>? Items { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public override Task<QueryData<TModel>> QueryAsync(QueryPageOptions options)
        {
            if (Items == null || Items.Count == 0)
            {
                Items = Foo.GenerateFoo(localizer).Cast<TModel>().ToList();
            }

            var items = Items.AsEnumerable();
            var isSearched = false;
            if (options.SearchModel is Foo model)
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    items = items.Cast<Foo>().Where(item => item.Name?.Contains(model.Name, StringComparison.OrdinalIgnoreCase) ?? false).Cast<TModel>();
                }

                if (!string.IsNullOrEmpty(model.Address))
                {
                    items = items.Cast<Foo>().Where(item => item.Address?.Contains(model.Address, StringComparison.OrdinalIgnoreCase) ?? false).Cast<TModel>();
                }

                isSearched = !string.IsNullOrEmpty(model.Name) || !string.IsNullOrEmpty(model.Address);
            }

            if (options.Searches.Count != 0)
            {
                items = items.Where(options.Searches.GetFilterFunc<TModel>(FilterLogic.Or));
            }

            var isFiltered = false;
            if (options.Filters.Count != 0)
            {
                items = items.Where(options.Filters.GetFilterFunc<TModel>());
                isFiltered = true;
            }

            var isSorted = false;
            if (!string.IsNullOrEmpty(options.SortName))
            {
                var invoker = SortLambdaCache.GetOrAdd(typeof(Foo), key => LambdaExtensions.GetSortLambda<TModel>().Compile());
                items = invoker(items, options.SortName, options.SortOrder);
                isSorted = true;
            }

            var total = items.Count();

            return Task.FromResult(new QueryData<TModel>()
            {
                Items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList(),
                TotalCount = total,
                IsFiltered = isFiltered,
                IsSorted = isSorted,
                IsSearch = isSearched
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override Task<bool> SaveAsync(TModel model, ItemChangedType changedType)
        {
            var ret = false;
            if (model is Foo foo)
            {
                if (changedType == ItemChangedType.Add)
                {
                    var id = Items.Count + 1;
                    while (Items.FirstOrDefault(item => (item as Foo)!.Id == id) != null)
                    {
                        id++;
                    }
                    var item = new Foo()
                    {
                        Id = id,
                        Name = foo.Name,
                        Address = foo.Address,
                        Complete = foo.Complete,
                        Count = foo.Count,
                        DateTime = foo.DateTime,
                        Education = foo.Education,
                        Hobby = foo.Hobby
                    } as TModel;
                    Items.Add(item!);
                }
                else
                {
                    var f = Items.OfType<Foo>().FirstOrDefault(i => i.Id == foo.Id);
                    if (f != null)
                    {
                        f.Name = foo.Name;
                        f.Address = foo.Address;
                        f.Complete = foo.Complete;
                        f.Count = foo.Count;
                        f.DateTime = foo.DateTime;
                        f.Education = foo.Education;
                        f.Hobby = foo.Hobby;
                    }
                }
                ret = true;
            }
            return Task.FromResult(ret);
        }

        public override Task<bool> DeleteAsync(IEnumerable<TModel> models)
        {
            foreach (var model in models)
            {
                Items.Remove(model);
            }

            return base.DeleteAsync(models);
        }
    }
}
