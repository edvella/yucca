# Using Yucca from the command line

On Windows, open a terminal and navigate to the Yucca project directory. The command is as follows:
```bash
yucca <arguments>
```

if using PowerShell:
```powershell
.\yucca <arguments>
```

Calling Yucca without any arguments will direct the user to use the help command to see available options:
```bash
yucca help
```

This will display a list of available commands and options for using Yucca from the command line. It is a summary of the information that is provided here.

### Examples of Common Commands
- To display information about the application:
```bash
yucca about
```

- To list all suppliers:
```bash
yucca supplier list
```

Listing suppliers supports output formats. If no format is specified, the table format is used. To specify an output format, use the `--format` option:
```bash
yucca supplier list --format <format>
```

Supported formats are:
- `table` (default)
- `json`
- `csv`

To view the full details of a particular supplier:
```bash
yucca supplier view --id <supplier-id>
```

- To add a supplier:
```bash
yucca supplier add "Supplier Name"
```
The supplier name is required and must be enclosed in quotes if it contains spaces. Other supplier details can be added using additional options during supplier record creation or at a later stage.

The available properties are:
- `--name` (required)
- `--address1` Address line 1
- `--address2` Address line 2
- `--city` City
- `--state` State
- `--postcode` Postal/ZIP code
- `--countryode <ISO>`   Country ISO code (e.g. US)
- `--phone` Contact phone
- `--email` Email address
- `--website` Website URL
- `--tax` Tax number

The same parameters can be used to update a supplier:
```bash
yucca supplier update --id <supplier-id> [--name "New Name"] [--address1 "New Address1"] ...
```

In this case, the `--id` parameter is required to identify which supplier to update. Other parameters are optional and can be used to modify specific fields of the supplier record.

- To remove a supplier:
```bash
yucca supplier remove --id <supplier-id>
```

This will delete the supplier with the specified ID from the system.

The supplier list can be exported directly to a file using:
```bash
yucca supplier export --file "suppliers.csv"
```

This will create a CSV file named `suppliers.csv` containing the list of all suppliers.

Optionally, the output can be redirected to a file or the printer using the list command with standard output redirection:
```bash
yucca supplier list > suppliers.txt
```
this will create a text file named `suppliers.txt` with the supplier list in table format.

```bash
yucca supplier list --format json > suppliers.json
```
this will result in the supplier data getting exported as json data.

```bash
yucca supplier list | lp
```

this will send the supplier list to the default printer.

In PowerShell, the same is achieved with:
```powershell
yucca supplier list | Out-Printer
```

## Data Migration

The `migrate` command allows you to import supplier data from the previous version of Yucca. It will read the data using the Yucca 2 Access database (.mdb) and replicate it to the currently active Yucca 3 database.

Notes: This command is only available on Windows.

The migration of the Supplier table only is supported at the moment. The Country Code field is also currently not supported and will be left empty during migration as local businesses tend to not set it anyway. This feature will be added in a future release.

### Migrate Suppliers from Access to SQL Server

```
yucca migrate --path <file-path> --password <database-password>
```

**Parameters:**
- `--path` (required) - Full path to the Yucca 2 Access database file (.mdb)
- `--password` (required) - Password for the Access database

**Example:**
```
yucca migrate --path "D:\data\suppliers.mdb" --password "MyDatabasePassword"
```

**What the command does:**
- Reads all supplier records from the specified Access database file
- Validates supplier data
- Migrates suppliers to the configured SQL Server database
- Displays migration progress and results
- Shows the total number of suppliers successfully migrated

**Requirements:**
- Windows platform (command is not available on Linux or macOS)
- Valid path to an Access database file
- Correct database password
- SQL Server connection configured in the application