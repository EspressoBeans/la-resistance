using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectronicsCalcApi.Repositories
{
    public interface IRepository<TEntity, in TPrimaryKey, in TFilter> where TEntity : class
    {
        IEnumerable<TEntity> Get();
        TEntity Get(TPrimaryKey id);
        TEntity Get(TFilter filter);
    }
}