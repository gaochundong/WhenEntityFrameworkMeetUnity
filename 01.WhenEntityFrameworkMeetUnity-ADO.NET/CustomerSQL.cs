using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhenEntityFrameworkMeetUnity
{
  public static class CustomerSQL
  {
    public const string InsertNewCustomer = @" 
SET NOCOUNT ON;

INSERT INTO [RETAIL].[STORE].[Customer]
          ([Name]
          ,[Address]
          ,[Phone])
    VALUES
          (@name
          ,@address
          ,@phone);

SELECT CAST(scope_identity() AS bigint);

SET NOCOUNT OFF;
		";

    public const string UpdateExistingCustomer = @" 
SET NOCOUNT ON;

UPDATE [RETAIL].[STORE].[Customer]
SET
	[Name] = @name,
	[Address] = @address,
	[Phone] = @phone
WHERE Id = @customerId;

SET NOCOUNT OFF;
		";

    public const string GetAllCustomers = @" 
SET NOCOUNT ON;

SELECT *
FROM [RETAIL].[STORE].[Customer];

SET NOCOUNT OFF;
";

    public const string GetCustomersByAddress = @" 
SET NOCOUNT ON;

SELECT *
FROM [RETAIL].[STORE].[Customer]
WHERE Address = @address;

SET NOCOUNT OFF;
";

    public const string DeleteAllCustomers = @" 
SET NOCOUNT ON;

DELETE FROM [RETAIL].[STORE].[Customer];

SET NOCOUNT OFF;
";

    public const string DeleteCustomersByAddress = @" 
SET NOCOUNT ON;

DELETE FROM [RETAIL].[STORE].[Customer]
WHERE Address = @address;

SET NOCOUNT OFF;
";
  }
}
