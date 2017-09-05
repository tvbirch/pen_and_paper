using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Linq;
using System.Web;

namespace RPG.Models.Context
{
    //public class Configuration : DbConfiguration
    //{
    //    public Configuration()
    //    {
    //        //https://blog.3d-logic.com/2014/03/20/second-level-cache-for-ef-6-1/
    //        var transactionHandler = new CacheTransactionHandler(new InMmemoryCache());

    //        AddInterceptor(transactionHandler);

    //        Loaded +=
    //          (sender, args) => args.ReplaceService<DbProviderServices>(
    //            (s, _) => new CachingProviderServices(s, transactionHandler,
    //              new DefaultCachingPolicy()));
    //    }
    //}
}