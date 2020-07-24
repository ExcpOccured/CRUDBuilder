[![NuGet]()]()

# CRUD Provider

This is a C# [NpgSQL](https://www.npgsql.org/) CRUD provider which allows use the main basic functions(available for extensions) of persistent storage and then use them in F# or C# project.

## Examples

In C#:
[Usage examples](EXAMPLES.md)


## Status

| OS      | Build & Test |
|---------|--------------|
| Mac OS  | In progress|
| Linux   | In progress |
| Windows | In progress |

Paket is used to acquire the type provider SDK and build the nuget package.

Build:

    .\build.sh --target Build --Configuration Release

Pack:

    .\build.sh --target Pack --Configuration Release
    
## License   

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

