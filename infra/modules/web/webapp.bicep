param location string
param resourceToken string
param appInsightsName string
param abbrs object

resource appInsights 'Microsoft.Insights/components@2020-02-02-preview' existing = {
  name: appInsightsName
}

resource serverFarm 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: '${abbrs.webServerFarms}${resourceToken}'
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
  }
  kind: 'app'
}

resource weatherApi 'Microsoft.Web/sites@2020-06-01' = {
  name: '${abbrs.webSitesAppService}weatherapi-${resourceToken}'
  location: location  
  properties: {
    serverFarmId: serverFarm.id    
    siteConfig: {
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }  
      ]
      vnetRouteAllEnabled: true
      metadata: [
        {
          name: 'CURRENT_STACK'
          value: 'dotnet'
        }
      ]
      netFrameworkVersion: 'v8.0'
      alwaysOn: true      
    }    
    clientAffinityEnabled: false
    httpsOnly: true      
  }  
}
