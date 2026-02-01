param location string

var topics = ['EventCreated', 'EventUpdated']

resource storage 'Microsoft.Storage/storageAccounts@2023-05-01' = {
    name: 'sthkorg'
    location: location
    kind: 'BlobStorage'
    sku: {
        name: 'Standard_LRS'
    }
    properties: {
      accessTier: 'Cool'
      allowBlobPublicAccess: true
      minimumTlsVersion: 'TLS1_2'
    }
}

resource serviceBus 'Microsoft.ServiceBus/namespaces@2024-01-01' = {
    name: 'sbns-hkorg'
    location: location
    sku: {
        capacity: 1
        tier: 'Standard'
        name: 'Standard'
    }

    resource topic 'topics@2022-10-01-preview' = [for topic in topics: {
        name: 'sbt-${toLower(topic)}'
        properties: {
            defaultMessageTimeToLive: 'P14D'
        }
    }]

    resource authorizationRule 'authorizationRules@2022-10-01-preview' = {
        name: 'auth-rule'
        properties: {
            rights: [ 'Manage', 'Listen', 'Send' ]
        }
    }
}

#disable-next-line outputs-should-not-contain-secrets
output serviceBusConnectionString string = serviceBus::authorizationRule.listKeys().primaryConnectionString