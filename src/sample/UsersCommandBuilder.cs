using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Net.Http;

namespace Microsoft.Graph.Cli
{
    internal class UsersCommandBuilder
    {
        internal static Command BuildUsersCommand()
        {
            var command = new Command("users");
            command.Description = "Retrieve a list of user objects.";
            // Create options for all the parameters
            var consistencyLevelOption = new Option<string[]>("--consistency-level", description: "Indicates the requested consistency level. Documentation URL: https://docs.microsoft.com/graph/aad-advanced-queries")
            {
                Arity = ArgumentArity.ZeroOrMore
            };
            consistencyLevelOption.IsRequired = false;
            command.AddOption(consistencyLevelOption);
            var topOption = new Option<int?>("--top", description: "Show only the first n items")
            {
            };
            topOption.IsRequired = false;
            command.AddOption(topOption);
            var skipOption = new Option<int?>("--skip", description: "Skip the first n items")
            {
            };
            skipOption.IsRequired = false;
            command.AddOption(skipOption);
            var searchOption = new Option<string>("--search", description: "Search items by search phrases")
            {
            };
            searchOption.IsRequired = false;
            command.AddOption(searchOption);
            var filterOption = new Option<string>("--filter", description: "Filter items by property values")
            {
            };
            filterOption.IsRequired = false;
            command.AddOption(filterOption);
            var countOption = new Option<bool?>("--count", description: "Include count of items")
            {
            };
            countOption.IsRequired = false;
            command.AddOption(countOption);
            var orderbyOption = new Option<string[]>("--orderby", description: "Order items by property values")
            {
                Arity = ArgumentArity.ZeroOrMore
            };
            orderbyOption.IsRequired = false;
            command.AddOption(orderbyOption);
            var selectOption = new Option<string[]>("--select", description: "Select properties to be returned")
            {
                Arity = ArgumentArity.ZeroOrMore
            };
            selectOption.IsRequired = false;
            command.AddOption(selectOption);
            var expandOption = new Option<string[]>("--expand", description: "Expand related entities")
            {
                Arity = ArgumentArity.ZeroOrMore
            };
            expandOption.IsRequired = false;
            command.AddOption(expandOption);
            var queryOption = new Option<string>("--query");
            command.AddOption(queryOption);
            command.SetHandler(async (invocationContext) =>
            {
                var consistencyLevel = invocationContext.ParseResult.GetValueForOption(consistencyLevelOption);
                var top = invocationContext.ParseResult.GetValueForOption(topOption);
                var skip = invocationContext.ParseResult.GetValueForOption(skipOption);
                var search = invocationContext.ParseResult.GetValueForOption(searchOption);
                var filter = invocationContext.ParseResult.GetValueForOption(filterOption);
                var count = invocationContext.ParseResult.GetValueForOption(countOption);
                var orderby = invocationContext.ParseResult.GetValueForOption(orderbyOption);
                var select = invocationContext.ParseResult.GetValueForOption(selectOption);
                var expand = invocationContext.ParseResult.GetValueForOption(expandOption);
                var query = invocationContext.ParseResult.GetValueForOption(queryOption);
                var cancellationToken = invocationContext.GetCancellationToken();
                var requestAdapter = invocationContext.BindingContext.GetRequestAdapter();
                var requestInfo = new RequestInformation
                {
                    HttpMethod = Method.GET,
                    UrlTemplate = "https://graph.microsoft.com/v1.0/users{?%24top,%24skip,%24search,%24filter,%24count,%24orderby,%24select,%24expand}",
                    PathParameters = new Dictionary<string, object>(),
                };
                requestInfo.Headers.Add("Accept", "application/json");
                if (consistencyLevel != null) requestInfo.Headers.Add("ConsistencyLevel", consistencyLevel);
                var response = await requestAdapter.SendPrimitiveAsync<Stream>(requestInfo);
                if (response != null)
                {
                    var reader = new StreamReader(response);
                    Console.WriteLine(await reader.ReadToEndAsync());
                }
            });
            return command;
        }
    }
}
