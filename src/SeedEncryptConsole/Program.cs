using NBitcoin;
using NBitcoin.Protocol;
using SeedEncrypt;
using SeedEncryptConsole;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Linq;

Console.OutputEncoding = Encoding.UTF8;

var app = new CommandApp<MainCommand>();

app.Run(args);

