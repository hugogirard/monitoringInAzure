param location string
param resourceToken string
param abbrs object

// var logAnalyticsWorkspaceName = '${abbrs.operationalInsightsWorkspaces}${resourceToken}'
// var appInsightsName = '${abbrs.applicationInsights}${resourceToken}'


var logAnalyticsWorkspaceName = 'log-webfarm'
var appInsightsName = 'apiinsights'

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: logAnalyticsWorkspaceName
  location: location  
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

output appInsightName string = appInsights.name
