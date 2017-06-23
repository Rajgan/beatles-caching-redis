using System;
using System.Collections.Generic;
using Xunit;    
using StackExchange.Redis;

namespace Beatles.Caching.Caches.Redis.Tests
{
    public class RedisCacheFactoryTests
    {
        [Fact]
        public void NewInstace_WithInvalidConnectionString_ThrowsException()
        {
            // Arrange
            // Act

            Assert.Throws<RedisConnectionException>((Action)(() => new RedisCacheFactory("1.1.1.1:6379"))); // this is a fake address
            // Assert
        }

        [Fact]
        public void NewInstace_WithValidConnectionString_NoException()
        {
            // Arrange
            //RedisCacheFactory factory;
            // Act
            using (new RedisCacheFactory(Environment.RedisConnectionString)) ;
            // Assert
        }

        [Fact]
        public void Create_RedisCacheFactory_NoException()
        {
            // Arrange
            using (var factory = new RedisCacheFactory(Environment.RedisConnectionString))
            {
                // Act
                var instance = factory.Create<SimpleObject>("testCache");

                // Assert
                Assert.IsType<RedisCache<SimpleObject>>(instance);
            }
        }
    }
}
