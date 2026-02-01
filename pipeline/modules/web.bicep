param location string
param apiEndpoint string

resource swa 'Microsoft.Web/staticSites@2024-04-01' = {
    name: 'stapp-hkorg-web'
    location: location
    properties: {
        
    }

    resource config 'config@2024-04-01' = {
        name: 'appsettings'
        properties: {
            VITE_REACT_APP_API_ENDPOINT: apiEndpoint
        }
    }
}