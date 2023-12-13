using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using fortnite_custom_launcher.Settings;
using System.Diagnostics;
using System.Text.Json;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();
builder.Configuration.Sources.Clear();

builder.Configuration.AddJsonFile("settings.json", false, true);

Settings config = new();
builder.Configuration.Bind(config);

using IHost host = builder.Build();

await host.StartAsync();

void menu(bool clear = true)
{
    if (clear) Console.Clear();

    Console.WriteLine("Select an option\n");

    Console.WriteLine("1. Run fortnite");
    Console.WriteLine("2. Change game path");
    Console.WriteLine("3. Change account credentials");

    var option = Console.ReadLine();

    string exePath = "FortniteGame/Binaries/Win64/FortniteClient-Win64-Shipping.exe";
    string fullPath = $"{config.Path}/${exePath}";

    switch (option)
    {
        case "1":
            if (!File.Exists(fullPath))
            {
                Console.WriteLine("Cannot find the executable, is game path correct?");
                menu(false);
                return;
            }
            Process.Start("cmd.exe", $"/c start {fullPath} -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -nobe -fromfl=eac -fltoken=24963ce04b575a5ca65526h0 -skippatchcheck -AUTH_LOGIN={config.Login} -AUTH_PASSWORD={config.Password} -AUTH_TYPE=epic");
            break;
        case "2":
            Console.Write("Enter new game path: ");
            config.Path = Console.ReadLine();

            File.WriteAllText("settings.json", JsonSerializer.Serialize(config));
            menu();
            break;
        case "3":
            Console.Write("Login: ");
            config.Login = Console.ReadLine();

            Console.Write("Password: ");
            config.Password = Console.ReadLine();

            File.WriteAllText("settings.json", JsonSerializer.Serialize(config));
            break;
    }
}

menu();