﻿namespace Restaurants.Infrastructure.Configuration;

public class BlobStorageSettings
{


    public string AccountKey { get; set; } = default!;


    public string ConnectionString { get;  set; } = default!;


    public string LogosContainerName { get;  set; } = default!;


}
