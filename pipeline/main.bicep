var location = resourceGroup().location

module sql 'modules/sql.bicep' = {
    name: '${deployment().name}-sql'
    params: {
        location: location
    }
}

module storage 'modules/storage.bicep' = {
    name: '${deployment().name}-str'
    params: {
        location: location
    }
}

module api 'modules/api.bicep' = {
    name: '${deployment().name}-api'
    params: {
        location: location
        sqlConnectionString: sql.outputs.connectionString
        serviceBusConnectionString: storage.outputs.serviceBusConnectionString
    }
}

module web 'modules/web.bicep' = {
    name: '${deployment().name}-web'
    params: {
        location: location
        apiEndpoint: api.outputs.endpoint
    }
}

output webApiEndpoint string = api.outputs.endpoint
