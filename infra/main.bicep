targetScope = 'subscription'

@minLength(1)
@description('Primary location for all resources')
param location string

@minLength(4)
@description('name of the resource group')
param rgName string

@description('The environment name created by Azure Dev CLI')
param environmentName string

var tags = {
  'azd-env-name': environmentName
}

var abbrs = loadJsonContent('./abbreviations.json')
var resourceToken = toLower(uniqueString(subscription().id))

resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: rgName
  location: location
  tags: tags
}

module monitoring './modules/monitoring/monitoring.bicep' = {
  name: 'monitoring'
  params: {
    location: location
    resourceToken: resourceToken
    abbrs: abbrs
  }
  scope: rg
}

module web 'modules/web/webapp.bicep' = {
  scope: rg
  name: 'web'
  params: {
    abbrs: abbrs    
    appInsightsName: monitoring.outputs.appInsightName
    location: location
    resourceToken: resourceToken
  }
}

module redis 'modules/cache/redis.bicep' = {
  scope: rg
  name: 'redis'
  params: {
    location: location
  }
}
