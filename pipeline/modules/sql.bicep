param location string

resource server 'Microsoft.Sql/servers@2024-05-01-preview' = {
    
    name: 'sql-hkorg'
    location: location
    properties: {
        administratorLogin: 'hkadmin'
        administratorLoginPassword: 'epsilon99?'
    }
    
    resource database 'databases@2024-05-01-preview' = {
        name: 'HikeOrganiser'
        location: location
    }
}

output connectionString string = server.properties.fullyQualifiedDomainName