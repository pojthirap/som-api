Entity Framework Core :
https://docs.microsoft.com/en-us/ef/core/get-started/overview/install


#Use this script to generate entity classes and config for 1 table or many tables

Step 1: run powershell command to generate new classes, this is an example for Employees

dotnet ef dbcontext scaffold "Initial Catalog = MyApp; Data Source =.; Integrated Security = True; Pooling = true" Microsoft.EntityFrameworkCore.SqlServer -f --context-dir Entity --data-annotations --context-namespace Entity --namespace Entity --output-dir Entity

dotnet ef dbcontext scaffold "Initial Catalog = XXXX; Data Source =XXXX; User ID=XXXX; Password=XXXX; Integrated Security = false; Pooling = true" Microsoft.EntityFrameworkCore.SqlServer -f --context-dir Entity --data-annotations --context-namespace Entity --namespace Entity --output-dir Entity --schema dbo
