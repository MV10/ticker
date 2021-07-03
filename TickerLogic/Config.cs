using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TickerData;

namespace TickerLogic
{
    /// <summary>
    /// Application configuration is stored in the user's personal directory as "ticker.json"
    /// </summary>
    public class Config
    {
        private static JsonSerializerOptions jsonOpts = new JsonSerializerOptions { WriteIndented = true };

        /// <summary>
        /// Location of the ticker account files (which are named "Account.Id.tikr")
        /// </summary>
        public string DataPath { get; set; } = string.Empty;

        /// <summary>
        /// Account to load automatically for transaction or display commands.
        /// </summary>
        public string ActiveAccountId { get; set; } = string.Empty;

        /// <summary>
        /// Populates a new Config object, or returns null if the configuration isn't found or is invalid.
        /// </summary>
        public static async ValueTask<Config> Read()
        {
            var pathname = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ticker.json");
            if (!File.Exists(pathname)) return null;
            var json = await File.ReadAllTextAsync(pathname);
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                return JsonSerializer.Deserialize<Config>(json);
            }
            catch(Exception ex) when (ex is JsonException || ex is NotSupportedException)
            {
                return null;
            }
        }

        /// <summary>
        /// Prompts the user for new configuration settings, saves them, and returns a new Config object.
        /// </summary>
        public static async ValueTask<Config> Prompt()
        {
            var cfg = new Config();
            Console.WriteLine("The application has not been configured for your user profile.");
            var path = ConsoleInput.GetPath("Enter the path to the directory to store data files, or leave blank for your local user path.", allowBlank: true);
            cfg.DataPath = string.IsNullOrEmpty(path) ? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) : path;
            await cfg.Write();
            return cfg;
        }

        /// <summary>
        /// Writes the configuration to the user's personal directory.
        /// </summary>
        public async ValueTask Write()
        {
            var json = JsonSerializer.Serialize(this, jsonOpts);
            var pathname = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ticker.json");
            await File.WriteAllTextAsync(pathname, json);
            Console.WriteLine("Configuration saved.\n");
        }

        /// <summary>
        /// Loads the active account.
        /// </summary>
        public async ValueTask<Account> GetActiveAccount()
        {
            if (string.IsNullOrWhiteSpace(DataPath) || string.IsNullOrWhiteSpace(ActiveAccountId))
                throw new ArgumentNullException("Invalid configuration (missing data path, or no account is active)");

            if (!Directory.Exists(DataPath))
                throw new InvalidOperationException($"Unable to find data path: {DataPath}");

            var pathname = Path.Join(DataPath, $"{ActiveAccountId}.tikr");
            if(!File.Exists(pathname))
                throw new InvalidOperationException($"Unable to find account data file: {pathname}");

            var json = await File.ReadAllTextAsync(pathname);
            var acct = JsonSerializer.Deserialize<Account>(json);

            Console.WriteLine($"Loaded account \"{acct.Id}\"\n{acct.Name}\n");
            return acct;
        }

        public async ValueTask WriteAccount(Account acct)
        {
            if (!Directory.Exists(DataPath))
                throw new InvalidOperationException($"Unable to find data path: {DataPath}");

            var pathname = Path.Join(DataPath, $"{acct.Id}.tikr");
            var json = JsonSerializer.Serialize(acct, jsonOpts);
            await File.WriteAllTextAsync(pathname, json);
            Console.WriteLine($"Account saved: {pathname}");
        }
    }
}
