// <auto-generated/>
using ApiSdk.Users;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Cli.Commons.IO;
using Microsoft.Kiota.Cli.Commons;
using Microsoft.Kiota.Serialization.Form;
using Microsoft.Kiota.Serialization.Json;
using Microsoft.Kiota.Serialization.Multipart;
using Microsoft.Kiota.Serialization.Text;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
namespace ApiSdk {
    /// <summary>
    /// The main entry point of the SDK, exposes the configuration and the fluent API.
    /// </summary>
    public class ApiClient : BaseCliRequestBuilder {
        /// <summary>
        /// Instantiates a new ApiClient and sets the default values.
        /// </summary>
        public Command BuildRootCommand() {
            var command = new RootCommand();
            command.Description = "Instantiates a new ApiClient and sets the default values.";
            command.AddCommand(BuildUsersNavCommand());
            return command;
        }
        /// <summary>
        /// The users property
        /// </summary>
        public Command BuildUsersNavCommand() {
            var command = new Command("users");
            command.Description = "The users property";
            var builder = new UsersRequestBuilder(PathParameters);
            var execCommands = new List<Command>();
            var nonExecCommands = new List<Command>();
            nonExecCommands.Add(builder.BuildCountNavCommand());
            execCommands.Add(builder.BuildCreateCommand());
            execCommands.Add(builder.BuildListCommand());
            var cmds = builder.BuildCommand();
            execCommands.AddRange(cmds.Item1);
            nonExecCommands.AddRange(cmds.Item2);
            foreach (var cmd in execCommands)
            {
                command.AddCommand(cmd);
            }
            foreach (var cmd in nonExecCommands.OrderBy(static c => c.Name, StringComparer.Ordinal))
            {
                command.AddCommand(cmd);
            }
            return command;
        }
        /// <summary>
        /// Instantiates a new ApiClient and sets the default values.
        /// </summary>
        public ApiClient() : base("{+baseurl}", new Dictionary<string, object>()) {
        }
    }
}
