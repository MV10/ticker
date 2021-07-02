# ticker

A simple command-line utility for managing very basic stock trading data. This makes it easy to evaluate annual profit and loss status, and to determine the age of a given share-lot for tax purposes (short- versus long-term capital gains). Fractional shares are supported, but more advanced trades like margin or options are not supported. All data is stored in the location of your choosing as a JSON serialization of a single `TickerData` object-graph using .NET's `System.Text.Json` (to avoid a JSON.NET [bug](https://github.com/JamesNK/Newtonsoft.Json/issues/1726) relating to the currency-friendly `decimal` data type). When .NET MAUI is more mature, I plan to add a companion GUI app.

Most switches begin a short session of interactive prompts. Hit CTRL+C to cancel any command in progress, data is only saved at the end of the interactive session.

| Switch | Description |
| --- | --- |
| -?, -help | list available switches (this list) |
| -path [dir] | show the storage path; specify [dir] to change it |
| -acct [id] | list accounts; specify [id] to set active account |
| -add | add new account (prompts) |
| -cash | cash transaction (prompts) |
| -trans | dump cash transaction history |
| [symbol] | dump transaction history of the symbol |
| -buy [symbol] | buy shares (prompts) |
| -sell [symbol] | sell shares (prompts) |
| -div [symbol] | buy divident reinvestment shares (prompts) |
| (none) | holdings summary of the active account |

This application and any associated documentation, code, articles, or repository content does not constitute financial, tax, or investment advice, and any and all liabilities are expressly disclaimed. Use at your own risk.
