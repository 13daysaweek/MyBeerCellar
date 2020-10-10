param location string = resourceGroup().location
param webAppName string

var farmName = '${webAppName}-farm'
var appInsightsName = '${webAppName}-insights'

resource webApp 'Microsoft.Web/sites@2018-11-01' = {
  name: webAppName
  location: location
  properties: {
      name: webAppName
      serverFarmId: farm.id
      siteConfig: {
          appSettings: [              
          ]
          connectionStrings: [              
          ]
      }
  }
}

resource farm 'Microsoft.Web/serverFarms@2018-11-01' = {
    name: farmName
    location: location
    sku: {
        name: 'S1'
        tier: 'Standard'
    }
    kind: 'linux'
    properties: {
        name: farmName
        workerSize: '0'
        workerSizeId: '0'
        numberOfWorkers: '1'
        reserved: true
    }
}

resource appInsights 'Microsoft.Insights/components@2018-05-01-preview' = {
    name: appInsightsName
    location: location
    kind: 'web'
    properties: {
        Application_Type: 'web'
        Request_Source: 'rest'
        RetentionInDays: 90
        publicNetworkAccessForIngestion: 'Enabled'
        publicNetworkAccessForQuery: 'Enabled'
    }
}