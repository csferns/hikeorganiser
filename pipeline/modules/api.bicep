param location string
param sqlConnectionString string
param serviceBusConnectionString string

resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
    name: 'asp-hkorg'
    location: location
    kind: 'linux'
    sku: {
        tier: 'Basic'
        name: 'B1'
    }
    properties: {
        
    }
}

resource site 'Microsoft.Web/sites@2024-04-01' = {
    name: 'app-hkorg'
    location: location
    kind: 'web'
    properties: {
      httpsOnly: true
      serverFarmId: appServicePlan.id
      
      siteConfig: {
        linuxFxVersion: 'DOTNETCORE|9.0'
        minTlsVersion: '1.2'
        ftpsState: 'FtpsOnly'
      }
    }
    identity: {
      type: 'SystemAssigned'
    }

    resource config 'config' = {
        name: 'connectionstrings'
        properties: {
            HikeOrganiser: {
                type: 'SQLAzure'
                value: sqlConnectionString
            }
            ServiceBus: {
                type: 'ServiceBus'
                value: serviceBusConnectionString
            }
        }
    }
}

output endpoint string = site.properties.defaultHostName