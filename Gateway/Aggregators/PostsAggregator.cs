using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using Ocelot.Request.Middleware;
using Ocelot.Responses;
using System.Net;
using System.Text;

namespace Gateway.Aggregators;

public class PostsAggregator : IDefinedAggregator
{
    private readonly ILogger<PostsAggregator> _logger;

    public PostsAggregator(ILogger<PostsAggregator> logger)
    {
        _logger = logger;
    }

    public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
    {
        var a1 = await responses[0].Items.DownstreamResponse().Content.ReadAsStringAsync();
        var a2 = await responses[1].Items.DownstreamResponse().Content.ReadAsStringAsync();
        _logger.LogInformation("{}", a1);
        _logger.LogInformation("{}", a2);

        var r = new
        {
            a1,
            a2
        };

        return new DownstreamResponse(
            new StringContent(JsonConvert.SerializeObject(r)),
            HttpStatusCode.OK,
            new List<Header>(),
            "OK");
    }
}
