using System;

namespace ExampleRedisClient {
    // Anti-pattern; convert static classes to real dependency-injection.
    public static class SingletonServices {
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
