namespace GateController.API.Middlewares.WebsocketMiddleware
{
    public static class WSServerExtensions
    {
        public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder builder)
        {
            builder.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120)
            });
            return builder.UseMiddleware<WSServerMiddleware>();
        }

        public static IServiceCollection AddWSServerConnectionManager(this IServiceCollection services)
        {
            services.AddSingleton<WSServerConnectionManager>();
            services.AddSingleton<WSServerHandler>();
            services.AddSingleton<WSServerActions>();
            return services;
        }
    }
}
