# Beatles.Caching.Redis
An adaptor for a distributed cache implementation for the core caching framework.

### What does it do?
The provides a distributed cache facility, via Redis cache, using the StackExchange.Redis client and the StackExchange.Redis.Extensions.Newtonsoft (Json.NET) serializer.


### Why use it?
Its fast(ish). Its generally faster than your database, however its important to realize there is an RPC call to the redis server, as well as object serialization overhead.
This cache will provide a shared state model for your cache, which is, for all nodes in your application cluster, connecting to the same redis cache, share the same cache entries.

Some important things to remember:
* Redis is not an RDB, but commands are generally [transactional](https://redis.io/topics/transactions)
* The Redis cluster may fail, and all your cache entries will be wiped. You must assume this will occur, and take it into account.

In short:
* Adding *static* data, like reference data may be ok, however using [Beatles.Caching.Memory] (https://github.com/eShopWorld/beatles-caching-redis) could be a better solution
* Adding 'read only' *transactional* data to this cache is good, once you manage [cache coherence](https://en.wikipedia.org/wiki/Cache_coherence)
* Adding 'mutable' *transactional* data comes with a risk. You need to assess this, and determine if its worth the performance gain vs an alternate store.


#### Further reading
* Please refer to [Beatles.Caching](https://github.com/eShopWorld/beatles-caching) for full framework details
* Please refer to the [Redis](https://redis.io/) site for the specifics of the cache
* Please refer to [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis) for client details
* Please refer to [StackExchange.Redis.Extensions](https://github.com/imperugo/StackExchange.Redis.Extensions) and [Json.NET](http://www.newtonsoft.com/json) for serializer details

### IoC container registration
```c#
public class RedisCacheModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(c => new RedisCacheFactory(yourAppConfiguration.RedisConnectionString))
            .SingleInstance();

        // default resolution       ICache -> RedisCache
        builder.RegisterSource(new CacheRegistrationSource<RedisCacheFactory>(typeof(ICache<>)));

        // optional, only required if you use the specific interface
        // for distributed cache    IDistributedCache -> RedisCache
        builder.RegisterSource(new CacheRegistrationSource<RedisCacheFactory>(typeof(IDistributedCache<>)));
    }
}
```