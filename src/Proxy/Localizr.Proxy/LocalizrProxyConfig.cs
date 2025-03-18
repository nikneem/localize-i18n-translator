using Localizr.Core;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

namespace Localizr.Proxy;


public class SpreaViewProxyConfiguration : IProxyConfigProvider
{
    public SpreaViewProxyConfiguration()
    {
        var routeConfigs = new[]
        {
            new RouteConfig
            {
                RouteId = "membersRoute",
                ClusterId = "membersCluster",
                Match = new RouteMatch
                {
                    Path = "/members/{**catch-all}"
                }
            }
        };

        var clusterConfigs = new[]
        {
            new ClusterConfig
            {
                ClusterId = "membersCluster",
                LoadBalancingPolicy = LoadBalancingPolicies.RoundRobin,
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    {
                        "default", new DestinationConfig
                        {
                            Address = $"http://{ServiceName.MembersService}",
                            Health = $"http://{ServiceName.MembersService}/health",
                            Host = ServiceName.MembersService
                        }
                    }
                }
            }
        };

        _config = new LocalizrProxyConfig(routeConfigs, clusterConfigs);
    }

    private readonly LocalizrProxyConfig _config;

    public IProxyConfig GetConfig() => _config;

}


public class LocalizrProxyConfig : IProxyConfig
{
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public LocalizrProxyConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            Routes = routes;
            Clusters = clusters;
            ChangeToken = new CancellationChangeToken(_cts.Token);
        }

        public IReadOnlyList<RouteConfig> Routes { get; }

        public IReadOnlyList<ClusterConfig> Clusters { get; }

        public IChangeToken ChangeToken { get; }

        internal void SignalChange()
        {
            _cts.Cancel();
        }
}